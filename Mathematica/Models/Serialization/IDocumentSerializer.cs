using System.Windows.Documents;

namespace Mathematica.Models.Serialization
{
    public interface IDocumentSerializer
    {
        FlowDocument Deserialize(MathDocument mathDocument);
        MathDocument Serialize(FlowDocument flowDocument);
    }
}