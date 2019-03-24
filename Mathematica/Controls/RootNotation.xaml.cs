namespace Mathematica.Controls
{
    public partial class RootNotation : NotationBase
    {
        protected override MathBox[] AllBoxes { get; }
        protected override MathBox[] AvailableBoxes { get; }

        public MathBox RootBase => boxRootBase;
        public MathBox ContentUnderRoot => boxUnderRoot;

        public RootNotation()
        {
            InitializeComponent();
            AvailableBoxes = AllBoxes = new MathBox[] { boxRootBase, boxUnderRoot };
        }
    }
}
