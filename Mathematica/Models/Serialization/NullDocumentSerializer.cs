using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Mathematica.Models.Serialization
{
    public class NullDocumentSerializer : IDocumentSerializer
    {
        public FlowDocument Deserialize(MathDocument mathDocument)
        {
            return new FlowDocument();
        }

        public MathDocument Serialize(FlowDocument flowDocument)
        {
            return new MathDocument();
        }
    }
}
