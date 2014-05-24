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

        private IConfigWorker<TConfig> worker;

		public ConfigWatcher(IConfigWorker<TConfig> worker)
		{
            this.worker = worker;
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
            var configObject = this.worker.Work();
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
