using UnityEngine;

namespace GameLogic.BattleDemo
{
    public static class AnimatorParamDefine
    {
        public static int Moving = Animator.StringToHash("Moving");
        public static int SkillIndex = Animator.StringToHash("SkillIndex");
        public static int ImpactId = Animator.StringToHash("ImpactId");
        public static int Death = Animator.StringToHash("Death");
        public static int MoveSpeedScale = Animator.StringToHash("MoveSpeed");
        public static int SkillSpeedScale = Animator.StringToHash("SkillSpeed");
        public static int Show = Animator.StringToHash("Show");
        public static int Appear = Animator.StringToHash("Appear");
        public static int Plot = Animator.StringToHash("Plot");
        public static int interact = Animator.StringToHash("interact");
        public static int interact2 = Animator.StringToHash("interact2");
        public static int Win = Animator.StringToHash("Win");
    }   
}