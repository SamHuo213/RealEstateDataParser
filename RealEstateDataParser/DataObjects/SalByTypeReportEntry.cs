namespace RealEstateDataParser.DataObjects {
    public class SalByTypeReportEntry {
        public string PropertyType { get; set; }

        public int Sales { get; set; }

        public double ProjectedSales { get; set; }

        public int Inventory { get; set; }

        public double Sal { get; set; }

        public double SalPercentage { get; set; }
    }
}
