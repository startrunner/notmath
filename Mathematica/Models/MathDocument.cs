using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
            = new List<MathElement>();

        public bool IsEmpty()
        {
            return TextContent == null && MathElements.Count == 0;
        }
    }
}
