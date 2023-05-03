using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using FrameWorkDesign;
using Unity.VisualScripting;
using UnityEngine;

public class PickGunItem : ShootingEditor2DController
{
    public string gunName;
    public int bulletInGun;
    public int bulletOutGun;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.SendCommand(new PickUpCommand(gunName, bulletInGun, bulletOutGun));
            Destroy(this.gameObject);
        }
    }
}
