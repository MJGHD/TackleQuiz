   M S B A M L   `   `   `  ����A  =Tackle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Tackle.PagesTackle.Pages  5      !    Tackle.Pages.QuizScreenView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      B >Stylet, Version=1.1.21.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      &localclr-namespace:Tackle.Pages  5      .��5
      �   � RowDefinitions  5   
   �� 5      $
��Auto �6   $��25��6*   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
�������$��2��62   6E   �� 5   
   $��0��6   $��Right=�6!   $��16(�6=    �� Padding$ 0,0,5,0q�6K   ��6]   ��  �� Path$ Model.TimeLeftDisplay��� 5   
   $��0��6   $
��Left=�6!   $��16(�6<   $ 5,0,0,0q�6J   ��6\   �� $! Model.QuestionNumberDisplay��� 5   
   $��1��6   
����� �6!   �� 5   
   $��1��6   $��Center=�6!   $��20(�6>   ��6L   �� $ Model.CurrentQuestion��� 5   
   $��2��6N   $��0q�6[    �� SelectedIndex 6   �� $ Model.CurrentQuestionType�.@�5      �� $��0��6   $��0��6#   .��5      �� .+�5      �� ��6    �� $ Model.UserInput.Length� �� StringFormat$" Answer: ({0}/150 characters)���� 5       �� 	MaxLength$	 150��6v   $	��300��6�   ��6   �� $ Model.UserInput� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   5      5       �� 5"      $��1��6   $��0��6#   .��5#      �� .+�5$      �� 	Answer:6)   �� 5%      $	��300��6�   ��6   �� $ Model.UserInput�$ TwoWay=� J� PreviewTextInput 6Q   !  Stylet.Xaml.ActionExtension  *NumericalInputFilter+6�   5&      5(      �� 5*      $��2��6   $��0��6#   .��5,      �� ��6   �� $! Model.MultipleChoiceOptions�	 �� SelectedItem	 6S   �� $ Model.UserInput���5-      �� 5.      
 �� 
TargetType"
 ��6    .�5/      ��  �� Property � �� �� Value 50   "   �� 51   &    �� 
TargetType" ��66   .R�52   *   	� ��66   ��  d� 	IsChecked 53   &   ��  �� RelativeSource �� *TemplatedParent+$ 
IsSelected�54   '   56      57      59      5:      5<      �� 5>   
   $��3��6   $
��Left=�6   ��69     *PreviousQuestion+.��6_   Previous Question6r   �� 5?   
   $��3��6   $��Center=�6   ��6;     *Quit+.��6U   Quit6[   �� 5@   
   $��3��6   $��Right=�6   ��6:     *NextQuestion+��6\   �� $ Model.NextButtonText�5B      5C      