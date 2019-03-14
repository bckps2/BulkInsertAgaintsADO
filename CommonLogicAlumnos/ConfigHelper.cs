using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLogicAlumnos
{
    public class ConfigHelper
    {
        public static IConfiguration Config
        {
            get
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                return builder.Build();
            }

        }

    }
}
