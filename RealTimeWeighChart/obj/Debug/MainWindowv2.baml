   M S B A M L   `   `   `  ����  RealTimeWeighChart%    RealTimeWeighChart.MainWindowv2   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  5   	   -   5          � Title$  MainWindowv2��5   	   $	��600��6   $	��800��6+    � WindowState$ 	Maximized=�67    � WindowStyle$
 None=�6O    � AllowsTransparency . .��5      � 
��� ��� � RowDefinitions 5   
   �� 5	      $��50 �6   6*   �� 5
      6   .+�5   
   �� $��Real Time Weight Chart��6   $��20(�61   $��Italic$�6?   6T   �� 5   
   -   5        $��Right=�5   &   $��X��6B   $
��Bold"�6N   $��15(�6`   $��30��6n   $��30��6y   6�   �� 5   
   -   5        $��Right=�5   &   $	��[ ]��6B   $��0,0,30,0q�6P   $
��Bold"�6b   $��15(�6t   $��30��6�   $��30��6�   6�   �� 5   
   -   5        $��Right=�5   &   $��_��6B   $��0,0,60,0q�6N   $
��Bold"�6`   $��15(�6r   $��30��6�   $��30��6�   6�   � 5   
   $��1��6   
������� � ColumnDefinitions 5   
   �� 5      $��1* �6   6,   �� 5      $��10 �6   6,   �� 5      $��3* �6   6,   .+�5   
   �  5      �� 5      6"   �� 5      $��10 �6    6.   �� 5      6"   .+�5      �� $%��Serial Port'tan Gelen Değerler��6   
���**��$��10q�6S   .��5      �� -   5         ��Name$ 	textboxSP��5      $�� ��6-    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6J   "	 �� VerticalScrollBarVisibility$
	 Auto=�6^   6�   5       �� 5!      $��NotePad��6   $��2��6,   
�������$��10q�6N   .��5"      �� -   5        
 ��Name$
 	listboxSP��5"      $
��Auto=�6/   6a   5$      5%      � 5&   
   $��2��6    5'      �� 5(      $��30 �6    6.   �� 5)      $��5 �6    6-   �� 5*      6"   .+�5,      �  5-      �� 5.      6)   �� 5/      6)   �� 50      6)   �� 51      6)   �� 52      6)   .+�54      �� -   5        $��comSelect_CmBx��54       �� Text$ Com Port��61   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6g   ��6A   ��  �� Path$ Property�6g   �� 55      -   5        $��openPort_BTN��55      $
��OPEN��6-   $��2��6T   6g   �� 56      -	   5        $��closePort_BTN��56      $��CLOSE��6.   $��3��6V   6j   �� 57      -
   5        $��save_BTN��57      $
��SAVE��6)   $��4��6K    � Cursor$
 Hand��6[   6l   5:      � 5;      $��2��6   .+�5<      8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -   5          Name$ plotter��5<   &   $��10q�67      HorizontalAxis 5=      I  CMicrosoft.Research.DynamicDataDisplay.Charts.HorizontalDateTimeAxis  5>      -   5        $��dateAxis��5>   4   3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. 5@      2  ,Microsoft.Research.DynamicDataDisplay.Header  $��Georgia)�6    $��
Data chart��65   =  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  5A      $��Georgia)�6+   $��Data [V]��6@   ?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  5B      $��Georgia)�6-   $
��Time��6B   A  ;Microsoft.Research.DynamicDataDisplay.Charts.HorizontalLine  5C      
���  ��$��2��6R   =  7Microsoft.Research.DynamicDataDisplay.Charts.SimpleLine   Value 6(   �� *
MaxVoltage+6R     5D      
���  ��$��2��6R    6(   �� *
MinVoltage+6R   5E      5F      5G      5H      5I      5J      