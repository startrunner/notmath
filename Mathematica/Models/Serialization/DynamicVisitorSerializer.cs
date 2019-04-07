using System;
using System.Windows.Documents;

namespace Mathematica.Models.Serialization
{
    public class DynamicVisitorSerializer : IDocumentSerializer
    {
        public FlowDocument Deserialize(MathDocument document)
        {
            throw new NotImplementedException();
        }

        public MathDocument Serialize(FlowDocument document)
        {
         throw new NotImplementedException();   
        }
    }
}