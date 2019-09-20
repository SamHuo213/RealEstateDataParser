using System.Collections.Generic;

namespace RealEstateDataParser.Maps {
    public class CityKeyMap {
        public static readonly Dictionary<string, string> CityToKeyMap = new Dictionary<string, string>() {
            { "burnaby east", "Burnaby" },
            { "burnaby north", "Burnaby" },
            { "burnaby south", "Burnaby" }
        };

        public static string GetCityKey(string key) {
            var lowerCaseKey = key.ToLower();
            var keyExist = CityToKeyMap.ContainsKey(lowerCaseKey);
            if (keyExist) {
                return CityToKeyMap[lowerCaseKey];
            }

            return key;
        }
    }
}
