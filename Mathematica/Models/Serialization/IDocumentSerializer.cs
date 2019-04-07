using System.Windows.Documents;

namespace Mathematica.Models.Serialization
{
    public interface IDocumentSerializer
    {
        FlowDocument Deserialize(MathDocument document);
        MathDocument Serialize(FlowDocument document);
    }
}