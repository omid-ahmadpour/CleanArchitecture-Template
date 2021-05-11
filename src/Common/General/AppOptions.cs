using System.Collections.Generic;

namespace Common.General
{
    public class AppOptions
    {
        public string WriteDatabaseConnectionString { get; set; }

        public string ReadDatabaseConnectionString { get; set; }

        public string AuthenticationServerUri { get; set; }

        public string AuthenticationServerApiName { get; set; }

        public string AuthenticationServerSecret { get; set; }

        public bool ActivateSwagger { get; set; }

        public List<string> CorsEnableUris { get; set; }
    }
}
