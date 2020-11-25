using Microsoft.Extensions.Configuration;
using System;

namespace WalutyMVCWebApp.Extensions.Configuration
{
    public static class ConfigExt
    {
        private static string _trueValue = "true";
        private static string _falseValue = "false";

        public static bool GetFlag(this IConfiguration configuration, string key)
        {
            string value = configuration.GetSection("Flags")[key] ?? throw new ArgumentNullException();

            if (_trueValue.Equals(value.ToLower())) return true;
            if (_falseValue.Equals(value.ToLower())) return true;

            throw new ArgumentOutOfRangeException("Wrong value at: " + key);
        }
    }
}
