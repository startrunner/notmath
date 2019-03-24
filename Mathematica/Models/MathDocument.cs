using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mathematica.Models
{
	[JsonObject]
	public class MathDocument
	{
		public string TextContent { get; set; }

		public List<MathElement> MathElements { get; set; }
	}
}
