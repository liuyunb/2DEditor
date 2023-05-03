using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FrameWorkDesign;
using UnityEngine;

public interface IGunSystem : ISystem
{
    GunInfo gunInfo { get; }
    
    Queue<GunInfo> GunInfos { get; }

    void PickGun(string name, int bulletInGun, int bulletOutGun);
    void ShiftGun();
}

public class OnCurrentGunChanged
{
    public string Name { get; set; }
}

public class GunSystem : AbstractSystem, IGunSystem
{
    protected override void OnInit()
    {
        
    }

    public GunInfo gunInfo { get; } = new GunInfo()
    {
        BulletCountInGun = new BindableProperty<int>()
        {
            Value = 3//初始弹药数量
        },
        Name = new BindableProperty<string>()
        {
            Value = "手枪"
        },
        State = new BindableProperty<GunState>()
        {
            Value = GunState.Idle
        },
        BulletCountOutGun = new BindableProperty<int>()
        {
            Value = 1
        }
    };

    public Queue<GunInfo> GunInfos => _gunInfos;

    private readonly Queue<GunInfo> _gunInfos = new Queue<GunInfo>();

    public void PickGun(string name, int bulletInGun, int bulletOutGun)
    {
        if (gunInfo.Name.Value == name)//当前枪和所捡枪相同
        {
            gunInfo.BulletCountOutGun.Value += bulletInGun;
            gunInfo.BulletCountOutGun.Value += bulletOutGun;
        }
        else if(_gunInfos.Any(gunInfo=>gunInfo.Name.Value == name))//背包里拥有同一把枪
        {
            var tempGun = _gunInfos.First(_gunInfos => _gunInfos.Name.Value == name);
            tempGun.BulletCountOutGun.Value += bulletInGun;
            tempGun.BulletCountOutGun.Value += bulletOutGun;
        }
        else//捡的是新枪
        {
            EnqueueCurrentGun(name, bulletInGun, bulletOutGun);
        }
    }

    public void ShiftGun()
    {
        if(_gunInfos.Count == 0)
            return;
        
        var nextGun = _gunInfos.Dequeue();
        
        EnqueueCurrentGun(nextGun.Name.Value, nextGun.BulletCountInGun.Value, nextGun.BulletCountOutGun.Value);
    }


    void EnqueueCurrentGun(string nextGunName, int bulletInGun, int bulletOutGun)
    {
        var tempGun = new GunInfo()
        {
            Name = new BindableProperty<string>()
            {
                Value = gunInfo.Name.Value
            },
            BulletCountInGun = new BindableProperty<int>()
            {
                Value = gunInfo.BulletCountInGun.Value
            },
            BulletCountOutGun = new BindableProperty<int>()
            {
                Value = gunInfo.BulletCountOutGun.Value
            },
            State = new BindableProperty<GunState>()
            {
                Value = GunState.Idle
            }
        };
            
        _gunInfos.Enqueue(tempGun);

        gunInfo.Name.Value = nextGunName;
        gunInfo.BulletCountInGun.Value = bulletInGun;
        gunInfo.BulletCountOutGun.Value = bulletOutGun;
        gunInfo.State.Value = GunState.Idle;
            
        this.SendEvent(new OnCurrentGunChanged()
        {
            Name = nextGunName
        });
    }
}
