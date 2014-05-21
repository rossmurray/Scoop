using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scoop
{
	public class JsonDeserializer : IDeserializer
	{
		public T Deserialize<T>(string s)
		{
			return JsonConvert.DeserializeObject<T>(s);
		}
	}
}
