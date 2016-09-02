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
            var testResults = lines.Where(l => l["Properties"]["LogType"]?.Value<string>() == "Outcome");


            // Render report
            var indexTemplateRaw = File.ReadAllText("./templates/index.hbs");
            var indexTemplate = Handlebars.Compile(indexTemplateRaw);

            var testResultTemplateRaw = File.ReadAllText("./templates/test_result_row.hbs");
            using (var reader = new StringReader(testResultTemplateRaw))
            {
                var testResultTemplate = Handlebars.Compile(reader);
                Handlebars.RegisterTemplate("test_result", testResultTemplate);
            }

            IApplicationConfiguration config = (ApplicationConfigurationSection) ConfigurationManager.GetSection("applicationConfigurationGroup/applicationConfiguration");
            var data = new
            {
                application_title = config.Title,
                test_results =
                    testResults.Select(t =>
                        new
                        {
                            test_name = t["Properties"]["TestName"],
                            status = t["Properties"]["outcome"],
                            status_color = (t["Properties"]["outcome"].Value<string>() == "Passed") ? "green" : "red"
                        })
            };
            var resultRaw = indexTemplate(data);
            using (var sw = new StreamWriter("./report.html"))
            {
                sw.WriteLine(resultRaw);
            }
        }
    }
}
