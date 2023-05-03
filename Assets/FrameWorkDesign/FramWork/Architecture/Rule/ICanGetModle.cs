
using FrameWorkDesign;
using UnityEngine;

public interface ICanGetModle : IBelongToArchitecture
{

}
public static class CanGetModleExtension
{
    public static T GetModle<T>(this ICanGetModle self) where T : class, IModel
    {
        return self.GetArchitecture().GetModle<T>();
    }
}
