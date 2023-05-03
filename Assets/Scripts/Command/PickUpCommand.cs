using System.Collections;
using System.Collections.Generic;
using FrameWorkDesign;
using UnityEngine;

public class PickUpCommand : AbstractCommand
{
    private readonly string _name;
    private readonly int _bulletInGun;
    private readonly int _bulletOutGun;


    public PickUpCommand(string name, int bulletInGun, int bulletOutGun)
    {
        _name = name;
        _bulletInGun = bulletInGun;
        _bulletOutGun = bulletOutGun;
    }

    protected override void OnExecute()
    {
        this.GetSystem<IGunSystem>().PickGun(_name, _bulletInGun, _bulletOutGun);
    }
}
