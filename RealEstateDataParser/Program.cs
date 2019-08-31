using System;

namespace SalesParser {

    public class Program {

        private static void Main(string[] args) {
            var app = new App();
            app.Run(GetReportDate(args));
        }

        private static DateTime GetReportDate(string[] args) {
            if (args.Length == 0) {
                return DateTime.Now;
            }

            var reportDateString = args[0];
            if (string.IsNullOrEmpty(reportDateString)) {
                return DateTime.Now;
            }

            var success = DateTime.TryParse(reportDateString, out DateTime reportDate);
            return (success) ? reportDate : DateTime.Now;
        }
    }
}