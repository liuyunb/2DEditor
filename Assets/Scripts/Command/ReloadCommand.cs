using System.Collections;
using System.Collections.Generic;
using FrameWorkDesign;
using UnityEngine;

public class ReloadCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var gunInfo = this.GetSystem<IGunSystem>().gunInfo;
        
        var gunConfigModel = this.GetModle<IGunConfigModel>().GetGunConfigItemByName(gunInfo.Name.Value);

        var needBullet = gunConfigModel.BulletMaxCount - gunInfo.BulletCountInGun.Value;

        if (needBullet > 0)
        {
            gunInfo.State.Value = GunState.Reload;

            this.GetSystem<ITimeSystem>().AddDelayTask(gunConfigModel.ReloadSeconds, () =>
            {
                if (gunInfo.BulletCountOutGun.Value > needBullet)
                {
                    gunInfo.BulletCountInGun.Value += needBullet;
                    gunInfo.BulletCountOutGun.Value -= needBullet;
                }
                else
                {
                    gunInfo.BulletCountInGun.Value += gunInfo.BulletCountOutGun.Value;
                    gunInfo.BulletCountOutGun.Value = 0;
                }
                
                gunInfo.State.Value = GunState.Idle;
            });
            
        }
    }
}
