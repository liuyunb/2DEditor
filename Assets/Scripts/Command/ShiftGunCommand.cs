using System.Collections;
using System.Collections.Generic;
using FrameWorkDesign;
using UnityEngine;

public class ShiftGunCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IGunSystem>().ShiftGun();
    }
}
