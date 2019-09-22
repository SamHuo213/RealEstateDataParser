using RealEstateDataParser.Enums;
using System.Collections.Generic;

namespace RealEstateDataParser.Maps {
    public class OverUnderEnumToStringMap {
        public static readonly Dictionary<OverUnder, string> OverUnderEnumToString = new Dictionary<OverUnder, string>() {
            { OverUnder.over, "Sold Over Asking Price" },
            { OverUnder.under, "Sold Under Asking Price" },
            { OverUnder.soldAt, "Sold At Asking Price" }
        };

        public static string GetOverUnderString(OverUnder key) {
            var keyExist = OverUnderEnumToString.ContainsKey(key);
            if (keyExist) {
                return OverUnderEnumToString[key];
            }

            return "";
        }
    }
}
