using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestParser {
    public class Program {
        public class ErrorClass {
            public int code;
        }

        public class TestClass {
            public ErrorClass error;
            public IEnumerable<IEnumerable<string>> rows;
        }

        private static IEnumerable<string> ConvertToDesiredFormat(IEnumerable<IEnumerable<string>> rows) {
            var formatedReturn = new List<string>();
            foreach (var row in rows) {
                var newRow = string.Join("\t", row);
                formatedReturn.Add(newRow);
            }

            return formatedReturn;
        }

        public static void Main() {
            IEnumerable<string> outputFile = new List<string>();
            foreach (string file in Directory.EnumerateFiles(Path.Combine("Data"), "*.txt")) {
                using (var reader = new StreamReader(file)) {
                    string test = reader.ReadToEnd();
                    var test2 = JsonConvert.DeserializeObject<TestClass>(test);
                    if (test2.rows.First().Count() < 138) {
                        continue;
                    }

                    outputFile = outputFile.Concat(ConvertToDesiredFormat(test2.rows));
                }
            }

            var outputPath = Path.Combine("Data", "2019-09-05.txt");
            File.WriteAllLines(outputPath, outputFile);
        }
    }
}
