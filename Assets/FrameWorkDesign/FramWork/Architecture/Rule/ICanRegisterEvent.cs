using System;
using FrameWorkDesign;

public interface ICanRegisterEvent : IBelongToArchitecture
{

}

public static class CanRegisterEventExtension
{
    public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> e) where T : new()
    {
        return self.GetArchitecture().RegisterEvent(e);
    }

    public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> e)
    {
        self.GetArchitecture().UnRegister(e);
    }
}