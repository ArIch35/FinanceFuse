using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia;
using Avalonia.VisualTree;
using System;
using System.Linq;

namespace FinanceFuse.Behaviors
{
    public static class ScrollToEndBehavior
    {
        public static readonly AttachedProperty<bool> ScrollToEndProperty =
            AvaloniaProperty.RegisterAttached<TabControl, bool>("ScrollToEnd", typeof(ScrollToEndBehavior), false);

        public static bool GetScrollToEnd(TabControl tabControl)
        {
            return tabControl.GetValue(ScrollToEndProperty);
        }

        public static void SetScrollToEnd(TabControl tabControl, bool value)
        {
            tabControl.SetValue(ScrollToEndProperty, value);
        }

        static ScrollToEndBehavior()
        {
            ScrollToEndProperty.Changed.AddClassHandler<TabControl>((sender, e) =>
            {
                if (e.NewValue != null)
                {
                    if (sender.GetVisualChildren().FirstOrDefault() is ScrollViewer scrollViewer)
                    {
                        scrollViewer.ScrollToEnd();
                    }
                }
            });
        }
    }
}
