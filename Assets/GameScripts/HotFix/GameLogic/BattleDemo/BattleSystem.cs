using GameLogic.BattleDemo;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 战斗控制系统。
    /// </summary>
    public class BattleSystem:Architecture<BattleSystem>
    {
        public static CameraSystem CameraSystem { private set; get; } = new CameraSystem();
        
        public static InPutSystem InPutSystem { private set; get; } = new InPutSystem();
        
        protected override void Init()
        {
            Log.Debug("Architecture BattleSystem OnInit");
            RegisterSubSystem();
            OnRegisterEvent();
            GameEvent.Send(BattleEvent.OnShowBattleMainUI);
        }

        private void RegisterSubSystem()
        {
            RegisterSystem(CameraSystem);
            RegisterSystem(InPutSystem);
        }

        private void OnRegisterEvent()
        {
            GameEvent.AddEventListener(BattleEvent.OnShowBattleMainUI,OnShowBattleMainUI);
            GameEvent.AddEventListener(BattleEvent.OnCloseBattleMainUI,OnCloseBattleMainUI);
        }

        #region 战斗事件回调

        private void OnShowBattleMainUI()
        {
            GameModule.UI.ShowUIAsync<BattleMainUI>();
        }

        private void OnCloseBattleMainUI()
        {
            GameModule.UI.CloseWindow<BattleMainUI>();
        }
        #endregion
        
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
        }
        
        public EntityLogic GetCurrCtrlEntity()
        {
            return null;
        }
    }
}