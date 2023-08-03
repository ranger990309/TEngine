using TEngine;

namespace GameLogic.BattleDemo
{
    public partial class ActorEventType
    {
        public static int ModelVisibleChange = StringId.StringToHash("ActorEventType.ModelVisibleChange");
        public static int CtrlMoveToPos = StringId.StringToHash("ActorEventType.CtrlMoveToPos");
        public static int ActorCtrlMoveDir = StringId.StringToHash("ActorEventType.ActorCtrlMoveDir");
        public static int ActorCtrlFlashDir = StringId.StringToHash("ActorEventType.ActorCtrlFlashDir");
        
        public static int ActorHpChg = StringId.StringToHash("ActorEventType.ActorHpChg");
        public static int ActorMpChg = StringId.StringToHash("ActorEventType.ActorMpChg");
        public static int ActorMoveSpeedChg = StringId.StringToHash("ActorEventType.ActorMoveSpeedChg");
        public static int ActorAttackSpeedChg = StringId.StringToHash("ActorEventType.ActorAttackSpeedChg");
        public static int ActorMaxHpChg = StringId.StringToHash("ActorEventType.ActorMaxHpChg");
        
        public static int SkillImpacted = StringId.StringToHash("ActorEventType.SkillImpacted");
        public static int ProcessDamage = StringId.StringToHash("ActorEventType.ProcessDamage");
        public static int ActorBuffAdd = StringId.StringToHash("ActorEventType.ActorBuffAdd");
        public static int ActorBuffRmv = StringId.StringToHash("ActorEventType.ActorBuffRmv");
        public static int SendBuffStateChanged = StringId.StringToHash("ActorEventType.SendBuffStateChanged");
        
        public static readonly int OnActorStatesChanged = StringId.StringToHash("ActorEventType.OnActorStatesChanged");
        public static readonly int ActorAttackSpeedChanged = StringId.StringToHash("ActorEventType.ActorAttackSpeedChanged");
        
        public static readonly int OnLeaveCurrentBattle = StringId.StringToHash("ActorEventType.OnLeaveCurrentBattle");
        
        #region 模型相关
        public static readonly int ModelShow= StringId.StringToHash("ActorEventType.ModelShow");
        public static readonly int ModelBeforeDestroy= StringId.StringToHash("ActorEventType.ModelBeforeDestroy");
        public static readonly int ModelDestroy= StringId.StringToHash("ActorEventType.ModelDestroy");
        #endregion

        #region 动画相关
        /// <summary>
        /// 受击动画播放结束
        /// </summary>
        public static readonly int AnimImpactEnd = StringId.StringToHash("ActorEventType.AnimImpactEnd");
        /// <summary>
        /// 动画run播放事件触发
        /// </summary>
        public static readonly int AnimRun= StringId.StringToHash("ActorEventType.AnimImpactEnd");

        #endregion
    }
    
    public partial class ActorStateEvent
    {
        public static int Actor_Arrived = StringId.StringToHash("ActorStateEvent.Actor_Arrived");
        public static int Actor_Die = StringId.StringToHash("ActorStateEvent.Actor_Die");
        public static int Actor_Enter_Stun = StringId.StringToHash("ActorStateEvent.Actor_Enter_Stun");
        public static int Actor_Finish_Stun = StringId.StringToHash("ActorStateEvent.Actor_Finish_Stun");
        
        public static int Actor_Skill_Cast = StringId.StringToHash("ActorStateEvent.Actor_Skill_Cast");
        public static int Actor_Finish_Cast = StringId.StringToHash("ActorStateEvent.Actor_Finish_Cast");
        
        
        public static int SkillAniPlayStart = StringId.StringToHash("ActorStateEvent.SkillAniPlayStart");
        public static int SkillAniPlayEnd = StringId.StringToHash("ActorStateEvent.SkillAniPlayEnd");
    }
    
    public partial class EntityVisualEvent
    {
        public static int ACTOR_BUFF_TIME_CHANGE = StringId.StringToHash("EntityVisualEvent.ACTOR_BUFF_TIME_CHANGE");
        public static int ACTOR_BUFF_STATE_CHANGE = StringId.StringToHash("EntityVisualEvent.ACTOR_BUFF_STATE_CHANGE");
        
        public static int BASE_TRANSFROM_INIT_POS = StringId.StringToHash("EntityVisualEvent.BASE_TRANSFROM_INIT_POS");
        public static int BASE_TRANSFROM_CHANGE = StringId.StringToHash("EntityVisualEvent.BASE_TRANSFROM_CHANGE");
    }
}