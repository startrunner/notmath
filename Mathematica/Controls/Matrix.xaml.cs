using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Mathematica.Controls
{
    public partial class Matrix : UserControl
    {
        List<List<MathBox>> boxRows;
        int rowCount = 1;
        int columnCount = 1;

        public Matrix()
        {
            InitializeComponent();
            AttachEvents(topLeft);
            boxRows = new List<List<MathBox>> {
                new List<MathBox> { topLeft }
            };
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

            boxRows[row][column].SetCaretPosition(BoxCaretPosition.Start);
            boxRows[row][column].Focus();
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

            Focus(selectedRow + 1, selectedColumn);
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
                new RowDefinition() {
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
        }

        void AddColumn()
        {
            contentGrid.ColumnDefinitions.Add(
                new ColumnDefinition() {
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
        }
    }
}
