   M S B A M L   `   `   `  ���� G  CCONTACT_INFO, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:CONTACT_INFO.WindowsCONTACT_INFO.Windows  E'clr-namespace:CONTACT_INFO.Windows.PageCONTACT_INFO.Windows.Page  K*clr-namespace:CONTACT_INFO.Model.ConverterCONTACT_INFO.Model.Converter       CONTACT_INFO.Windows.wMain   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:CONTACT_INFO.Windows     8page'clr-namespace:CONTACT_INFO.Windows.Page     8c*clr-namespace:CONTACT_INFO.Model.Converter     V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes     	MainModel  � DataContext#  [   � Title$ Contact Info��$	��720��$
��1350��  MaterialDesignBody#���  $��Regular"�$��16(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�
������   MaterialDesignFont#���   � Icon$&  /CONTACT_INFO;component/icon.ico�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ CanResizeWithGrip=� � WindowStyle$
 None=� � AllowsTransparency . 	 � 	Resources	 <   6CONTACT_INFO.Model.Converter.BoolToVisibilityConverter  VisibilityConverter   DataGridColumnSeparatorStyle
 �� 
TargetType*  $System.Windows.Controls.DataGridCell �� Property �� Value��%k   &	       &	        �� "
  .���  � �� �� .R��� $��Stretch=�$��Stretch=�
�������.���� $��3q� �� CornerRadius$ 10��
�����������  �� ElementName$ titleBar�� �� Path$ 
Background�._�)  #MaterialDesignThemes.Wpf.DialogHost     IsOpen �� *IsOpenDiaglog+   DialogContent �� $��20q�.+��� ���� "
 ��.���  � y�$ Visible=� �� Triggers ��  �� Binding �� *IsExcelExportDialog+ �� Value True.a���  � y�$ 	Collapsed=�.+��� $C��=Nhập tên cổng COM và ấn đồng ý để tiếp tục���  � ColumnDefinitions �� $
��Auto ��� $��* �.+�'  !MaterialDesignThemes.Wpf.PackIcon  $��0���  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ 
PlugSocket=�$��Center=�$��	4 10 10 0q�$��14(����� $ 
txtCOMName��$ BorderBrush��� -    ��Name$ 
txtCOMName��$��0 10 0 0q�$��1��)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tên cổng COM�� % !MaterialDesignFloatingHintTextBox#��[ ���� *InputCOMName+�� $��0 10 0 0q�  MaterialDesignFlatButton#��[  �� 	IsDefault . ���� *CloseDialog+.��
ĐỒNG Ý� ���� "
 �.���  � y�$ Visible=� ��  �� *IsExcelExportDialog+	 False.a���  � y�$ 	Collapsed=�.+��  % !MaterialDesignCircularProgressBar#��[  � Value$ 35Q� � IsIndeterminate . .��]� ��. .+�(  "MaterialDesignThemes.Wpf.ColorZone  -   $��titleBar��$	��Top=�   CornerRadius$ 8 8 0 0��   Mode$ PrimaryDark=�$��5q�.��]� .+��� -   $��Right=�
������� � Cursor$
 Hand�� J� RenderTransformOrigin$ 0.5,0.5&�$��25��$��26��  �� TextAlignment$  Center=�z���   $ Close=�$��24��$��24��! � Triggers! :� " :� RoutedEvent$" Mouse.MouseLeave��[��� .���� .��W� # �� TargetProperty$# RenderTransform.ScaleX�	$ W� To$$ 1X�% g� Duration$% 0:0:0.3D�W� $# RenderTransform.ScaleY�$$ 1X�$% 0:0:0.3D�:� $" Mouse.MouseEnter��[��� .���� .��W� $# RenderTransform.ScaleX�$	$ 1.3X�$% 0:0:0.3D�W� $# RenderTransform.ScaleY�$	$ 1.3X�$% 0:0:0.3D��� -   $��Right=�
�������$
 Hand��$ 0.5,0.5&�$��25��$��26��$  Center=�z���   $ WindowMinimize=�$��24��$��24��! :� $" Mouse.MouseLeave��[��� .���� .��W� $# RenderTransform.ScaleX�$$ 1X�$% 0:0:0.3D�W� $# RenderTransform.ScaleY�$$ 1X�$% 0:0:0.3D�:� $" Mouse.MouseEnter��[��� .���� .��W� $# RenderTransform.ScaleX�$	$ 1.3X�$% 0:0:0.3D�W� $# RenderTransform.ScaleY�$	$ 1.3X�$% 0:0:0.3D��� & �� Orientation$& 
Horizontal=�.+��� $��18(�$��Center=�$��Light"�$��16 0 0 0q�$"Contact Info v1.2 [C&T Production]  $ 
PrimaryMid=�$	��Top=����� $ titleBar��$ 
Background�.���� $& 
Horizontal=�.+�	� -   $��radio1��$��KHÁCH HÀNG��   MaterialDesignTabRadioButton#��[ $��10q�' d� 	IsChecked$
' TrueY�
�������	� -   $��radio2��$��NHÂN VIÊN��#��[ $��10q�
�������	� -   $��radio3��$��SẢN PHẨM��#��[ $��10q�
�������	� -   $��radio4��$��	KHO HÀNG��#��[ $��10q�
�������	� -	   $��radio5��$��
HÓA ĐƠN��#��[ $��10q�
�������	� -
   $��radio6��$��
HỖ TRỢ��#��[ $��10q�
�������� .+�,	   &CONTACT_INFO.Windows.Page.PageCustomer	  y��� $ radio1��$ 	IsChecked�( �� 	Converter#( [ -
   'CONTACT_INFO.Windows.Page.PageEmployees
  y��� $ radio2��$ 	IsChecked�#( [ +   %CONTACT_INFO.Windows.Page.PageProduct  y��� $ radio3��$ 	IsChecked�#( [ -   'CONTACT_INFO.Windows.Page.PageWarehouse  y��� $ radio4��$ 	IsChecked�#( [ (   "CONTACT_INFO.Windows.Page.PageBill  y��� $ radio5��$ 	IsChecked�#( [ (   "CONTACT_INFO.Windows.Page.PageHelp  y��� $ radio6��$ 	IsChecked�#( [ 