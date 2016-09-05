namespace Tiver.Fowl.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using Core.Configuration;
    using HandlebarsDotNet;
    using Newtonsoft.Json.Linq;
    using System.Linq;

    public static class Report
    {
        public static void Build()
        {
            // Parse log of tests
            var lines = new List<JObject>();
            using (var fs = new FileStream("./log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(JObject.Parse(line));
                }
            }
            var testResults = lines.GroupBy(l => l["Properties"]["TestName"]);


            // Render report
            var indexTemplateRaw = File.ReadAllText("./templates/index.hbs");
            var indexTemplate = Handlebars.Compile(indexTemplateRaw);

            CompilePartial("test_result_row");
            CompilePartial("foundation_style");
            CompilePartial("report_style");
            CompilePartial("jquery_javascript");
            CompilePartial("foundation_javascript");

            IApplicationConfiguration config = (ApplicationConfigurationSection) ConfigurationManager.GetSection("applicationConfigurationGroup/applicationConfiguration");

            var testResultsData = new List<dynamic>();

            foreach (var testResult in testResults)
            {
                var outcome = testResult.Single(d => d["Properties"]["LogType"]?.Value<string>() == "Outcome");

                var tempActions = new List<dynamic>();

                foreach (var detail in testResult.Where(d => d["Properties"]["LogType"]?.Value<string>() == "ElementAction"))
                {
                    tempActions.Add(new
                    {
                        element = detail["Properties"]["Name"].Value<string>(),
                        action = detail["Properties"]["Action"].Value<string>(),
                    });
                }

                var temp = new
                {
                    test_report_id = Guid.NewGuid().ToString("D"),
                    test_name = testResult.Key.Value<string>(),
                    status = outcome["Properties"]["Outcome"].Value<string>(),
                    status_color = (outcome["Properties"]["Outcome"].Value<string>() == "Passed") ? "green" : "red",
                    actions = tempActions.ToArray()
                };

                testResultsData.Add(temp);
            }

            var data = new
            {
                application_title = config.Title,
                test_results = testResultsData.ToArray()
            };

            var resultRaw = indexTemplate(data);
            using (var sw = new StreamWriter("./report.html"))
            {
                sw.WriteLine(resultRaw);
            }
        }

        /// <summary>
        /// Compile template file and include it in result
        /// 
        /// Template file should be in "./templates" directory and be named same as template variable
        /// starting with underscore
        /// </summary>
        /// <param name="partialName"></param>
        private static void CompilePartial(string partialName)
        {
            var templateRaw = File.ReadAllText($"./templates/_{partialName}.hbs");
            using (var reader = new StringReader(templateRaw))
            {
                var template = Handlebars.Compile(reader);
                Handlebars.RegisterTemplate(partialName, template);
            }
        }
    }
}
