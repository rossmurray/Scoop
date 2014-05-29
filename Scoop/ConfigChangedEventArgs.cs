using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class ConfigChangedEventArgs<TConfig> : EventArgs
	{
		public readonly TConfig Config;

		public ConfigChangedEventArgs(TConfig config)
		{
			this.Config = config;
		}
	}
}
