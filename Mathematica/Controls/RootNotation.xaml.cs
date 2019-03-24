namespace Mathematica.Controls
{
    public partial class RootNotation : NotationBase
    {
        protected override MathBox[] AvailableBoxes { get; }

        

        public RootNotation()
        {
            InitializeComponent();
            AvailableBoxes = new MathBox[] { boxBase, boxContent };
        }
    }
}
