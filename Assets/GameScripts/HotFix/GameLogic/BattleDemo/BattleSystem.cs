using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 战斗控制系统。
    /// </summary>
    [Update][FixedUpdate][LateUpdate]
    public class BattleSystem:BehaviourSingleton<BattleSystem>
    {
        public override void Active()
        {
            GameModule.UI.ShowUIAsync<BattleMainUI>();
            base.Active();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}