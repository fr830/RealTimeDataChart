   M S B A M L   `   `   `  ����   RealTimeWeighChart#    RealTimeWeighChart.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     d `DevExpress.Xpf.Docking.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a@dxdo5http://schemas.devexpress.com/winfx/2008/xaml/docking  a ]DevExpress.Xpf.Grid.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49af bDevExpress.Xpf.Grid.v14.1.Core, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxg2http://schemas.devexpress.com/winfx/2008/xaml/grid   Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0    � Title$  
MainWindow��$	��600��$
��1000��.��� .+�.  (DevExpress.Xpf.Docking.DockLayoutManager     
LayoutRoot. (  "DevExpress.Xpf.Docking.LayoutGroup     Items.      Orientation$ Vertical=�$	��200��. (  "DevExpress.Xpf.Docking.LayoutPanel  +  %DevExpress.Xpf.Docking.BaseLayoutItem   Caption$ Hercules��   
AllowClose .  (  "DevExpress.Xpf.Docking.ContentItem   Content. �� -    ��Name$ 	textboxSP��$�� �� �� AcceptsReturn . 	 �� TextWrapping$
	 Wrap=�"
 �� VerticalScrollBarVisibility$

 Auto=�  $ NotPad�� .  . �� -    ��Name$ 	listboxSP��$��Center=�$	��300��$
��Auto=�  $  ��$��50��   Closed . . �� -    ��Name$
 ugur��$��Bunu Kullanma!��  $ 
Data Chart�� .  . �  � RowDefinitions �� $	��30* ��� $
��539* � � ColumnDefinitions �� $	��94* ��� $��5* �.+��� -   $��comSelect_LB��$��
COM Select��$
��Left=�$��80���� -   $��comSelect_CmBx��$ �� IsSynchronizedWithCurrentItem$
 TrueY�$��0��$��85,0,0,0q�$
��Left=�$��79������  �� Path$ Property��� -   $��openPort_BTN��$
��OPEN��$��	185,0,0,0q�$
��Left=�$��60���� -   $��closePort_BTN��$��CLOSE��$��	260,0,0,0q�$
��Left=�$��60���� -   $��save_BTN��$
��SAVE��$��	340,2,0,0q�$
��Left=�$��60�� � Cursor$
 Hand���� -	   $
��Harf��$�� ��$��
580,0,10,0q�$��2��8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -
     Name$ plotter��$��0,2,10,0q�$��1��$��2��   HorizontalAxis I  CMicrosoft.Research.DynamicDataDisplay.Charts.HorizontalDateTimeAxis  -   $��dateAxis��3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. 2	  ,Microsoft.Research.DynamicDataDisplay.Header	  $��Georgia)�$��
Data chart��=
  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle
  $��Georgia)�$��Data [V]��?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  $��Georgia)�$
��Time��A  ;Microsoft.Research.DynamicDataDisplay.Charts.HorizontalLine  
���  ��$��2��=  7Microsoft.Research.DynamicDataDisplay.Charts.SimpleLine   Value �� *
MaxVoltage+  
���  ��$��2�� �� *
MinVoltage+