using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class CachingConfigProvider<TConfig> : IConfigProvider<TConfig>
	{
		private IConfigWorker<TConfig> worker;
		private TConfig config;

		public CachingConfigProvider(FileInfo file)
		{
			var configReader = new FileConfigReader(new ConfigFileLocation { File = file });
			var deserializer = new JsonDeserializer();
			this.worker = new ConfigWorker<TConfig>(configReader, deserializer);
		}

		public CachingConfigProvider(IConfigWorker<TConfig> worker)
		{
			this.worker = worker;
		}

		public CachingConfigProvider(IConfigReader reader, IDeserializer deserializer)
		{
			this.worker = new ConfigWorker<TConfig>(reader, deserializer);
		}

		public TConfig GetConfig()
		{
			if (this.config != null) return this.config;
			var config = this.worker.Work();
			this.config = config;
			return config;
		}
	}
}
