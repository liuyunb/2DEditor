using FrameWorkDesign.Rule;

namespace FrameWorkDesign
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanGetModle, ICanGetSystem, ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        public void Execute();
    }

    public abstract class AbstractCommand : ICommand
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

        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();

    }
}
