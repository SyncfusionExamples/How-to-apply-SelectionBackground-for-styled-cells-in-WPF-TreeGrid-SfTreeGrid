# How to apply SelectionBackground for styled cells in WPF TreeGrid (SfTreeGrid)?

In [WPF TreeGrid](https://www.syncfusion.com/wpf-controls/treegrid) (SfTreeGrid),  when the [TreeGridCell](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.TreeGrid.TreeGridCell.html) background is customized, the **SelectionBackground** is not applied by default. Instead, when a row is selected, only the customized TreeGridCell background is retained. This behavior is expected.

However, you can achieve the required behavior by using a [StyleSelector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.styleselector?view=windowsdesktop-10.0). In the selector, define the background for non‑editable cells. Additionally, handle the [SelectionChanged](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.TreeGrid.SfTreeGrid.html#Syncfusion_UI_Xaml_TreeGrid_SfTreeGrid_SelectionChanged) event and call the **UpdateDataRow** method by passing the indices of the removed and added items.

This approach triggers the style selector at runtime during UI selection. Using this logic, you can apply the same selection background color to TreeGridCells when they are selected, and remove the selection background when they are deselected, restoring the original TreeGridCells background color.

**XAML**
```
<syncfusion:SfTreeGrid Name="treeGrid"
                    AllowEditing="True"
                    AutoGenerateColumns="False"
                    ItemsSource="{Binding Employees}"
                    ChildPropertyName="ReportsTo"
                    ParentPropertyName="ID"    
                    CellStyleSelector="{StaticResource styleSelector}"
                    SelfRelationRootValue="-1"  >
    <syncfusion:SfTreeGrid.Columns>
        <syncfusion:TreeGridTextColumn MappingName="FirstName" HeaderText="First Name" />
        <syncfusion:TreeGridTextColumn  MappingName="LastName" 
                                     HeaderText="LastName" 
                                     AllowEditing="False"                                 
                                     >
        </syncfusion:TreeGridTextColumn>
        <syncfusion:TreeGridTextColumn MappingName="ID" HeaderText="ID"/>
        <syncfusion:TreeGridCurrencyColumn  MappingName="Salary" HeaderText="Salary" />
    </syncfusion:SfTreeGrid.Columns>
</syncfusion:SfTreeGrid>
```

**C#**
```
// StyleSelector Class
 public class SelectorClass : StyleSelector
 {
     public override Style SelectStyle(object item, DependencyObject container)
     {
         var cell = container as TreeGridCell;
         var columnBase = cell.ColumnBase;
         // Use reflection to get the DataRow property from ColumnBase
         var dataRowProperty = columnBase.GetType().GetProperty("DataRow", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
         var dataRow = dataRowProperty?.GetValue(columnBase);

         var iseSelectedRow = (dataRow as TreeDataRow).IsSelectedRow;
         var hasEditing = columnBase.TreeGridColumn.AllowEditing;
         if (iseSelectedRow)
         {
             return App.Current.Resources["Selected"] as Style;
         }

         if (!hasEditing)
             return App.Current.Resources["NoEditable"] as Style;

         return base.SelectStyle(item, container);
     }
 }

 // SelectionChanged event
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
```

![Apply SelectionBackground for styled cells](Apply%20SelectionBackground%20for%20styled%20cells.gif)

Take a moment to peruse the [WPF TreeGrid - Conditional styling](https://help.syncfusion.com/wpf/treegrid/conditional-styling) documentation, to learn more about conditional styling with example.