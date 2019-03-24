namespace Mathematica.Controls
{
    public partial class RootNotation : NotationBase
    {
        protected override MathBox[] AvailableBoxes { get; }

        public MathBox RootBase => boxRootBase;
        public MathBox ContentUnderRoot => boxUnderRoot;

        public RootNotation()
        {
            InitializeComponent();
            AvailableBoxes = new MathBox[] { boxRootBase, boxUnderRoot };
        }
    }
}
