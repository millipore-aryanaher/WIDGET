   M S B A M L   `   `   `  ����
  WIDGET    WPFWidget.PopupWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  PopupWindow��5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	   
������  � Topmost$
 True��5   	    � 
ResizeMode$ NoResize=�5	   	    � ShowInTaskbar$ False��5
   	   $	��850��5   	   $	��600��6    � 	Resources 5       �� 
TargetType �� Property	 �� Value
 �� 
TargetType��%  'n�       �� 5   
   " n�6   .�5      ��  �@J�Opacity$	 0Q�6(   �� 5       �@�OverridesDefaultStyle$
	 True��66   �� 5       � ��	 5      �� 5      "
 n�6&   .R�5      �� 
������ 6$   5      5      5      5      .��5      �� ���#E6F0FA �� CornerRadius$ 12��6"    �� Padding$ 15q�64   ���#B0C4DE$��1q�6W   ._�5   
   � .+�5      ]� $
��True��6   .+�5"      �  � Orientation$ 
Horizontal=�6   $��Center=�65   $	��Top=�6R   $��0,0,0,25q�6g   .+�5#      �� -   5         ��Name$ 	SearchBox��5#      $	��250��5$      $��34��6*   $��0,0,20,0q�5%      $��14(�5&      $��6q�5'      $��Center=�5(      
����������#A9CFF4 � ToolTip$ Enter filename��5+      �� 5-      -   5         ��Name$ SearchButton��5-      $	��100��5/      $��34��6)   $��0,0,20,0q�50      $��14(�51      $
��Bold"�52       � Cursor$
 Hand��53      ���#4A90E2
����������#4A90E2$��Search��57      ��58      �� 59      " ��6$   .�5:   "   ��  � ��	 �#4A90E26?   �� 5;   "    � ��
	 �����6?   �� 5<   "    � ��	 �#4A90E26@   �� 5=   "    �@�Cursor$
	 Hand��6;    �� Triggers 5>   "   P� 5?   &    P� Property �@J�IsMouseOver P� Value$
 True��6E   . �5@   *   ��  � ��	 �#6AB0F36G   5A   '   5C      5E      �� 5G      -   5         ��Name$ ConfigComboBox��5G      $	��240��5H      $��34��6+   $��14(�5I      $��0,0,0,0q�5J      ���#A9CFF45L      5M      �� 5P      $
��Auto=�6   $
��Auto=�5Q      .��5R      &   System.Windows.Controls.DataGrid  -   5          Name$ DataTableGrid��5R         AutoGenerateColumns$ False��5S         CanUserAddRows$ False��5T         
IsReadOnly$
 True��5U      $��13(�5V      
�������   HeadersVisibility$	 All=�5X         GridLinesVisibility$	 All=�5Y      ���#A9CFF4$��1q�5[      $��0q�5\         RowHeaderWidth$ 0Q�5]         AlternatingRowBackground �#F7FAFD   Columns 5`      0  *System.Windows.Controls.DataGridTextColumn  5a      ,  &System.Windows.Controls.DataGridColumn   Header$ ITEM NO.��61       Width5  /System.Windows.Controls.DataGridLengthConverter$  80 6_   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6C   �� *	ITEM_NO+6_     5b      $ PART NUMBER��61   $	  150 6h   ! 6F   �� *[PART NUMBER]+6h     5c      $ DESCRIPTION��61   $	  300 6f   ! 6F   �� *DESCRIPTION+"   ElementStyle" 5d   "   �� 5e   &   " ��6,   .�5f   *   ��  � ��$
	 Wrap=�6I   �� 5g   *    � ��$	 CharacterEllipsis=�6I   �� 5h   *    � ��$		 Top=�6N   5i   '   5k        5l      $ COMMENTS��61   $	  400 6`   ! 6C   �� *
COMMENTS+" 5m   "   �� 5n   &   " ��6,   .�5o   *   ��  � ��$
	 Wrap=�6I   �� 5p   *    � ��$	 CharacterEllipsis=�6I   �� 5q   *    � ��$		 Top=�6N   5r   '   5t      #   RowStyle# 5y      �� 5z      )  #System.Windows.Controls.DataGridRow"  6$   .�5{   "   ��  � ��$		 Top=�6F   �� 5|   "    � ��$
	 Auto��6;   5}      5      5�      5�      n� 5�      -   5        $ n�Name$$ TopThumb��5�      $��5��6&   $	��Top=�61   $ SizeNS��6I   6Y   n� 5�      -   5        $$ BottomThumb��5�      $��5��6)   $��Bottom=�64   $ SizeNS��6O   6_   n� 5�      -   5        $$ 	LeftThumb��5�      $��5��6'   $
��Left=�61   $ SizeWE��6L   6\   n� 5�      -   5        $$ 
RightThumb��5�      $��5��6(   $��Right=�62   $ SizeWE��6N   6^   n� 5�      -	   5        $$ TopLeftThumb��5�      $��10��6*   $��10��65   $
��Left=�6A   $	��Top=�6\   $ SizeNWSE��6t   6�   n� 5�      -
   5        $$ TopRightThumb��5�      $��10��6+   $��10��66   $��Right=�6B   $	��Top=�6^   $ SizeNESW��6v   6�   n� 5�      -   5        $$ BottomLeftThumb��5�      $��10��6-   $��10��68   $
��Left=�6D   $��Bottom=�6_   $ SizeNESW��6z   6�   n� 5�      -   5        $$ BottomRightThumb��5�      $��10��6.   $��10��69   $��Right=�6E   $��Bottom=�6a   $ SizeNWSE��6|   6�   5�      5�      5�      