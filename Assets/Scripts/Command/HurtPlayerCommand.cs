using FrameWorkDesign;
using Model;

namespace Command
{
    public class HurtPlayerCommand : AbstractCommand
    {
        private int _hurt;


        public HurtPlayerCommand(int hurt)
        {
            _hurt = hurt;
        }

        protected override void OnExecute()
        {
            this.GetModle<IPlayerModel>().HP.Value -= _hurt;
        }
    }
}