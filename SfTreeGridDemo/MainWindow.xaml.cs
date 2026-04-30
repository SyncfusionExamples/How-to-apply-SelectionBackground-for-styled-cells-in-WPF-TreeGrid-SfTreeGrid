using Syncfusion.UI.Xaml.TreeGrid;
using Syncfusion.UI.Xaml.TreeGrid.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SfTreeGridDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            treeGrid.SelectionChanged += TreeGrid_SelectionChanged;
        }

        private void TreeGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            var treeGrid = sender as SfTreeGrid;
            foreach (var item in e.RemovedItems)
            {
                treeGrid.UpdateDataRow((item as TreeGridRowInfo).RowIndex);
            }
            foreach (var item in e.AddedItems)
            {
                treeGrid.UpdateDataRow((item as TreeGridRowInfo).RowIndex);
            }
        }
    }
}
