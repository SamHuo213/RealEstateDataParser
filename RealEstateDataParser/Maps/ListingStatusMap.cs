using RealEstateDataParser.Enums;
using System.Collections.Generic;

namespace RealEstateDataParser.Maps {

    public class ListingStatusMap {

        public static readonly Dictionary<string, ListingStatus> ListingStatusStringToEnum = new Dictionary<string, ListingStatus>() {
            { ListingStatusStrings.SoldString, ListingStatus.sold },
            { ListingStatusStrings.ActiveString, ListingStatus.active },
            { ListingStatusStrings.CancelProtectedString, ListingStatus.cancelProtected },
            { ListingStatusStrings.TerminatedString, ListingStatus.terminated },
            { ListingStatusStrings.ExpiredString, ListingStatus.expired }
        };

        public static ListingStatus? GetListingStatusEnum(string key) {
            var keyLowerCase = key.ToLower();
            var keyExist = ListingStatusStringToEnum.ContainsKey(keyLowerCase);
            if ( keyExist ) {
                return ListingStatusStringToEnum[keyLowerCase];
            }

            return null;
        }
    }
}