using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using FrameWorkDesign;
using FrameWorkDesign.Rule;
using Model;
using UnityEngine;

public class UIController : MonoBehaviour, IController
{
    private IStatSystem _statSystem;

    private IPlayerModel _playerModel;

    private IGunSystem _gunSystem;

    private int _maxBulletCount;
    
    private void Awake()
    {
        _statSystem = this.GetSystem<IStatSystem>();
        _playerModel = this.GetModle<IPlayerModel>();
        _gunSystem = this.GetSystem<IGunSystem>();

        _maxBulletCount = this.SendQuery(new MaxBulletCountQuery(_gunSystem.gunInfo.Name.Value));

        this.RegisterEvent<OnCurrentGunChanged>(e =>
        {
            _maxBulletCount = this.SendQuery(new MaxBulletCountQuery(e.Name));
        }).UnRegisterWhenGameObjectDestroy(this.gameObject);
    }

    private readonly Lazy<GUIStyle> _labelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
    {
        fontSize = 40
    });

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 100), $"生命: {_playerModel.HP.Value}/3", _labelStyle.Value);
        GUI.Label(new Rect(10, 50, 300, 100), $"子弹: {_gunSystem.gunInfo.BulletCountInGun.Value}/{_maxBulletCount}", _labelStyle.Value);
        GUI.Label(new Rect(10, 90, 300, 100), $"枪名: {_gunSystem.gunInfo.Name.Value}", _labelStyle.Value);
        GUI.Label(new Rect(10, 130, 300, 100), $"状态: {_gunSystem.gunInfo.State.Value}", _labelStyle.Value);
        GUI.Label(new Rect(10, 170, 300, 100), $"枪外子弹: {_gunSystem.gunInfo.BulletCountOutGun.Value}", _labelStyle.Value);
        GUI.Label(new Rect(Screen.width - 10 - 300, 10, 300, 100), $"杀敌数量: {_statSystem.KillCount.Value}", _labelStyle.Value);
    }

    private void OnDestroy()
    {
        _statSystem = null;
        _playerModel = null;
        _gunSystem = null;
    }

    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return ShootingEditor2D.Instance;
    }
}
