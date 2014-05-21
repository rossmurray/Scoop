using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class ConfigWatcher<TConfig> : IConfigProvider<TConfig>, IConfigWatcher
	{
		public event EventHandler ConfigChanged;

		private IConfigReader reader;
		private IDeserializer deserializer;

		public ConfigWatcher(IConfigReader reader, IDeserializer deserializer)
		{
			this.reader = reader;
			this.deserializer = deserializer;
		}

		public void Start()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		public TConfig GetConfig()
		{
			var configText = this.reader.ReadConfig();
			var configObject = this.deserializer.Deserialize<TConfig>(configText);
			return configObject;
		}

		public void Dispose()
		{
			
		}

		private void OnConfigChanged()
		{
			var handlers = this.ConfigChanged;
			if(handlers != null)
			{
				handlers(this, EventArgs.Empty);
			}
		}
	}
}
