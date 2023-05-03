using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using Command;
using FrameWorkDesign;
using FrameWorkDesign.Rule;
using UnityEngine;

public class Gun : ShootingEditor2DController
{
    private Bullet _bullet;
    private GunInfo _curGun;

    private int _maxBulletCount;
    private void Awake()
    {
        _bullet = transform.Find("Bullet").GetComponent<Bullet>();
        _curGun = this.GetSystem<IGunSystem>().gunInfo;

        _maxBulletCount = this.SendQuery(new MaxBulletCountQuery(_curGun.Name.Value));
        
        this.RegisterEvent<OnCurrentGunChanged>(e =>
        {
            _maxBulletCount = this.SendQuery(new MaxBulletCountQuery(e.Name));
        }).UnRegisterWhenGameObjectDestroy(this.gameObject);
    }

    public void Shoot()
    {
        if (_curGun.BulletCountInGun.Value > 0 && _curGun.State.Value == GunState.Idle)
        {
            var newBullet = Instantiate(_bullet, _bullet.transform.position, _bullet.transform.rotation);
            newBullet.transform.localScale = _bullet.transform.lossyScale;
            newBullet.gameObject.SetActive(true);
            
            this.SendCommand(ShootCommand.Single);
        }
    }

    public void Reload()
    {
        if (_curGun.BulletCountInGun.Value < _maxBulletCount && _curGun.BulletCountOutGun.Value > 0 &&
            _curGun.State.Value == GunState.Idle)
        {
            this.SendCommand<ReloadCommand>();
        }
    }
}
