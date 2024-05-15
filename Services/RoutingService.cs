using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

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
    
    public static RoutingService GetInstance(Action<ObservableObject>? changeScreenCallback = null)
    {
        if (_instance != null)
            return _instance;

        lock (Locker)
        {
            _instance ??= new RoutingService();
        }

        return _instance;
    }

    public void AddScreenToStaticScreen(string screenName, ObservableObject screen)
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
    
    public void ChangeScreen(string staticScreenName)
    {
        if (_staticScreens.TryGetValue(staticScreenName, out var screen))
        {
            _changeScreenCallback(screen);
        }
    }
}