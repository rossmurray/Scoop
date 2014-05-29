using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class ConfigWatcher<TConfig> : IConfigWatcher<TConfig>
	{
		public event EventHandler<ConfigChangedEventArgs<TConfig>> ConfigChanged;

		private IConfigProvider<TConfig> provider;
		private IFileWatcher fileWatcher;
		private bool disposed;
		private object startStopLock = new object();

		public ConfigWatcher(IConfigProvider<TConfig> provider, IFileWatcher fileWatcher)
		{
            this.provider = provider;
			this.fileWatcher = fileWatcher;
			this.fileWatcher.FileChanged += OnConfigChanged;
		}

		public void Start()
		{
			lock (this.startStopLock)
			{
				if (this.disposed) throw new ObjectDisposedException(this.GetType().FullName);
				this.fileWatcher.Start();
			}
		}

		public void Stop()
		{
			lock (this.startStopLock)
			{
				if (this.disposed) throw new ObjectDisposedException(this.GetType().FullName);
				this.fileWatcher.Stop();
			}
		}

		public TConfig GetConfig()
		{
			return this.provider.GetConfig();
		}

		private void OnConfigChanged(object sender, EventArgs e)
		{
			NotifySubscribers();
		}

		private void NotifySubscribers()
		{
			var configArgs = new ConfigChangedEventArgs<TConfig>(this.provider.GetConfig());
			var handlers = this.ConfigChanged;
			if (handlers != null)
			{
				handlers(this, configArgs);
			}
		}

		public void Dispose()
		{
			if (this.disposed) return;
			this.disposed = true;
			this.fileWatcher.FileChanged -= OnConfigChanged;
			this.fileWatcher.Dispose();
		}
	}
}
