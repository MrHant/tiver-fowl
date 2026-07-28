# Logging and HTML Reporting

The HTML report is generated *from* the structured log file, so the two subsystems are one story:
Serilog writes structured events during the run, and `HtmlReportGenerator` parses them afterwards.
No code instrumentation beyond normal logging is required.

## Logging

Uses **Serilog** with structured logging:

- Writes to console and to a JSON file (`log.txt` in the test output directory)
- Test name added automatically via `TestNameEnricher`
- Session id added automatically via `SessionIdEnricher`
- Element actions logged via `ILoggableElementExtensions.LogAction()`
- Screenshots logged as base64 strings

### Log types

| `LogType` | Meaning |
| --------- | ------- |
| `TestResult` | Test pass/fail/skip |
| `ElementAction` | Click, Type, etc. |
| `Screenshot` | Base64 image data |
| `TestStep` | Custom step logging via `this.LogStep("message")` |

Serilog is configured in `Logger.Configure()`, which ships as a source file
(`Tiver.Fowl/contentFiles/cs/any/Logging/Logger.cs`) so consumers can customize it.

## HTML report generation

- Generated automatically in `Flow.SessionTeardown()` after all tests complete
- Parses `log.txt` to extract test execution data
- Self-contained single HTML file with embedded CSS/JS
- Each session produces its own file: `test-report-{SessionId}.html`

### Report contents

- Summary statistics (total, passed, failed, skipped, duration)
- Tests grouped by namespace with collapsible sections
- Per test: name, result, duration, steps, element actions
- Screenshots embedded as base64 for failed tests
- Interactive filtering by result (all/passed/failed/skipped)
- Search by test name

```
Test Report
├─ Summary: Total/Passed/Failed/Skipped/Duration
├─ Filters: All | Passed | Failed | Skipped | Search
└─ Namespaces (collapsible)
    └─ Tests (click to expand)
        ├─ Error message & stack trace (if failed)
        ├─ Screenshot (if failed)
        └─ Steps & Element Actions
```

### Generating a sample report

`task report-demo` runs the `[Explicit]` fixtures in `Tests/Demo/` to produce a sample report
covering passing, failing-with-screenshot and skipped tests across two groups, then prints the
report path. One of its tests fails by design, so the task ignores the non-zero exit code.

## Session identification

- SessionId format: `yyyyMMdd-HHmmss-fff-RRRR` (date, time, milliseconds, random suffix)
- Set automatically in `Logger.Configure()` via `TestExecutionContext.SessionId`
- Separates multiple runs within an accumulated `log.txt`
- Milliseconds plus random suffix prevent collisions in parallel or rapid executions

## Template customization

- Template file: `Tiver.Fowl/Core/Reporting/report-template.html`
- Copied to the output directory via `.csproj` configuration
- Uses `{{PLACEHOLDER}}` tokens: `{{TITLE}}`, `{{SUMMARY}}`, `{{TESTS}}`, `{{FOOTER}}`
- Styled with Tailwind CSS and DaisyUI loaded via CDN
- Customize by editing Tailwind/DaisyUI classes or adding CSS to the template `<style>` block
- Loaded from several fallback locations (current directory, base directory, assembly locations)

## Related classes

All in `Tiver.Fowl/Core/Reporting/` unless noted:

| Class | Responsibility |
| ----- | -------------- |
| `LogFileParser` | Parses Serilog JSON log entries, with validation and error logging |
| `HtmlReportGenerator` | Template-based HTML generation with fallback path resolution |
| `TestResultRecord`, `TestStepRecord`, `ElementActionRecord` | Data models |
| `SessionIdEnricher` | Adds SessionId to all log events (`Tiver.Fowl/Logging/`) |

## Troubleshooting

| Symptom | Check |
| ------- | ----- |
| No report generated | Console output for a "Failed to generate test report" error |
| Template not found | Error message lists every searched location; confirm `.csproj` has `<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>` |
| Empty or partial report | `log.txt` must contain valid JSON, one object per line |
| Missing entries | Malformed JSON entries are logged as warnings and skipped — partial parsing is supported by design |
