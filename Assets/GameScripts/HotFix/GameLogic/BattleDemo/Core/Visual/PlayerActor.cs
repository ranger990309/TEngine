using TEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 玩家演员。
    /// <remarks>PlayerEntity的表现层。</remarks>
    /// </summary>
    public class PlayerActor : EntityVisual
    {
        public DisplayComponent Display { private set; get; }
        
        public override void OnAttach()
        {
            Display = Attach<DisplayComponent>();
            OnEnterMap();
            base.OnAttach();
        }

        private void OnEnterMap()
        {
            DisplayInfo displayInfo = MemoryPool.Acquire<DisplayInfo>();
            displayInfo.Location = "boss_invader_model";
            Display.SetDisplay(displayInfo).Forget();
        }
    }
}