using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;



namespace SofarBMS.Helper
{
    public class ConfigHelper
    {
        private static ConfigHelper config;
        private IConfigurationBuilder builder;
        private IConfigurationRoot root;
        private ConfigHelper()
        {

        }


        public static ConfigHelper GetHeper()
        {
            if (config == null)
            {
                config = new ConfigHelper();
            }

            config.builder = new ConfigurationBuilder();
            config.root = config.builder.Add<WritableJsonConfigurationSource>(
                (Action<WritableJsonConfigurationSource>)(s =>
                {
                    s.FileProvider = null;
                    s.Path = "config.json";
                    s.Optional = false;
                    s.ReloadOnChange = true;
                    s.ResolveFileProvider();
                })).Build();

            return config;
        }

        public string ReadConfig(string path)
        {
            return config.root[path];
        }

        public void SaveConfig(string path, string value)
        {
            config.root.GetSection(path).Value = value;
        }


    }
    public class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.EnsureDefaults(builder);
            return (IConfigurationProvider)new WritableJsonConfigurationProvider(this);
        }
    }
    public class WritableJsonConfigurationProvider : JsonConfigurationProvider
    {
        public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
        {
        }

        public override void Set(string key, string value)
        {


            string[] getParent = key.Split(':');
            string parent = getParent[0];


            string[] substantialKey = key.Split(":");

            base.Set(key, value);

            var fileFullPath = base.Source.FileProvider.GetFileInfo(base.Source.Path).PhysicalPath;
            string json = File.ReadAllText(fileFullPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parent][substantialKey[1]] = value;

            string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fileFullPath, output);
        }
    }
}
