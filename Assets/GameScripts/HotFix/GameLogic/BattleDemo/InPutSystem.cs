using TEngine;

namespace GameLogic
{
    public class InPutSystem: SubSystem
    {
        public IUICtrlMove MoveCtrlUI;
        
        protected override void OnInit()
        {
            
        }
        
        public void OnUIManualStop()
        {
            StopManualMove();
        }
        
        private void StopManualMove()
        {
            // //if (m_ctrlActor != null)
            // //{
            // //    ActorEventHelper.SendCtrlStopMove(m_ctrlActor);
            // //}
            // BattleCoreSys.Instance.PlayerAutoBattle(true);
            // BattleCoreSys.Instance.SetRuntimeInput(false, null, 0);
            // GameEvent.Get<IBattleLogic>().StopMove();
        }
    }
}