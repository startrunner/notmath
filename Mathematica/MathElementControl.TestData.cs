namespace Mathematica
{
    public partial class MathElementControl
    {
        public class TestData
        {
            public MathElement Value { get; set; }

            public TestData()
            {
                Value = new MathElement
                {
                    Main = "M",
                    Sup = "5",
                    Sub = "M"
                };
            }
        }
    }
}