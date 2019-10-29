using Newtonsoft.Json;
using System.Collections.Generic;

namespace RealEstateDataParser.Services {
    public class BussinessDaysLookUpService {
        private class MonthlyBussinessDays {
            public int BusinessDays { get; set; }
            public IEnumerable<int> Holidays { get; set; }
        }

        private readonly Dictionary<int, string> monthToStringMap = new Dictionary<int, string>() {
            { 1, "january" },
            { 2, "february" },
            { 3, "march" },
            { 4, "april" },
            { 5, "may" },
            { 6, "june" },
            { 7, "july" },
            { 8, "august" },
            { 9, "september" },
            { 10, "october" },
            { 11, "november" },
            { 12, "december" }
        };

        private Dictionary<string, Dictionary<string, Dictionary<string, MonthlyBussinessDays>>> bussinessObject;

        private Dictionary<string, Dictionary<string, Dictionary<string, MonthlyBussinessDays>>> BussinessObject {
            get {
                if (bussinessObject == null) {
                    var jsonString = FileService.ReadAllText(ConfigurationService.Configuration["businessDaysFile"]);
                    bussinessObject = JsonConvert.DeserializeObject<
                        Dictionary<string,
                            Dictionary<string,
                                Dictionary<string,
                                    MonthlyBussinessDays>>>>(jsonString);
                }

                return bussinessObject;
            }
        }

        public BussinessDaysLookUpService() {
        }

        public int GetNumberOfBusinessDaysInMonth(int year, int month) {
            return BussinessObject["canada"][year.ToString()][monthToStringMap[month]].BusinessDays;
        }

        public IEnumerable<int> GetHolidaysInMonth(int year, int month) {
            return BussinessObject["canada"][year.ToString()][monthToStringMap[month]].Holidays;
        }
    }
}
