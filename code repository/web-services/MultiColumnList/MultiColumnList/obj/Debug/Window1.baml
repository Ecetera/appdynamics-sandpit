   M S B A M L   `   `   `  ����O  KSimulationSettingsUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MultiColumnListMultiColumnList  5          MultiColumnList.Window1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      /localclr-namespace:MultiColumnList     5      -   5          �Name$  Simulation_settings��5   	    � Title$0 *Simulation settings (Rental service (WCF))��5      $��810.448��68   $��1361.464��6I    � 
ResizeMode$ NoResize=�6Z   .��5      �� $��0,0,46,0q�6   ._�5   
   � $��10,10,-39,-27q�6    � 	Resources 5         BlogDataTemplate � ColumnDefinitions z� SharedSizeGroup 
 Header �� 
TargetType �� Padding  
ListHeader  HeaderedScrollViewer �� 
TargetType	 �� Property
 �� Value � RowDefinitions)   #MultiColumnList.SetHorizontalOffset   Offset �� RelativeSource ��Name-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation ��Name �� Orientation �� ScrollableWidth � Maximum �� ViewportWidth �� ViewportSize �� HorizontalOffset � Value, �� %ComputedHorizontalScrollBarVisibility �� ScrollableHeight �� ViewportHeight �� VerticalOffset* �� #ComputedVerticalScrollBarVisibility � IsSharedSizeScope K� IsItemsHost��%(  &	        &	 s    &	      7  7  7  7  7  7  &	 �    7  '�� ,
    7  �� 5      .R�5      � $��20(�6    5      �� 5      $
��Auto �6/   $ BloggerColumn��6<   �� 5      $
��Auto �6/   $ BlogSiteColumn��6<   �� 5      $
��Auto �6/   $ BlogSiteColumn��6<   �� 5      $
��Auto �6/   $ BlogSiteColumn��6<   �� 5      $
��Auto �6/   $ BlogSiteColumn��6<   �� 5      $
��Auto �6/   $ BlogSiteColumn��6<   .+�5"      �� $��0��6$   $��	10,0,10,0q�64   ��6G   �� *
methodname+�� 5#      $��1��6$   $��	10,0,10,0q�64   ��6G   �� *sleep+�� 5$      $��2��6$   $��	10,0,10,0q�64   ��6G   �� *throwException+�� 5%      $��3��6$   $��	10,0,10,0q�64   ��6G   �� *consumeheap+�� 5&      $��4��6$   $��	10,0,10,0q�64   ��6G   �� *consumeheapfactor+�� 5'      $��5��6$   $��	10,0,10,0q�64   ��6G   �� *
consumecpu+5(      5)      �� 53      " ��61   .R�54      �� 
�������
�������$��20(�56      $ 	10,3,10,4q�57      ._�58      �� 59      5:      � 5?      $��5q�6*    5@      �� 5A      $
��Auto �6+   $ BloggerColumn��68   �� 5B      $
��Auto �6+   $ BlogSiteColumn��68   �� 5C      $
��Auto �6+   $ BlogSiteColumn��68   �� 5D      $
��Auto �6+   $ BlogSiteColumn��68   �� 5E      $
��Auto �6+   $ BlogSiteColumn��68   �� 5F      $
��Auto �6+   $ BlogSiteColumn��68   .+�5H      �� $��0��6   8��  $��Method Name��6P   �� 5I      $��1��6   8�� $��
Sleep time��6P   �� 5J      $��2��6   8�� $��throw Exception��6P   �� 5K      $��3��6   8�� $��consume heap��6P   �� 5L      $��4��6   8�� $��heap growth rate��6P   �� 5M      $��5��6   8�� $��Max out cpu��6P   5N      �� 5T      " ��65   .�5U      �� 	 � ��
 5V      �� 5W      " ��6.   .R�5X   "   � #��z��6'    5Y   &   �� 5Z   *   $��* �6;   �� 5[   *   $
��Auto �6;    5]   &   �� 5^   *   $��* �68   �� 5_   *   $
��Auto �68   .+�5b   &   ]� #��z��60   .+�5c   *   �� $	��Top=�67   $��Hidden=�5e      $��Hidden=�5f      |�.  8��   5d      �� *HorizontalOffset+ �� *TemplatedParent+5h      �� 5j   *   $! PART_ScrollContentPresenter��6A   $ Local=�5k      5l   '   �� 5n   &   $ PART_HorizontalScrollBar��60   $ 
Horizontal=�5o      $��1��5p      # z 5q      # z 5r      # z 5s      #y�z 5t      �� 5v   &   $ PART_VerticalScrollBar��60   $��1��5w      # z 5x      # z 5y      # z 5z      #y�z 5{      5|   #   5}      5      5�      �� 5�      " ��61   .�5�      �� 	 � ��
 5�      �� 5�      .R�5�   "   �� 8��   . .��5�   &   ��  . 61   5�   #   5�      5�      5�      5�      .+�5�      �� -   5         ��Name$ 
lbTodoList��5�      #��[  6*   $
��Left=�6[   $
��1211��6v   $	��367��6�   $	��Top=�6�   $��	22,10,0,0q�6�   �� 5�      -   5        $��Update��5�      $
��Left=�6&   $��
39,641,0,0q�6A   $	��Top=�6U   $	��254��6m   $��47��6�   �� 5�      -   5          ��Name$  txtSleeptimer��5�      $
��Left=�6-   $��23��6H   $��
39,530,0,0q�6T   ! �� TextWrapping$
! Wrap=�6h   $��TextBox��6|   $	��Top=�6�   $	��120��6�   $��2q�6�   �� 5�      $��Sleep time(Sec)��6   $
��Left=�6.   $��
39,499,0,0q�6I   $	��Top=�6]   �� 5�      -   5        " ��Name$" chkException��5�      $��Throw exception��6-   $
��Left=�6G   $��
39,563,0,0q�6b   $	��Top=�6v   $��18��6�   $	��160��6�   �� 5�      -   5        # ��Name$# 	lblmethod��5�      $��
Methodname��6'   $
��Left=�6<   $��
39,468,0,0q�6W   $	��Top=�6k   $	��254��6�   ���	#FFF790906�   �� 5�      -   5        $��chkUnmanaged��5�      $��consume heap��6+   $
��Left=�6B   $��
39,581,0,0q�6]   $	��Top=�6q   �� 5�      $$��Please select a method (above)��6   $
��Left=�6=   $��
39,442,0,0q�6X   $	��Top=�6l   $	��225��6�   �� 5�      -   5        $��chkCpu��5�      $��Max CPU��6%   $
��Left=�67   $��
39,601,0,0q�6R   $	��Top=�6f   $��14��6~   $	��120��6�   �� 5�      -	   5        $��heaprate��5�      $
��Left=�6&   $��23��6A   $��199,579,0,0q�6M   $
! Wrap=�6b   $��TextBox��6v   $	��Top=�6�   $	��120��6�   �� 5�      $#��Heap Growth rate(per reqeust)��6   $
��Left=�6<   $��199,548,0,0q�6W   $	��Top=�6l   $	��120��6�   	� 5�      -
   5        $��radioFacade1��5�      $��DataFacade1��6/   $
��Left=�6E   $��408,468,0,0q�6`   $	��Top=�6u   	� 5�      -   5        $��radioFacade2��5�      $��DataFacade2��6/   $
��Left=�6E   $��408,499,0,0q�6`   $	��Top=�6u   5�      5�      5�      