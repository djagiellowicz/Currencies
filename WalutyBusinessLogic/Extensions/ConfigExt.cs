using Microsoft.Extensions.Configuration;
using System;

namespace WalutyBusinessLogic.Extensions
{
    public static class ConfigExt
    {
        private static string trueValue = "true";
        private static string falseValue = "false";

        public static bool GetFlag(this IConfiguration configuration, string key)
        {
            string value = configuration.GetSection("Flags")[key] ?? throw new ArgumentNullException();

            if (trueValue.Equals(value.ToLower())) return true;
            if (falseValue.Equals(value.ToLower())) return true;

            throw new ArgumentOutOfRangeException("Wrong value at: " + key);
        }
    }
}
