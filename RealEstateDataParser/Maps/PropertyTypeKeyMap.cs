using System.Collections.Generic;

namespace RealEstateDataParser.Maps {
    public class PropertyTypeKeyMap {
        public static readonly Dictionary<string, string> PropertyTypeToKeyMap = new Dictionary<string, string>() {
            { "1/2 duplex", "TownHouse" },
            { "townhouse", "TownHouse" },
            { "house", "House" },
            { "house with acreage", "House" }
        };

        public static string GetPropertyTypeKey(string key) {
            var lowerCaseKey = key.ToLower();
            var keyExist = PropertyTypeToKeyMap.ContainsKey(lowerCaseKey);
            if (keyExist) {
                return PropertyTypeToKeyMap[lowerCaseKey];
            }

            return key;
        }
    }
}
