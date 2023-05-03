using System;
using System.Collections;
using System.Collections.Generic;
using FrameWorkDesign;
using UnityEngine;

public enum GunState
{
    Idle,
    Shooting,
    Reload,
    EmptyBullet,
    CoolDown
}

public class GunInfo
{
    [Obsolete("请使用BulletCountInGun", true)]
    public BindableProperty<int> BulletCount
    {
        get => BulletCountInGun;
        set => BulletCountInGun = value;
    }

    public BindableProperty<int> BulletCountInGun;//记录枪内的子弹

    public BindableProperty<string> Name;//记录枪的名字

    public BindableProperty<GunState> State;//记录枪的状态

    public BindableProperty<int> BulletCountOutGun;//记录已打完的子弹
}
