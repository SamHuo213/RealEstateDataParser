using System.IO;

namespace RealEstateDataParser.Services {

    public static class FileService {

        public static string ReadAllText(string filePath) {
            return File.ReadAllText(filePath);
        }
    }
}