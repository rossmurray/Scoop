using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public interface IConfigWatcher<TConfig> : IDisposable, IConfigProvider<TConfig>
	{
		event EventHandler ConfigChanged;
		void Start();
		void Stop();
	}
}
