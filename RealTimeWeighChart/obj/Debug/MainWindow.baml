   M S B A M L   `   `   `  ����  RealTimeWeighChart#    RealTimeWeighChart.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   d `DevExpress.Xpf.Docking.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a@dxdo5http://schemas.devexpress.com/winfx/2008/xaml/docking  5   	   a ]DevExpress.Xpf.Grid.v14.1, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49af bDevExpress.Xpf.Grid.v14.1.Core, Version=14.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxg2http://schemas.devexpress.com/winfx/2008/xaml/grid   6L   Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  5   	     � Title$  
MainWindow��5   	   $	��600��6   $
��1000��6)   .��5      � .+�5
   
   .  (DevExpress.Xpf.Docking.DockLayoutManager     
LayoutRoot. 5      (  "DevExpress.Xpf.Docking.LayoutGroup     Items. 5           Orientation$ Vertical=�6#   $	��200��6:   . 5      (  "DevExpress.Xpf.Docking.LayoutPanel  +  %DevExpress.Xpf.Docking.BaseLayoutItem   Caption$ Hercules��6'      
AllowClose .  (  "DevExpress.Xpf.Docking.ContentItem   Content. 5      �� -   5         ��Name$ 	textboxSP��5   "   $�� ��65    �� AcceptsReturn . 	 �� TextWrapping$
	 Wrap=�6R   "
 �� VerticalScrollBarVisibility$

 Auto=�6f   6�   5        5      $ NotPad��6'    .  . 5      �� -   5         ��Name$ 	listboxSP��5   &   $��Center=�6:   $
��Auto=�6V   6�   5        5      $  ��6'   $��50��63      Closed . . 5      �� -   5         ��Name$
 ugur��5   !   $��Bunu Kullanma!��6/   6J   5      5        5      $ 
Data Chart��6#    .  . 5      �  � RowDefinitions 5      �� 5      $	��30* �6(   �� 5      $
��539* �6(    � ColumnDefinitions 5!      �� 5"      $	��94* �6+   �� 5#      $��5* �6+   .+�5%      �� -   5        $��comSelect_LB��5%      $��
COM Select��6S   $
��Left=�6h   $��80��6�   �� 5'      -   5        $��comSelect_CmBx��5'      $ �� IsSynchronizedWithCurrentItem$
 TrueY�6[   $��0��6�   $��85,0,0,0q�6�   $
��Left=�6�   $��79��6�   ��65   ��  �� Path$ Property�6�   �� 5(      -   5        $��openPort_BTN��5(      $
��OPEN��61   $��	185,0,0,0q�6W   $
��Left=�6j   $��60��6�   6�   �� 5)      -   5        $��closePort_BTN��5)      $��CLOSE��62   $��	260,0,0,0q�6Z   $
��Left=�6m   $��60��6�   6�   �� 5*      -   5        $��save_BTN��5*   !   $
��SAVE��61   $��	340,2,0,0q�6S   $
��Left=�6f   $��60��6�    � Cursor$
 Hand��6�   6�   �� 5+      -	   5        $
��Harf��5+       $�� ��6,   $��
580,0,10,0q�67   $��2��6K   8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  5,      -
   5          Name$ plotter��5,   *   $��0,2,10,0q�6;   $��1��6M   $��2��6Z      HorizontalAxis 5-      I  CMicrosoft.Research.DynamicDataDisplay.Charts.HorizontalDateTimeAxis  5.   "   -   5        $��dateAxis��5.   <   3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. 50      2	  ,Microsoft.Research.DynamicDataDisplay.Header	  $��Georgia)�6(   $��
Data chart��6=   =
  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle
  51      $��Georgia)�63   $��Data [V]��6H   ?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  52      $��Georgia)�65   $
��Time��6J   A  ;Microsoft.Research.DynamicDataDisplay.Charts.HorizontalLine  53      
���  ��$��2��6Z   =  7Microsoft.Research.DynamicDataDisplay.Charts.SimpleLine   Value 60   �� *
MaxVoltage+6Z     54      
���  ��$��2��6Z    60   �� *
MinVoltage+6Z   55      56      57      58      59      5;      5<      