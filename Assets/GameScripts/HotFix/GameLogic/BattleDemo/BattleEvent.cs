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
        /// 停止移动。
        /// </summary>
        public static readonly int BreakTouchMove = StringId.StringToHash("BattleEvent.BreakTouchMove");
    }
}