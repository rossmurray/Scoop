using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
    public static class Configuration
    {
		public static IConfigProvider<TConfig> Provide<TConfig>(string file)
		{
			return Configuration.Provide<TConfig>(new FileInfo(file));
		}

		public static IConfigProvider<TConfig> Provide<TConfig>(FileInfo file)
		{
			var reader = new FileConfigReader(new ConfigFileLocation { File = file });
			var deserializer = new JsonDeserializer();
			var worker = new ConfigWorker<TConfig>(reader, deserializer);
			return new CachingConfigProvider<TConfig>(worker);
		}

		public static TConfig Read<TConfig>(string file)
		{
			return Configuration.Provide<TConfig>(file).GetConfig();
		}

		public static TConfig Read<TConfig>(FileInfo file)
		{
			return Configuration.Provide<TConfig>(file).GetConfig();
		}

		public static IConfigWatcher<TConfig> Watch<TConfig>(FileInfo file)
		{
			var reader = new FileConfigReader(new ConfigFileLocation { File = file });
			var deserializer = new JsonDeserializer();
			var worker = new ConfigWorker<TConfig>(reader, deserializer);
			var provider = new BasicConfigProvider<TConfig>(worker);
			var fileWatcher = new FileWatcher(file);
			var watcher = new ConfigWatcher<TConfig>(provider, fileWatcher);
			return watcher;
		}

		public static IConfigWatcher<TConfig> Watch<TConfig>(string file)
		{
			return Watch<TConfig>(new FileInfo(file));
		}
    }
}
