namespace Tiver.Fowl.Reporting
{
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
            var testResults = lines.Where(l => l["Properties"]["LogType"]?.Value<string>() == "Outcome");


            // Render report
            var indexTemplateRaw = File.ReadAllText("./templates/index.hbs");
            var indexTemplate = Handlebars.Compile(indexTemplateRaw);

            CompilePartial("test_result_row");
            CompilePartial("foundation_style");
            CompilePartial("report_style");
            CompilePartial("jquery_javascript");
            CompilePartial("foundation_javascript");

            IApplicationConfiguration config = (ApplicationConfigurationSection) ConfigurationManager.GetSection("applicationConfigurationGroup/applicationConfiguration");
            var data = new
            {
                application_title = config.Title,
                test_results =
                    testResults.Select(t =>
                        new
                        {
                            test_name = t["Properties"]["TestName"],
                            status = t["Properties"]["Outcome"],
                            status_color = (t["Properties"]["Outcome"].Value<string>() == "Passed") ? "green" : "red"
                        })
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
