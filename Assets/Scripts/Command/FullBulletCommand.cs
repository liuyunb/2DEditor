using System.Collections;
using System.Collections.Generic;
using FrameWorkDesign;
using UnityEngine;

public class FullBulletCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var gunSystem = this.GetSystem<IGunSystem>();
        var gunConfigModel = this.GetModle<IGunConfigModel>();

        gunSystem.gunInfo.BulletCountInGun.Value = gunConfigModel.GetGunConfigItemByName(gunSystem.gunInfo.Name.Value).BulletMaxCount;

        foreach (var gunInfo in gunSystem.GunInfos)
        {
            gunInfo.BulletCountInGun.Value = gunConfigModel.GetGunConfigItemByName(gunInfo.Name.Value).BulletMaxCount;
        }
    }
}
