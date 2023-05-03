
using FrameWorkDesign;
using FrameWorkDesign.Rule;

public interface IController : IBelongToArchitecture, ICanSendCommand, ICanGetModle, ICanGetSystem, ICanRegisterEvent, ICanSendQuery
{
    
}
