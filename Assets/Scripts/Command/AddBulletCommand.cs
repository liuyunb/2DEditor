using System.Collections;
using System.Collections.Generic;
using FrameWorkDesign;
using UnityEngine;

public class AddBulletCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var gunSystem = this.GetSystem<IGunSystem>();
        var gunConfigModel = this.GetModle<IGunConfigModel>();

        var curGunInfo = gunSystem.gunInfo;
        AddBullet(curGunInfo, gunConfigModel);

        foreach (var gunInfo in gunSystem.GunInfos)
        {
            AddBullet(gunInfo, gunConfigModel);
        }
        
    }

    public void AddBullet(GunInfo gunInfo, IGunConfigModel gunConfigModel)
    {
        var gunItemModel = gunConfigModel.GetGunConfigItemByName(gunInfo.Name.Value);
        var maxBullet = gunItemModel.BulletMaxCount;

        if (gunItemModel.NeedBullet)
        {
            gunInfo.BulletCountOutGun.Value += maxBullet;
        }
        

    }
}
