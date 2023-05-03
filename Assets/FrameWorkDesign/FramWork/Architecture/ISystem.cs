
using FrameWorkDesign;

public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetModle, ICanGetUtility, ICanSendEvent, ICanRegisterEvent
{
    // public IArchitecture Architecture { get; set; }
    void Init();
}

public abstract class AbstractSystem : ISystem
{
    private IArchitecture mArchitecture;
    
    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return mArchitecture;
    }

    void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
    {
        mArchitecture = architecture;
    }

    void ISystem.Init()
    {
        OnInit();
    }

    protected abstract void OnInit();
}
