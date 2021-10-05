using System;
using System.Collections.Generic;
using System.Linq;

namespace SerilogDemo.Infrastructure
{
    public static class SerilogProperties
    {
        public static string Url = "URL";
        public static string User = "User";
        public static string IpAddress = "IPAddress";

        public static List<string> AllProperties = new()
        {
            Url,
            User,
            IpAddress,
        };

        // Format should be "User: {User} {NewLine} IP Address: {IP Address}"
        public static string MessageTemplateWithAllProperties => String.Join(" {NewLine} ", AllProperties.Select(p => p + ": {" + p + "}").ToList());
    }
}