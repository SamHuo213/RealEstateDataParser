using System.Collections.Generic;

namespace RealEstateDataParser.Maps {

    public class PropertyTypeKeyMap {

        public static readonly Dictionary<string, string> PropertyTypeToKeyMap = new Dictionary<string, string>() {
            { "1/2 duplex", "TownHouse" },
            { "townhouse", "TownHouse" },
            { "house", "House" },
            { "house with acreage", "House" }
        };

        public static readonly Dictionary<string, string> SalPropertyTypeToKeyMap = new Dictionary<string, string>() {
            { "townhouse", "TownHouse" },
            { "house", "House" },
            { "apartment", "Apartment" }
        };

        public static string GetPropertyTypeKey(string key) {
            var lowerCaseKey = key.ToLower();
            var keyExist = PropertyTypeToKeyMap.ContainsKey(lowerCaseKey);
            if ( keyExist ) {
                return PropertyTypeToKeyMap[lowerCaseKey];
            }

            return key;
        }

        public static string GetSalPropertyTypeKey(string key) {
            var lowerCaseKey = key.ToLower();
            var keyExist = SalPropertyTypeToKeyMap.ContainsKey(lowerCaseKey);
            if ( keyExist ) {
                return SalPropertyTypeToKeyMap[lowerCaseKey];
            }

            return "Everything else";
        }
    }
}