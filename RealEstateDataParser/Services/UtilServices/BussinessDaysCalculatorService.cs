using System;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateDataParser.Services.UtilServices {

    public class BussinessDaysCalculatorService {
        private static readonly Dictionary<string, int> bussinessDayCountMap = new Dictionary<string, int>();

        public static int GetBusinessDays(DateTime start, DateTime end, IEnumerable<int> holidays) {
            var key = GetKey(start, end);
            if ( bussinessDayCountMap.ContainsKey(key) ) {
                return bussinessDayCountMap[key];
            }

            DateTime current = start;
            DateTime inclusiveEnd = end;

            var bussinessDayCount = 0;
            while ( current <= inclusiveEnd ) {
                if (
                    current.DayOfWeek != DayOfWeek.Saturday
                    && current.DayOfWeek != DayOfWeek.Sunday
                    && !holidays.Contains(current.Day)
                ) {
                    bussinessDayCount++;
                }

                current = current.AddDays(1);
            }

            bussinessDayCountMap.Add(key, bussinessDayCount);
            return bussinessDayCountMap[key];
        }

        private static string GetKey(DateTime start, DateTime end) {
            return $"{GetFormatedDate(start)} - {GetFormatedDate(end)}";
        }

        private static string GetFormatedDate(DateTime date) {
            return date.ToString("yyyy-MM-dd");
        }
    }
}