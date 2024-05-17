using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;

namespace FinanceFuse.Services;
public static class RoutingService
{
    private static Action<ObservableObject> _changeScreenCallback = null!;
    private static readonly Dictionary<string, ObservableObject> StaticScreens;
    private static readonly object Locker = new();

    static RoutingService()
    {
        StaticScreens = new Dictionary<string, ObservableObject>();
    }

    public static void SetMainCallback(Action<ObservableObject> callback)
    {
        lock (Locker)
        {
            _changeScreenCallback ??= callback;
        }
    }

    public static void AddScreenToStaticScreen<T>(string screenName, T screen) where T : RoutableObservableBase
    {
        if (StaticScreens.TryAdd(screenName, screen))
        {
            return;
        }
        StaticScreens.Add(screenName, screen);
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
    
    public static void ChangeStaticScreen(string staticScreenName, RoutableObservableBase currentScreenRef)
    {
        ChangeStaticScreen(staticScreenName, default!, currentScreenRef);
    }
    public static void ChangeStaticScreen(string staticScreenName, IModelBase item)
    {
        ChangeStaticScreen(staticScreenName, item, default!);
    }
    private static void ChangeStaticScreen(string staticScreenName, IModelBase? item, RoutableObservableBase? currentScreenRef) 
    {
        if (!(StaticScreens.TryGetValue(staticScreenName, out var screen)))
        {
            return;
        }
        
        _changeScreenCallback(screen);
        if (screen is RoutableObservableBase routableScreen)
        {
            routableScreen.OnRouted(item, currentScreenRef);
        }
    }

    public static bool CheckStaticScreenExist(string name)
    {
        return StaticScreens.ContainsKey(name);
    }
}