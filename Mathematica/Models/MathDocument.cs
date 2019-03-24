using System.Collections.Generic;

namespace Mathematica.Models
{
    public class MathDocument
    {
        public string TextContent { get; set; }
        public List<MathElement> MathElements { get; set; }
    }
}
