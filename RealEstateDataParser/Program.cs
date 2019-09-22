using RealEstateDataParser.Services;
using System;

namespace SalesParser {
    public class Program {
        private static void Main(string[] args) {
            var app = new App();
            SetMode(args);
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

        private static void SetMode(string[] args) {
            if (args.Length < 2) {
                return;
            }

            var mode = args[1];
            if (!string.IsNullOrWhiteSpace(mode)) {
                ConfigurationService.Mode = mode;
            }
        }
    }
}
