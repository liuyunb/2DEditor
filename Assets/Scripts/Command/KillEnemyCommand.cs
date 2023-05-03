using System;
using FrameWorkDesign;
using Random = UnityEngine.Random;

namespace Command
{
    public class KillEnemyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IStatSystem>().KillCount.Value++;

            var rand = Random.Range(0, 100);
            if (rand > 20)
            {
                this.GetSystem<IGunSystem>().gunInfo.BulletCountInGun.Value += Random.Range(1, 4);
            }
        }
    }
}