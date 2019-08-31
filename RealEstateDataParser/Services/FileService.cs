using System;
using System.Collections.Generic;
using System.IO;

namespace SalesParser.Services {

    public class FileService {

        public FileService() {
        }

        public IEnumerable<string> ReadAllLines(DateTime dateTime) {
            var DateString = dateTime.Date.ToString("yyyy-MM-dd");
            var DateFile = $"{DateString}.txt";
            var filePath = Path.Combine("Data", DateString, DateFile);
            return File.ReadAllLines(filePath.ToString());
        }

        public void WriteAllLines(DateTime dateTime, string prefix, IEnumerable<string> lines) {
            var DateString = dateTime.Date.ToString("yyyy-MM-dd");
            var DateFile = string.IsNullOrEmpty(prefix)
                ? $"{DateString}_report.txt"
                : $"{prefix}_{DateString}_report.txt";

            var filePath = Path.Combine("Data", DateString, DateFile);

            File.WriteAllLines(filePath, lines);
        }
    }
}