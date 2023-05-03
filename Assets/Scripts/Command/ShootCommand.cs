using FrameWorkDesign;

namespace Command
{
    public class ShootCommand : AbstractCommand
    {
        public static readonly ShootCommand Single = new ShootCommand();
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            gunSystem.gunInfo.BulletCountInGun.Value--;
            gunSystem.gunInfo.State.Value = GunState.Shooting;

            var gunConfig = this.GetModle<IGunConfigModel>().GetGunConfigItemByName(gunSystem.gunInfo.Name.Value);
            
            var timeSystem = this.GetSystem<ITimeSystem>();    
            timeSystem.AddDelayTask(1/gunConfig.Frequency, () =>
            {
                gunSystem.gunInfo.State.Value = GunState.Idle;

                if (gunSystem.gunInfo.BulletCountInGun.Value == 0 && gunSystem.gunInfo.BulletCountOutGun.Value > 0)
                {
                    this.SendCommand<ReloadCommand>();
                }
            });
        }
    }
}