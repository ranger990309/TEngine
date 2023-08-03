using System;
using Cysharp.Threading.Tasks;
using GameLogic.BattleDemo;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 战斗控制系统。
    /// </summary>
    public class BattleSystem:Architecture<BattleSystem>
    {
        public static CameraSystem CameraSystem { private set; get; } = new CameraSystem();
        
        public static InPutSystem InPutSystem { private set; get; } = new InPutSystem();
        
        public static LevelSystem LevelSystem{ private set; get; } = new LevelSystem();
        
        public static ActorSystem ActorSystem{ private set; get; } = new ActorSystem();
        
        protected override void Init()
        {
            Log.Debug("Architecture BattleSystem OnInit");
            RegisterSubSystem();
            OnRegisterEvent();
            GameEvent.Send(BattleEvent.OnShowBattleMainUI);
            OnStartBattle().Forget();
        }

        private void RegisterSubSystem()
        {
            RegisterSystem(CameraSystem);
            RegisterSystem(InPutSystem);
            RegisterSystem(LevelSystem);
            RegisterSystem(ActorSystem);
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

        /// <summary>
        /// 示例示范开始战斗。
        /// <remarks>单机由客户端触发，联机由服务器协议触发和参数填充。</remarks>
        /// </summary>
        private async UniTaskVoid OnStartBattle()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            EntityCreateData createData = new EntityCreateData();
            createData.actorEntityType = ActorEntityType.Player;
            createData.SetBornPos(bornPos: Vector3.zero, forward: Vector3.forward);
            // 逻辑层先创建实体。
            EntityLogicMgr.CreateEntityLogic(createData, isStartActor: true);
            LevelSystem.OnStartBattle().Forget();
        }

        private void OnEndBattle()
        {
            
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
    }
}