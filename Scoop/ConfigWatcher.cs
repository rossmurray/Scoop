using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class ConfigWatcher<TConfig> : IConfigWatcher<TConfig>
	{
		public event EventHandler ConfigChanged;

		private IConfigProvider<TConfig> provider;

		public ConfigWatcher(IConfigProvider<TConfig> provider)
		{
            this.provider = provider;
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
			return this.provider.GetConfig();
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
