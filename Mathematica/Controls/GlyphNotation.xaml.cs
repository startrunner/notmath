﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mathematica.Controls
{
	/// <summary>
	/// Interaction logic for GlyphNotation.xaml
	/// </summary>
	public partial class GlyphNotation : NotationBase
	{
		protected override double FontSizeCoefficient { get; } = 1;

		public GlyphNotation()
		{
			InitializeComponent();
		}
	}
}