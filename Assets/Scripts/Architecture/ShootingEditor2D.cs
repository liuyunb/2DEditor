using System;
using FrameWorkDesign;
using Model;

namespace Architecture
{
    public class ShootingEditor2D : Architecture<ShootingEditor2D>
    {
        protected override void Init()
        {
            RegisterSystem<ITimeSystem>(new TimeSystem());
            RegisterSystem<IStatSystem>(new StatSystem());
            RegisterSystem<IGunSystem>(new GunSystem());
            
            RegisterModle<IGunConfigModel>(new GunConfigModel());
            RegisterModle<IPlayerModel>(new PlayerModel());
        }
    }
}