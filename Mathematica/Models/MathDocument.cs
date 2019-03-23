using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematica.Models
{
    public class MathDocument
    {
        public string TextContent { get; set; }
        public MathElementCollection MathElements { get; set; }
    }
}
