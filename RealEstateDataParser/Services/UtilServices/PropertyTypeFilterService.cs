using SalesParser.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateDataParser.Services.UtilServices {

    public class PropertyTypeFilterService {

        private static readonly HashSet<string> propertyTypeSet = new HashSet<string>() {
            { "1/2 duplex" },
            { "townhouse" },
            { "house" },
            { "house with acreage" },
            { "apartment" }
        };

        public static IEnumerable<UnitEntry> GetParsedUnitEntries(IEnumerable<UnitEntry> unitEntries) {
            return unitEntries.Where(x => propertyTypeSet.Contains(x.Type.ToLowerInvariant()));
        }
    }
}