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

    public void AddScreenToStaticScreen<T>(string screenName, T screen) where T : RoutableObservableBase
    {
        if (_staticScreens.TryAdd(screenName, screen))
        {
            return;
        }
        _staticScreens.Add(screenName, screen);
    }
    
    public static void ChangeScreen<TTarget>(TTarget newScreen) 
        where TTarget: ObservableObject, IRoutable 
    {
        _changeScreenCallback(newScreen);
    }
    public static void ChangeScreen<TTarget, TItem>(TTarget newScreen, TItem? item = default) 
        where TTarget: RoutableObservableBase
        where TItem: IModelBase
    {
        _changeScreenCallback(newScreen);
        newScreen.OnRouted<TItem, TTarget>(item);
    }
    public static void ChangeScreen<TSource, TTarget, TItem>(TTarget newScreen, TItem? item = default, TSource? currentScreenRef = default) 
        where TSource: RoutableObservableBase
        where TTarget: RoutableObservableBase
        where TItem: IModelBase
    {
        _changeScreenCallback(newScreen);
        newScreen.OnRouted(item, currentScreenRef);
    }
    
    public void ChangeStaticScreen<TScreen, TItem>(string staticScreenName, TItem? item = default, TScreen? currentScreenRef = default) 
        where TScreen: RoutableObservableBase
        where TItem: IModelBase
    {
        if (_staticScreens.TryGetValue(staticScreenName, out var screen))
        {
            _changeScreenCallback(screen);
            if (screen is IRoutable routableScreen && item != null)
            {
                routableScreen.OnRouted(item, currentScreenRef);
            }
        }
    }

    public bool CheckStaticScreenExist(string name)
    {
        return _staticScreens.ContainsKey(name);
    }
}