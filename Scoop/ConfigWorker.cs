using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class ConfigWorker<TConfig> : IConfigWorker<TConfig>
	{
		private IConfigReader configReader;
		private IDeserializer deserializer;

		public ConfigWorker(IConfigReader configReader, IDeserializer deserializer)
		{
			this.configReader = configReader;
			this.deserializer = deserializer;
		}

		public TConfig Work()
		{
			var configText = this.configReader.ReadConfig();
			var configObject = this.deserializer.Deserialize<TConfig>(configText);
			return configObject;
		}
	}
}
