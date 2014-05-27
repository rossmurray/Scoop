using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class BasicConfigProvider<TConfig> : IConfigProvider<TConfig>
	{
		private IConfigWorker<TConfig> worker;

		public BasicConfigProvider(FileInfo file)
		{
			var configReader = new FileConfigReader(new ConfigFileLocation { File = file });
			var deserializer = new JsonDeserializer();
			this.worker = new ConfigWorker<TConfig>(configReader, deserializer);
		}

		public BasicConfigProvider(IConfigWorker<TConfig> worker)
		{
			this.worker = worker;
		}

		public BasicConfigProvider(IConfigReader reader, IDeserializer deserializer)
		{
			this.worker = new ConfigWorker<TConfig>(reader, deserializer);
		}

		public TConfig GetConfig()
		{
			return this.worker.Work();
		}
	}
}
