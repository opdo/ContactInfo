   M S B A M L   `   `   `  ���� G  CCONTACT_INFO, Version=1.2.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:CONTACT_INFO.Model.ConverterCONTACT_INFO.Model.Converter  E'clr-namespace:CONTACT_INFO.Windows.PageCONTACT_INFO.Windows.Page  -    'CONTACT_INFO.Windows.Page.PageWarehouse   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     8c*clr-namespace:CONTACT_INFO.Model.Converter     9local'clr-namespace:CONTACT_INFO.Windows.Page     V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     WarehouseVM  � DataContext#  [   � 	Resources 4   .CONTACT_INFO.Model.Converter.DataNullConverter  NullConverter   DataGridColumnSeparatorStyle �� 
TargetType*  $System.Windows.Controls.DataGridCell �� Property �� Value��%k   &	       &	        �� "  .���  � �� �� .R��� $��Stretch=�$��Stretch=�
�������.��]� .+�� $	��Top=�$��
20 20 20 0q� � ColumnDefinitions �� $��* ��� $��* �.+��� $��5q�.+�(  "System.Windows.Controls.DatePicker  )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Xem từ ngày�� ( $MaterialDesignFloatingHintDatePicker#��[    SelectedDate �� *
FromDate+�� $��1��$��5q�.+�  $ Xem đến ngày��#��[  �� *ToDate+]� $��Bottom=�$��	0 5 15 15q�.+�� $
��Left=�$��15 0 0 0q� �� $
��auto ���  � RowDefinitions �� �� �� ���� " �.���  � y�$ Visible=�	 �� Triggers	 �� 
 �� Binding
 �� *SelectedProduct+ �� 	Converter# [  �� Value true.a���  � y�$ 	Collapsed=�.+��� $��10 0 0 0q�$ ��Số lượng nhập vào:��$��16(�$��0��$��0���� $��10 0 0 0q�$��16(�$��1��$��0������ *
TotalCount+�� $��10 0 0 0q�$��Số lượng xuất:��$��16(�$��0��$��1���� $��10 0 0 0q�$��16(�$��1��$��1������ *TotalCountOutput+�� $��10 0 0 0q�$��Số lượng tồn:��$��16(�$��0��$��2���� $��10 0 0 0q�$��16(�$��1��$��2������ *TotalCountRemain+�� $��Right=� �� Orientation$ 
Horizontal=�.+��� -    ��Name$ 	btnExport��$��0 0 10 0q� * &MaterialDesignFloatingActionDarkButton#��[  � ToolTip$ Xuất file Excel������ *ExportExcel+.��'  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ Export=�$��24��$��24��� ���� " �.���  � y�$ Visible=�	 �� 
 �� *SelectedWarehouse+# [  true.a���  � y�$ 	Collapsed=��� 
 �� *SelectedWarehouse.Id+ 0.a���  � y�$ 	Collapsed=�.+��� $��0 0 10 0q� , (MaterialDesignFloatingActionAccentButton#��[ $ Xóa phiếu nhập������  �� ElementName$ listWarehouse3�� �� Path$ SelectedItems����� *DeleteWarehouse+.��  $ DeleteForever=�$��24��$��24��� ���� " �.���  � y�$ Visible=�	 �� 
 �� *SelectedWarehouse+# [  true.a���  � y�$ 	Collapsed=��� 
 �� *SelectedWarehouse.Id+ 0.a���  � y�$ 	Collapsed=�.+��� -   $��btnAdd3��$��0 0 10 0q�#��[ $# Xem thông tin phiếu nhập������ *InfoWarehouse+.��  $	 Eye=�$��24��$��24���� -   $��btnAdd2��#��[ $  Thêm phiếu nhập mới������ *NewWarehouse+.��  $ CreateNewFolder=�$��24��$��24��� $��10q� �� $��* ��� $��3 ��� $��* � �� $
��Auto ��� $��* �.+�&   System.Windows.Controls.DataGrid  -   $��listWarehouse2��$��16(�   SelectionMode$ Single=�   CanUserDeleteRows .  "   VerticalScrollBarVisibility$
 Auto=�$��1��
������ $��5 8 5 0q�   CanUserSortColumns .    CanUserAddRows .     AutoGenerateColumns .  -  'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 13 8 8 8q�   ColumnHeaderPadding$ 8q� �� SelectedItem �� *SelectedProduct+���� *List+ �� Mode$ TwoWay=�   ColumnHeaderStyle �� =	  7System.Windows.Controls.Primitives.DataGridColumnHeader" 	 .���  � ��$ 16(�   Columns 9
  3MaterialDesignThemes.Wpf.MaterialDataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn   Header$	 STT��    Width5  /System.Windows.Controls.DataGridLengthConverter$  60 ! 
  	MaxLength$! 15��"   
IsReadOnly" . 1  +System.Windows.Controls.DataGridBoundColumn#   Binding# �� *STT+4  .System.Windows.Controls.DataGridTemplateColumn  $   MinWidth$$ 1Q�$  1 %   	CellStyle#% [ 
  $ Tên sản phẩm��$
  1.5* $! 50��" .  0 ,MaterialDesignDataGridTextColumnEditingStyle&   EditingElementStyle#& [ # �� *Name+  $$ 1Q�$  1 #% [ 
  $ Tồn��$  2* " . $	! 255��#& [ # �� *Count+' �� StringFormat$' {0:N0}���� $��1��$��1��$��Stretch=�
�������( �� ShowsPreview( . $��1��  -   $��listWarehouse3��$��14(�$ Single=� .  $
 Auto=�$��1��$��2��
������ $��5 8 5 0q� .  .   .  $ 13 8 8 8q�$ 8q� �� *SelectedWarehouse+���� *SelectedProduct.List+$ TwoWay=� �� " 	 .���  � ��$ 16(� 
  $	 STT��$  60 $! 15��" . # �� *STT+  $$ 1Q�$  1 #% [ 
  $ Ngày nhập��$
  1.5* " . $! 50��#& [ # �� *	DateInput+$' 
dd/MM/yyyy��  $$ 1Q�$  1 #% [ 
  $ Số lượng nhập��$  2* $	! 255��" . #& [ # �� *Count+$' {0:N0}��  $$ 1Q�$  1 #% [ 
  $ Ghi chú��$  2* $	! 255��" . #& [ # �� *Note+