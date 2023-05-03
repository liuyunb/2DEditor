using System.Collections;
using System.Collections.Generic;
using Architecture;
using FrameWorkDesign;
using UnityEngine;

public class MaxBulletCountQuery : AbstractQuery<int>
{
    private readonly string _gunName;

    public MaxBulletCountQuery(string gunName)
    {
        _gunName = gunName;
    }

    protected override int OnDo()
    {
        var gunConfigModel = this.GetModle<IGunConfigModel>();
        var gunConfig =  gunConfigModel.GetGunConfigItemByName(_gunName);
        return gunConfig.BulletMaxCount;
    }
}
