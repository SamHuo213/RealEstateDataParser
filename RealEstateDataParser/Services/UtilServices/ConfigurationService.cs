using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RealEstateDataParser.Services {
    public static class ConfigurationService {
        private static Dictionary<string, string> configuration;

        public static string Mode { get; set; } = "production";

        public static Dictionary<string, string> Configuration {
            get {
                if (configuration == null) {
                    configuration = GetConfigurations();
                }

                return configuration;
            }
        }

        private static Dictionary<string, string> GetConfigurations() {
            var settingFilePath = "appsettings.production.json";
            if (Mode == "debug") {
                settingFilePath = Environment.GetEnvironmentVariable("debugSettingFilePath");
            }

            return JsonConvert
                   .DeserializeObject<Dictionary<string, string>>(FileService.ReadAllText(settingFilePath));
        }
    }
}
