using RealEstateDataParser.Enums;

namespace RealEstateDataParser.Services.UtilServices {

    public class ListingStatusValidationService {

        public static bool ListingStatusIsCorrect(string[] rawEntry, ListingStatus? listingStatus) {
            var hasListingStatus = rawEntry.Length >= 140;

            if ( hasListingStatus ) {
                return ListingStatusIsCorrectWithListingStatus(rawEntry, listingStatus);
            }

            return ListingStatusIsCorrectWithoutListingStatus(rawEntry, listingStatus);
        }

        private static bool ListingStatusIsCorrectWithListingStatus(string[] rawEntry, ListingStatus? listingStatus) {
            var status = rawEntry[139].ToLower();
            if ( listingStatus == ListingStatus.sold ) {
                return status == ListingStatusStrings.SoldString && IsValidSoldEntry(rawEntry[33]);
            } else if ( listingStatus == ListingStatus.active ) {
                return status == ListingStatusStrings.ActiveString && IsValidActiveEntry(rawEntry[33]);
            }

            return status == ListingStatusStrings.CancelProtectedString ||
                status == ListingStatusStrings.TerminatedString ||
                status == ListingStatusStrings.ExpiredString;
        }

        private static bool ListingStatusIsCorrectWithoutListingStatus(string[] rawEntry, ListingStatus? listingStatus) {
            var soldDateString = rawEntry[33];
            if ( listingStatus == ListingStatus.sold && string.IsNullOrEmpty(soldDateString) ) {
                return false;
            } else if ( listingStatus != ListingStatus.sold && !string.IsNullOrEmpty(soldDateString) ) {
                return false;
            }

            return true;
        }

        private static bool IsValidSoldEntry(string soldDateString) {
            return !string.IsNullOrEmpty(soldDateString);
        }

        private static bool IsValidActiveEntry(string soldDateString) {
            return string.IsNullOrEmpty(soldDateString);
        }
    }
}