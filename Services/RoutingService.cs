using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;

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
        lock (Locker)
        {
            _changeScreenCallback ??= callback;
        }
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

    public void AddScreenToStaticScreen<T>(string screenName, T screen) where T : RoutableObservableBase
    {
        if (_staticScreens.TryAdd(screenName, screen))
        {
            return;
        }
        _staticScreens.Add(screenName, screen);
    }
    
    public static void ChangeScreen(RoutableObservableBase newScreen) 
    {
        ChangeScreen(newScreen, default!, default!);
    }
    public static void ChangeScreen(RoutableObservableBase newScreen, RoutableObservableBase currentScreenRef)
    {
        ChangeScreen(newScreen, default!, currentScreenRef);
    }
    public static void ChangeScreen(RoutableObservableBase newScreen, IModelBase item)
    {
        ChangeScreen(newScreen, item ,default!);
    }
    private static void ChangeScreen(RoutableObservableBase newScreen, IModelBase? item, RoutableObservableBase? currentScreenRef) 
    {
        _changeScreenCallback(newScreen);
        newScreen.OnRouted(item, currentScreenRef);
    }
    
    public void ChangeStaticScreen(string staticScreenName, RoutableObservableBase currentScreenRef)
    {
        ChangeStaticScreen(staticScreenName, default!, currentScreenRef);
    }
    public void ChangeStaticScreen(string staticScreenName, IModelBase item)
    {
        ChangeStaticScreen(staticScreenName, item, default!);
    }
    private void ChangeStaticScreen(string staticScreenName, IModelBase? item, RoutableObservableBase? currentScreenRef) 
    {
        if (!(_staticScreens.TryGetValue(staticScreenName, out var screen)))
        {
            return;
        }
        
        _changeScreenCallback(screen);
        if (screen is IRoutable routableScreen)
        {
            routableScreen.OnRouted(item, currentScreenRef);
        }
    }

    public bool CheckStaticScreenExist(string name)
    {
        return _staticScreens.ContainsKey(name);
    }
}