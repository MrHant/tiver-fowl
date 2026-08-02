namespace Tiver.Fowl.Core.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using Enums;
    using Serilog;

    /// <summary>
    /// Parses Serilog JSON log files to extract test execution data for reporting
    /// </summary>
    public static class LogFileParser
    {
        /// <summary>
        /// Parse a Serilog JSON log file and extract test results grouped by session.
        /// Malformed entries are logged and skipped to allow partial parsing.
        /// </summary>
        /// <param name="logFilePath">Absolute path to the log.txt file</param>
        /// <returns>Dictionary keyed by SessionId containing list of test results</returns>
        /// <exception cref="ArgumentException">If log file path is null or empty</exception>
        /// <exception cref="FileNotFoundException">If log file doesn't exist</exception>
        /// <exception cref="IOException">If log file cannot be read</exception>
        public static Dictionary<string, List<TestResultRecord>> Parse(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
                throw new ArgumentException("Log file path cannot be null or empty", nameof(logFilePath));

            if (!File.Exists(logFilePath))
                throw new FileNotFoundException($"Log file not found: {logFilePath}", logFilePath);

            var sessions = new Dictionary<string, Dictionary<string, TestResultRecord>>();
            var startTimes = new Dictionary<string, DateTime>();
            var endTimes = new Dictionary<string, DateTime>();

            foreach (var line in File.ReadLines(logFilePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var entry = JsonSerializer.Deserialize<LogEntry>(line);
                    if (entry?.Properties == null)
                        continue;

                    var sessionId = entry.Properties.SessionId ?? "unknown";
                    var testName = entry.Properties.TestName;

                    if (!sessions.ContainsKey(sessionId))
                        sessions[sessionId] = new Dictionary<string, TestResultRecord>();

                    if (string.IsNullOrEmpty(testName))
                        continue;

                    var tests = sessions[sessionId];
                    var testKey = $"{sessionId}:{testName}";

                    if (!tests.ContainsKey(testName))
                    {
                        tests[testName] = new TestResultRecord { TestName = testName };
                        startTimes[testKey] = entry.Timestamp;
                    }
                    endTimes[testKey] = entry.Timestamp;

                    var record = tests[testName];

                    switch (entry.Properties.LogType)
                    {
                        case "TestResult":
                            record.Result = ParseTestResult(entry.Properties.TestResult);
                            break;

                        case "TestStep":
                            if (!entry.Properties.Step.HasValue || entry.Properties.Step.Value == 0)
                            {
                                Log.Warning("TestStep entry missing Step number for test '{TestName}', skipping step", testName);
                                break;
                            }
                            record.AddStep(new TestStepRecord
                            {
                                StepNumber = entry.Properties.Step.Value,
                                Description = entry.Properties.Text ?? string.Empty,
                                Timestamp = entry.Timestamp
                            });
                            break;

                        case "ElementAction":
                            // Validate required properties
                            if (string.IsNullOrEmpty(entry.Properties.Type) ||
                                string.IsNullOrEmpty(entry.Properties.Action))
                            {
                                Log.Debug("ElementAction entry missing required properties (Type or Action) for test '{TestName}', skipping", testName);
                                break;
                            }

                            var action = new ElementActionRecord
                            {
                                ElementType = entry.Properties.Type,
                                ElementName = entry.Properties.Name ?? "(unnamed)",
                                Action = entry.Properties.Action,
                                Timestamp = entry.Timestamp
                            };

                            if (record.CurrentStep != null)
                                record.CurrentStep.AddAction(action);
                            else
                                record.AddPreStepAction(action);
                            break;

                        case "Screenshot":
                            if (!string.IsNullOrEmpty(entry.Properties.Base64))
                                record.ScreenshotBase64 = entry.Properties.Base64;
                            break;
                    }

                    if (entry.Level == "Error" && !string.IsNullOrEmpty(entry.Exception))
                    {
                        record.ErrorMessage = entry.MessageTemplate;
                        record.StackTrace = entry.Exception;
                    }
                }
                catch (JsonException ex)
                {
                    Log.Warning(ex, "Failed to parse log entry at line position (skipping): {Line}",
                        line.Length > 100 ? line.Substring(0, 100) + "..." : line);
                    continue;
                }
            }

            // Set timestamps
            foreach (var (sessionId, tests) in sessions)
            {
                foreach (var (testName, record) in tests)
                {
                    var key = $"{sessionId}:{testName}";
                    if (startTimes.TryGetValue(key, out var start))
                        record.StartTime = start;
                    if (endTimes.TryGetValue(key, out var end))
                        record.EndTime = end;
                }
            }

            return sessions.ToDictionary(s => s.Key, s => s.Value.Values.ToList());
        }

        private static TestResult ParseTestResult(string? resultString)
        {
            var result = resultString?.ToLowerInvariant() switch
            {
                "passed" => TestResult.Passed,
                "failed" => TestResult.Failed,
                _ => TestResult.Unknown
            };

            if (result == TestResult.Unknown && !string.IsNullOrEmpty(resultString))
            {
                Log.Warning("Unrecognized test result value: '{Result}', treating as Unknown", resultString);
            }

            return result;
        }

        // Deserialization targets for Serilog's JSON lines. Every member is nullable because a given
        // log entry carries only the properties relevant to it - an element action has Name and
        // Action but no TestResult, a test completion the reverse.
        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string? Level { get; set; }
            public string? MessageTemplate { get; set; }
            public string? Exception { get; set; }
            public LogProperties? Properties { get; set; }
        }

        private class LogProperties
        {
            public string? SessionId { get; set; }
            public string? TestName { get; set; }
            public string? LogType { get; set; }
            public string? TestResult { get; set; }
            public int? Step { get; set; }
            public string? Text { get; set; }
            public string? Type { get; set; }
            public string? Name { get; set; }
            public string? Action { get; set; }
            public string? Base64 { get; set; }
        }
    }
}
