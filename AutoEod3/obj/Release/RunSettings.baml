   M S B A M L   `   `   `  ���� C  ?AutoEod3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:AutoEod3.ModelsAutoEod3.Models  ?$clr-namespace:AutoEod3.Models.ResultAutoEod3.Models.Result      AutoEod3.RunSettings   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:AutoEod3.Models     4res$clr-namespace:AutoEod3.Models.Result     -     � Title$%  Настойки запуска��$	��500��$	��791�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�$��Center=�$��Center=� � ShowActivated .  � Icon$ icons8-robot-2-80.ico��.���  � RowDefinitions �� $��* ��� $��50 �.+��� $��Stretch=�$��Stretch=�$��0��.@��� $)��#Обновные настройки��.����  �� Orientation$ Vertical=�.+��� $��Подключения��$
��Left=�$	��274��$��10,5,0,0q�$	��Top=�$	��756��.���� $��Stretch=�$	��Top=� � DataContext ��  �� Source$ VariantConnections��.+��� -   	 ��Name$	 listTypeConnections��$��
10,10,10,0q�$
 �� IsSynchronizedWithCurrentItem$

 TrueY�����  �� SelectedItem �� *VariantConnectionName+���� .R��� $ 
Horizontal=�.+��� $!��Images\databaseCategory.png��$��32��$��32���� ���� *VariantConnectionName+�� -    ��Name$ listConnections��$	��200��$��
10,10,10,0q�$	��Top=� �� *SelectedItem+ �� ElementName$ listTypeConnections������ *Connections+ � ContextMenu �� .@�m� -   $��Выбрать все��
 � Tag$	 All��m� -   $)��#Снять выбор со всех��$ NoAll��m� -   $��Инверсия��$ Inverse������ .R��� $ 
Horizontal=�.+��� $��Images\db.jpg��$��24��$��24���� $��5,0,0,0q� d� 	IsChecked �� *Check+���� *
FullName+�� $��Даты��$	��100��$��	10,5,10,0q�$	��Top=�    AutoEod3.Models.Settings  ���� -    ��Name$ convasDates��.+��� $��
Дата 1��$
��Left=�$��	10,10,0,0q�$	��Top=�(  "System.Windows.Controls.DatePicker  -     Name$ dateRun1��   Date1   SelectedDate# Z  �� Left$ 60��
 �� Top$ 12��$	��116���� $��
Дата 2��$
��Left=�$��	10,40,0,0q�$	��Top=�  -	   $ dateRun2��$ 60��$ 42��$	��116�� ��  �� Path$ (local:Settings.Date2)� �� Mode$ TwoWay=��� $��
Дата 3��$
��Left=�$��
250,10,0,0q�$	��Top=�  -
   $ dateRun3��$	 300��$ 12��$	��116�� �� $ (local:Settings.Date3)�$ TwoWay=��� $��
Дата 4��$
��Left=�$��
250,40,0,0q�$	��Top=�  -   $ dateRun4��$	 300��$ 42��$	��116�� �� $ (local:Settings.Date4)�$ TwoWay=��� $ ��Дополнительно��.���� -   $ 	convasDop��.+��� $)��#Количество потоков��$ 5��$ 10���� $��50��$	 142��$ 14������ $" (local:Settings.CountThread)�$ TwoWay=��� -   $	 resultFormat�� �� SelectedIndex$ 0��$��25��$	��122��$	 142��$ 42��{�.  .@��� $ 1��.���� $ 
Horizontal=�.+��� $��Images\csv.png��$��16��$��16���� �� $�CSV файл���� $ 2��.���� $ 
Horizontal=�.+��� $��Images\excel.png��$��16��$��16���� �� $�Excel файл���� $.��(Результат сохранить в��$ 5��$ 39���� $h��bВ случае возникновения ошибки при выполнении скрипта��$	��100��$	��520��$ 10��$ 72��.���� .+��� $M��GКоличество попыток выполнения скрипта��$ 5��$ 9���� $��50��$	 250��$ 13������ $" (local:Settings.RepeatCount)�$ TwoWay=��� $A��;Интервал между попытками (минут)��$ 5��$ 36���� $��50��$	 250��$ 40������ $$ (local:Settings.RepeatTimeout)�$ TwoWay=�� $��Bottom=�$��Stretch=�$��1��.+��� -    ��Name$ btnStartRun��$��Right=�$��Отмена��$��25��$	��130��$��0,10,150,10q� �� IsCancel . �� -   $ btnCloseRun��$��Right=�$��Запуск��$��25��$	��130��$��
0,10,10,10q� �� 	IsDefault . 