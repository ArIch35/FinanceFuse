using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Interfaces;

namespace FinanceFuse.Services;

public class RoutingService
{
    private static Action<ObservableObject> _changeScreenCallback = null!;
    private readonly Dictionary<string, ObservableObject> _staticScreens;
    private static RoutingService _instance = null!;
    private static readonly object Locker = new();

    private RoutingService()
    {
        _staticScreens = new Dictionary<string, ObservableObject>();
    }

    public static void SetMainCallback(Action<ObservableObject> callback)
    {
        _changeScreenCallback ??= callback;
    }
    
    public static RoutingService GetInstance()
    {
        if (_instance != null!)
            return _instance;

        lock (Locker)
        {
            _instance ??= new RoutingService();
        }

        return _instance;
    }

    public void AddScreenToStaticScreen<T>(string screenName, T screen) where T : ObservableObject, IRoutable
    {
        if (_staticScreens.TryAdd(screenName, screen))
        {
            return;
        }
        _staticScreens.Add(screenName, screen);
    }
    public static void ChangeScreen(ObservableObject newScreen)
    {
        _changeScreenCallback(newScreen);
    }
    
    public void ChangeStaticScreen<T>(string staticScreenName, T? item = default) where T: IModelBase
    {
        if (_staticScreens.TryGetValue(staticScreenName, out var screen))
        {
            _changeScreenCallback(screen);
            if (screen is IRoutable routableScreen && item != null)
            {
                routableScreen.OnRouted(item);
            }
        }
    }
}