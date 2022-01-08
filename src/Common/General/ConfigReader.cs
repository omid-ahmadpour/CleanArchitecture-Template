namespace CleanTemplate.Common.General
{
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ConfigReader
    {
        public static T Get<T>(this IConfiguration configuration, string key)
        {
            try
            {
                var Options = new AppOptions();
                configuration.GetSection(nameof(Options)).Bind(Options);

                var propsInfo = Options.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var propInfo = propsInfo.FirstOrDefault(c => c.Name.ToLower() == key.ToLower());
                if (propInfo is null) throw new KeyNotFoundException($"config property '{key}' not found.");

                var value = propInfo.GetValue(Options);
                return (T)value;
            }
            catch (System.Exception)
            {
                return default;
            }
        }
    }
}
