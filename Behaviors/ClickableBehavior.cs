using System.Windows.Input;
using Avalonia;
using Avalonia.Data;
using Avalonia.Interactivity;
namespace FinanceFuse.Behaviors;
using Avalonia.Input;

public class ClickableBehavior: AvaloniaObject
{
    static ClickableBehavior()
    {
        CommandProperty.Changed.AddClassHandler<Interactive>(HandleCommandChanged);
    }
    
    public static readonly AttachedProperty<ICommand> CommandProperty = AvaloniaProperty.RegisterAttached<ClickableBehavior, Interactive, ICommand>(
        "Command", null!, false, BindingMode.OneTime);
    
    private static readonly AttachedProperty<object> CommandParameterProperty = AvaloniaProperty.RegisterAttached<ClickableBehavior, Interactive, object>(
        "CommandParameter");
    
    private static void HandleCommandChanged(Interactive interactElem, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is ICommand)
        {
             interactElem.AddHandler(InputElement.PointerPressedEvent, Handler!);
        }
        else
        {
             interactElem.RemoveHandler(InputElement.PointerPressedEvent, Handler!);
        }
    }
    
    private static void Handler(object s, RoutedEventArgs e)
    {
        if (!(s is Interactive interactElem))
            return;
        
        var commandParameter = interactElem.GetValue(CommandParameterProperty);
        var commandValue = interactElem.GetValue(CommandProperty);
        if (commandValue.CanExecute(commandParameter))
        {
            commandValue.Execute(commandParameter);
        }
    }
    
    public static void SetCommand(AvaloniaObject element, ICommand commandValue)
    {
        element.SetValue(CommandProperty, commandValue);
    }
    
    public static ICommand GetCommand(AvaloniaObject element)
    {
        return element.GetValue(CommandProperty);
    }
}