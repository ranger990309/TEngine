using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 战斗系统事件定义。
    /// </summary>
    public static class BattleEvent
    {
        /// <summary>
        /// 显示战斗MainUI。
        /// </summary>
        public static readonly int OnShowBattleMainUI = StringId.StringToHash("BattleEvent.OnShowBattleMainUI");
        
        /// <summary>
        /// 显示战斗MainUI。
        /// </summary>
        public static readonly int OnCloseBattleMainUI = StringId.StringToHash("BattleEvent.OnCloseBattleMainUI");
        
        /// <summary>
        /// 开始触碰移动。
        /// </summary>
        public static readonly int StartTouchMove = StringId.StringToHash("BattleEvent.StartTouchMove");
        
        /// <summary>
        /// 停止触碰移动。
        /// </summary>
        public static readonly int BreakTouchMove = StringId.StringToHash("BattleEvent.BreakTouchMove");
        
        /// <summary>
        /// Actor事件开始移动逻辑。
        /// </summary>
        public static readonly int StartMove = StringId.StringToHash("BattleEvent.StartMove");
        
        /// <summary>
        /// Actor事件停止移动逻辑。
        /// </summary>
        public static readonly int StopMove = StringId.StringToHash("BattleEvent.StopMove");
        
        /// <summary>
        /// 输入层输入执行技能。
        /// </summary>
        public static readonly int InputSkill = StringId.StringToHash("BattleEvent.InputSkill");
        
        /// <summary>
        /// Actor事件执行技能。
        /// <remarks>普攻也可以算作技能。</remarks>
        /// </summary>
        public static readonly int DoSkill = StringId.StringToHash("BattleEvent.DoSkill");
    }
}