   M S B A M L   `   `   `  ���� C  ?AutoEod3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:AutoEod3AutoEod3  ?$clr-namespace:AutoEod3.Models.ResultAutoEod3.Models.Result      AutoEod3.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:AutoEod3     7result$clr-namespace:AutoEod3.Models.Result     -     �Name$
  Main�� � Title$ Авто ЭОД v3.0��$	��600��$
��1300�� � WindowStartupLocation$ CenterScreen=� � Icon$ icons8-robot-2-80.ico�� � TaskbarItemInfo *  $System.Windows.Shell.TaskbarItemInfo  -   taskBarInfo��   ProgressState$
 None=�.���  � ColumnDefinitions �� $	��330 ��� $
��Auto ��� $��* � � RowDefinitions �� $
��Auto ��� $��* �.+��� $��1�� �� ShowsPreview . $��3��$��Center=�$��Stretch=�$��1��+  %System.Windows.Controls.Ribbon.Ribbon  -   	  Name$	 	RibbonWin��
 �� SelectedIndex$
 0��$��Stretch=�$��Stretch=�$��0��$��3��   HelpPaneContent 1  +System.Windows.Controls.Ribbon.RibbonButton  -      SmallImageSource$ Images\help.png��   QuickAccessToolBar =  7System.Windows.Controls.Ribbon.RibbonQuickAccessToolBar     ApplicationMenu :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5  /System.Windows.Controls.Ribbon.RibbonMenuButton   KeyTip$ Q��.@�>  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -     Name$ btnExit��$��
Выход��3  -System.Windows.Controls.Ribbon.RibbonMenuItem   ImageSource$ Images\quit.png��.@�.	  (System.Windows.Controls.Ribbon.RibbonTab	  $��Главная�� 	  KeyTip$ H��.@�0
  *System.Windows.Controls.Ribbon.RibbonGroup
  -    
 Name$	 Run��$��Выполнение��.@�  -     Name$ btnRunSelect��   LargeImageSource$ Images\play.png��   Label$ Выполнить��   KeyTip$ T��   ToolTipTitle$ Выполнить��   ToolTipDescription$� �Запускает выполнение выделенного скрипта (галочки при этом не учитываются)��  -   $ 	btnRunAll��$ Images\playAll.png��$+ %Выполнить выбранные��$ T��$ Выполнить��$f `Запускает выполнение отмеченных галочками скриптов��  -	   $ 	btnCancel��$ Остановить��$ Images\stop.png��$ S��{�.  
  -
   $ waitRunGroup��$'��!Отложенный запуск��$	��0,1q�$	��215��$y�Hidden=�.@��� -    ��Name$ waitRunText��$ ��Осталось {Binding}��$��	ExtraBold"�$��24(�$��Center=�$
��Wrap=�R� -    R�Name$ treeQBE��$��Stretch=�$��Stretch=�$��2�� � 	Resources  �� DataType   AutoEod3.Node �� ItemsSource �� Orientation  d� 	IsChecked
! J� Uid" �� EndPoint# �� 
StartPoint$ � Color% � Offset& �� Color��%\  (��       *, +)(�� �     * SystemColors.HighlightBrushKey+)(��     *SystemColors.ControlBrushKey+)(�� n    *$"SystemColors.HighlightTextBrushKey+)(��     *" SystemColors.ControlTextBrushKey+)�� "   �� *
Children+.R��� $ 
Horizontal=�.+��� -     �� *	IsChecked+! �� *Id+� $��5,0,5,0q�.+��� $��16��$��16������ *Ico+�� ���� *Text+�� $	" 0,1&�$	# 0,0&�.��� $$ 	#FFD9F4FF��$% 0Q�� $$ 	#FF9BDDFB��$% 1Q��� $	" 0,1&�$	# 0,0&�.��� $$ 	#FFEEEDED��$% 0Q�� $$ 	#FFDDDDDD��$% 1Q��� $& Black���� $& Black������ ' �� 
TargetType"' Q�.��� ( �� Property( �@Q�
IsExpanded) �� Value) �� * �� Path$* 
IsExpanded�+ �� Mode$+ TwoWay=��� ( � ��$	) 1.5q�, �� Triggers, P� - P� Property- �@Q�
IsSelected. P� Value. . . ��� ( � ��) �#adc6e5]� / ]� 
Conditions/ �� 0 �� Property0 �@Q�
IsSelected1 �� Value1 . �� 0 �@Q�IsSelectionActive1 .  ./��� ( � ��
) �����2 �� 	Resources2 ��%9   '��       �� "' ��.��� ( �@��CornerRadius$) 2��� $��2��$��2�� �� $��* ��� $��50 �.+��� -   3 ��Name$3 listViewResult��$��Stretch=�$��Stretch=�$��0��4 �� View4 �� .P��� $�� ��5 �� Width$5 35��6 �� CellTemplate6 �� .R��� $��20������ *
StatusIcon+�� $��#��$5 30��7 �� DisplayMemberBinding7 �� *Id+�� $��ИФНС��$	5 100��7 �� *
CodeIfns+�� $��Скрипт��$	5 200��7 �� *
ScriptName+�� $��Статус��$	5 300��7 �� *
StatusText+� $��Stretch=�$��Bottom=�$��50��$��2��.+��� 8 �� TextAlignment$
8 Left=�$��Center=�$��15 0 0 0q�Выполнено �� -   $ textCountThreadComplete��$��0�� из �� -   $ textCountThreadAll��$��0�� документов