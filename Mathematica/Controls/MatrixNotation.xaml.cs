using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mathematica.Contracts;
using TinyMVVM.Commands;

namespace Mathematica.Controls
{
    public partial class MatrixNotation : NotationBase
    {
        List<List<MathBox>> boxRows;
        int rowCount = 1;
        int columnCount = 1;

        protected override MathBox[] AllBoxes => boxRows.SelectMany(x => x).ToArray();

        public MatrixNotation()
        {
            InitializeComponent();
            AttachEvents(topLeft);
            boxRows = new List<List<MathBox>> {
                new List<MathBox> { topLeft }
            };
        }

        public MathBox[][] Elements
        {
            get => boxRows.Select(x => x.ToArray()).ToArray();
            set
            {
                boxRows = value.Select(x => x.ToList()).ToList();
                rowCount = value.Length;
                columnCount = value.First().Length;
                contentGrid.Children.Clear();
                
                for (int row = 0; row < value.Length; row++)
                {
                    for (int column = 0; column < value[row].Length; column++)
                    {
                        value[row][column].SetValue(Grid.RowProperty, row);
                        value[row][column].SetValue(Grid.ColumnProperty, column);
                        contentGrid.Children.Add(value[row][column]);
                    }
                }

                for (int i = 0; i < rowCount; i++)contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto});
                for(int i=0;i<columnCount;i++)contentGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = GridLength.Auto});
            }
        }

        protected override bool FocusDirectionProtected(Direction direction)
        {
            if (!TryGetSelected(out int selectedRow, out int selectedColumn)) return false;

            if (direction == Direction.Up)
            {
                if (selectedRow == 0) return false;
                Focus(selectedRow - 1, selectedColumn);
                return true;
            }
            else if (direction == Direction.Down)
            {
                if (selectedRow == rowCount - 1) return false;
                Focus(selectedRow + 1, selectedColumn);
                return true;
            }
            else if (direction == Direction.Left)
            {
                if (selectedColumn == 0)
                {
                    return false;
                }
                Focus(selectedRow, selectedColumn - 1);
                return true;
            }
            else if (direction == Direction.Right)
            {
                if (selectedColumn == columnCount - 1)
                {
                    return false;
                }
                Focus(selectedRow, selectedColumn + 1);
                return true;
            }
            return false;
        }

        protected override bool FocusFirstProtected()
        {
            Focus(rowCount / 2, 0);
            return true;
        }

        protected override bool FocusLastProtected()
        {
            Focus(rowCount / 2, columnCount - 1);
            return true;
        }

        bool TryGetSelected(out int row, out int column)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (boxRows[i][j].IsFocused)
                    {
                        row = i;
                        column = j;
                        return true;
                    }
                }
            }

            row = column = 0;
            return false;
        }

        public void Focus(int row, int column)
        {
            if (row >= rowCount || row < 0) throw new IndexOutOfRangeException();
            if (column >= columnCount || column < 0) throw new IndexOutOfRangeException();

            FocusBox(boxRows[row][column], BoxCaretPosition.Start);
        }

        private void AttachEvents(MathBox box)
        {
            box.NextMatrixRowRequested += HandleNextRowRequested;
            box.NextMatrixColumnRequested += HandleNextColumnRequested;
        }

        void HandleNextRowRequested(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (!TryGetSelected(out int selectedRow, out int selectedColumn)) return;
            while (selectedRow + 1 >= rowCount) AddRow();

            Focus(selectedRow + 1, 0);
        }

        void HandleNextColumnRequested(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (!TryGetSelected(out int selectedRow, out int selectedColumn)) return;
            while (selectedColumn + 1 >= columnCount) AddColumn();

            Focus(selectedRow, selectedColumn + 1);
        }

        void AddRow()
        {
            contentGrid.RowDefinitions.Add(
                new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Auto)
                }
            );

            var newRow = new List<MathBox>();
            for (int i = 0; i < columnCount; i++)
            {
                var box = new MathBox();
                AttachEvents(box);
                box.SetValue(Grid.RowProperty, rowCount);
                box.SetValue(Grid.ColumnProperty, i);
                newRow.Add(box);
                contentGrid.Children.Add(box);
            }
            boxRows.Add(newRow);
            rowCount++;

            ReattachBoxEvents();
        }

        void AddColumn()
        {
            contentGrid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Auto)
                }
            );

            for (int i = 0; i < rowCount; i++)
            {
                var box = new MathBox();
                AttachEvents(box);
                box.SetValue(Grid.RowProperty, i);
                box.SetValue(Grid.ColumnProperty, columnCount);
                boxRows[i].Add(box);
                contentGrid.Children.Add(box);
            }
            columnCount++;

            ReattachBoxEvents();
        }
    }
}
