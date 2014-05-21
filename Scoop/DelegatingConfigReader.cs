using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class DelegatingConfigReader : IConfigReader
	{
		private Func<string> reader;

		public DelegatingConfigReader(Func<string> reader)
		{
			this.reader = reader;
		}

		public string ReadConfig()
		{
			return this.reader();
		}
	}
}
