using System.Collections;
using System.Collections.Generic;
using Architecture;
using FrameWorkDesign;
using UnityEngine;

public class ShootingEditor2DController : MonoBehaviour, IController
{
    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return ShootingEditor2D.Instance;
    }
}
