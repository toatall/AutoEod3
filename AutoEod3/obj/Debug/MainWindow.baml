   M S B A M L   `   `   `  ����C  ?AutoEod3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:AutoEod3AutoEod3  5   	   1clr-namespace:AutoEod3.ModelsAutoEod3.Models  5   	   ?$clr-namespace:AutoEod3.Models.ResultAutoEod3.Models.Result  5   	       AutoEod3.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:AutoEod3     5   	   0modelsclr-namespace:AutoEod3.Models     5   	   7result$clr-namespace:AutoEod3.Models.Result     5   	   -   5          �Name$
  Main��5
   9    � Title$ Авто ЭОД v3.0��6	   $	��600��6   $
��1300��6,    � WindowStartupLocation$ CenterScreen=�6G    � Icon$ icons8-robot-2-80.ico��6�    � TaskbarItemInfo 5      *  $System.Windows.Shell.TaskbarItemInfo  5   
   -   5        taskBarInfo��   ProgressState$
 None=�5   /   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��330 �6   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��1��6    �� ShowsPreview . $��3��68   $��Center=�6B   $��Stretch=�6_   $��1��6{   +  %System.Windows.Controls.Ribbon.Ribbon  5   
   -   5        	  Name$	 	RibbonWin��5      
 �� SelectedIndex$
 0��6%   $��Stretch=�67   $��Stretch=�6S   $��0��6q   $��3��6~      HelpPaneContent 5      1  +System.Windows.Controls.Ribbon.RibbonButton  5      -   5           SmallImageSource$ Images\help.png��5      6B      ApplicationMenu 5$      :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5%      5  /System.Windows.Controls.Ribbon.RibbonMenuButton   KeyTip$ Q��6(   .@�5&      >  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -   5          Name$ btnExit��5&   ?   $��
Выход��60   3  -System.Windows.Controls.Ribbon.RibbonMenuItem   ImageSource$ Images\quit.png��6f   5'      .@�5*      .  (System.Windows.Controls.Ribbon.RibbonTab  $��Главная��6      KeyTip$ H��6)   .@�5,      0	  *System.Windows.Controls.Ribbon.RibbonGroup	  -   5         	 Name$	 Run��5,      $��Выполнение��6+   .@�5-        -   5          Name$ btnRunSelect��5-   #      LargeImageSource$ Images\play.png��69      Label$ Выполнить��6\      KeyTip$ T��6n      ToolTipTitle$ Выполнить��6y      ToolTipDescription$� �Запускает выполнение выделенного скрипта (галочки при этом не учитываются)��6�   6�     5.      -   5        $ 	btnRunAll��5.   #   $ Images\playAll.png��66   $+ %Выполнить выбранные��6\   $ T��6x   $ Выполнить��6�   $f `Запускает выполнение отмеченных галочками скриптов��6�   6�     5/      -	   5        $ 	btnCancel��5/   #   $ Остановить��66   $ Images\stop.png��6I   $ S��6l   {�.  6�   50      	  51      -
   5        $ waitRunGroup��51      $'��!Отложенный запуск��64   $	��135��6O   $y�Hidden=�6[   .@�52      �� -   5         ��Name$ waitRunText��52       $��-��65   $��	ExtraBold"�6>   $��24(�6U   $
��Wrap=�6c   #��� ��6w   $��Center=�6�   $��Center=�6�   53      55      56      R� 57   
   -   5         R�Name$ treeQBE��57   Z   $��Stretch=�6   $��Stretch=�6/   $��2��6M    � 	Resources 58       �� DataType
   AutoEod3.Node �� ItemsSource �� Orientation d� 	IsChecked
  J� Uid! �� EndPoint" �� 
StartPoint# � Color$ � Offset% �� Color��%�  (��       59   +   *,
 +)(�� \    5B   &   * SystemColors.HighlightBrushKey+)(�� �    5F   &   *SystemColors.ControlBrushKey+)(�� �    5J   "   *$"SystemColors.HighlightTextBrushKey+)(�� �    5K   "   *" SystemColors.ControlTextBrushKey+)�� 59      " 
 6+    6J   �� *
Children+.R�5:      �� $ 
Horizontal=�6!   .+�5;      �� -   5         5;   #   �� *	IsChecked+  6C   �� *Id+6�   � 5<      $��5,0,5,0q�6   .+�5=      �� $��16��6;   $��16��6J   ��6$   �� *Ico+6J   5>      �� 5?      ��6$   �� *Text+5@      5A      �� 5B      $	! 0,1&�6X   $	" 0,0&�6g   .��5C      � $# 	#FFD9F4FF��6#   $$ 0Q�65   � 5D      $# 	#FF9BDDFB��6#   $$ 1Q�65   5E      �� 5F      $	! 0,1&�6V   $	" 0,0&�6e   .��5G      � $# 	#FFEEEDED��6#   $$ 0Q�65   � 5H      $# 	#FFDDDDDD��6#   $$ 1Q�65   5I      �� 5J      $% Black��6X   �� 5K      $% Black��6V   5L      ��5M      �� 5N      & �� 
TargetType"& Q�6   .�5O      �� ' �� Property' �@Q�
IsExpanded( �� Value( 63   �� ) �� Path$) 
IsExpanded�* �� Mode$* TwoWay=��� 5P      ' � ��$	( 1.5q�68   + �� Triggers+ 5Q      P� 5R      , P� Property, �@Q�
IsSelected- P� Value- . . �5S      �� ' � ��( �#adc6e56<   5T      ]� 5U      . ]� 
Conditions. 5V      �� 5W   "   / �� Property/ �@Q�
IsSelected0 �� Value0 . 6B   �� 5X   "   / �@Q�IsSelectionActive0 .  6I   ./�5Z      �� ' � ��
( �����6<   5[      1 �� 	Resources1 5]      ��%^   '��       �� 5^      "& ��6    .�5_      �� ' �@��CornerRadius$( 2��6=   5`      5a      5b      5d      � 5g   
   $��2��6   $��2��6    5h      �� 5i      $��* �6    �� 5j      $��50 �6    .+�5l      �� -   5        2 ��Name$2 listViewResult��5l      $��Stretch=�6/   $��Stretch=�6M   $��0��6i   3 �� View3 5m      �� 5n      .P�5o      �� $�� ��6)   4 �� Width$4 35��63   5 �� CellTemplate5 5p      �� 5q   "   .R�5r   &   �� $��20��6J   ��6,   �� *
StatusIcon+6J   5s   #   5u      �� 5v      $��#��6)   $4 30��6X   6 �� DisplayMemberBinding6 64   �� *Id+6X   �� 5w      $��ИФНС��6)   $	4 100��6a   6 67   �� *
CodeIfns+6a   �� 5x      $��Скрипт��6)   $	4 200��6e   6 69   �� *
ScriptName+6e   �� 5y      $��Статус��6)   $	4 300��6e   6 69   �� *
StatusText+6e   5z      5|      � 5}      $��Stretch=�6   $��Bottom=�61   $��50��6L   $��2��6X   .+�5~      �� 7 �� TextAlignment$
7 Left=�6   $��Center=�61   $��15 0 0 0q�6L   Выполнено �� 5       -   5        $ textCountThreadComplete��5   *   $��0��6K   6V    из �� 5�      -   5        $ textCountThreadAll��5�   #   $��0��6?   6J    документов6a   5�      5�      5�      5�      