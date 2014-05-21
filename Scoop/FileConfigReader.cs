using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class FileConfigReader : IConfigReader
	{
		private ConfigFileLocation fileLocation;

		public FileConfigReader(ConfigFileLocation fileLocation)
		{
			this.fileLocation = fileLocation;
		}

		public string ReadConfig()
		{
			return File.ReadAllText(this.fileLocation.File.FullName);
		}
	}

	public class ConfigFileLocation
	{
		public FileInfo File { get; set; }
	}
}
