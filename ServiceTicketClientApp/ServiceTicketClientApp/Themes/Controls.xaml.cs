using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bootstrap
{
    public partial class StyleResourceDictionary
    {
    }

    public class BaseWindowCommand
    {
        public virtual bool CanExecute(object parameter) => true;

        public event EventHandler CanExecuteChanged;
    }

    public class MaximizeWindowCommand : BaseWindowCommand, ICommand
    {
        public void Execute(object parameter)
        {
            var setting = 0b101;
            if (parameter is Window window && window.ResizeMode != ResizeMode.NoResize && !setting.ToString().Equals(window.Tag?.ToString()))
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }
    }

    public class MinimizeWindowCommand : BaseWindowCommand, ICommand
    {
        public void Execute(object parameter)
        {
            var window = parameter as Window;
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }

    public class CloseWindowCommand : BaseWindowCommand, ICommand
    {
        public void Execute(object parameter) => (parameter as Window)?.Close();
    }


    public class MouseDragBehavior
    {
        public static Window GetLeftMouseButtonDrag(DependencyObject obj) => (Window)obj.GetValue(LeftMouseButtonDrag);

        public static void SetLeftMouseButtonDrag(DependencyObject obj, Window window) => obj.SetValue(LeftMouseButtonDrag, window);

        public static readonly DependencyProperty LeftMouseButtonDrag = DependencyProperty.RegisterAttached(nameof(LeftMouseButtonDrag), typeof(Window), typeof(MouseDragBehavior), new UIPropertyMetadata(null, OnLeftMouseButtonDragChanged));

        private static void OnLeftMouseButtonDragChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as UIElement;
            if (element != null)
            {
                element.MouseLeftButtonDown += (_sender, _e2) => (element.GetValue(LeftMouseButtonDrag) as Window)?.DragMove(); ;
            }
        }
    }

    public static class MouseDoubleClickBehavior
    {
        public static ICommand GetExecuteCommand(DependencyObject obj) => (ICommand)obj.GetValue(ExecuteCommand);

        public static void SetExecuteCommand(DependencyObject obj, ICommand command) => obj.SetValue(ExecuteCommand, command);

        public static readonly DependencyProperty ExecuteCommand = DependencyProperty.RegisterAttached("ExecuteCommand", typeof(ICommand), typeof(MouseDoubleClickBehavior), new UIPropertyMetadata(null, OnExecuteCommandChanged));

        public static Window GetExecuteCommandParameter(DependencyObject obj) => (Window)obj.GetValue(ExecuteCommandParameter);

        public static void SetExecuteCommandParameter(DependencyObject obj, ICommand command) => obj.SetValue(ExecuteCommandParameter, command);

        public static readonly DependencyProperty ExecuteCommandParameter = DependencyProperty.RegisterAttached("ExecuteCommandParameter", typeof(Window), typeof(MouseDoubleClickBehavior));

        private static void OnExecuteCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                control.MouseDoubleClick += (_sender, _e) =>
                {
                    var command = control.GetValue(ExecuteCommand) as ICommand;
                    var commandParameter = control.GetValue(ExecuteCommandParameter);
                    if (command.CanExecute(e))
                    {
                        command.Execute(commandParameter);
                    }
                };
            }
        }
    }
}
