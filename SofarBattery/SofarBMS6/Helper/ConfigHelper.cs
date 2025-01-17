using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofarBMS.Helper
{
    public class ConfigHelper
    {
        static IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("config.json", false, true);
        public static string ReadConfig(string path)
        {

            IConfigurationRoot root = builder.Build();


            //读取配置
            var a = root[path];
            //var a = config["JWTSettings:Secret"];
            return a;
        }

        public static string SaveConfig(string path, string value)
        {
            IConfigurationRoot root = builder.Build();
            root[path] = value;
            Console.WriteLine(root.GetSection(path).Value);
            //var a = config["JWTSettings:Secret"];
            return value;
        }

    }
}
