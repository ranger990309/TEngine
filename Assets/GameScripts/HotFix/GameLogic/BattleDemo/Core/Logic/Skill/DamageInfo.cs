using GameConfig.Battle;
using UnityEngine;

namespace GameLogic.BattleDemo
{
    public enum SkillHitDamageType
    {
        /// <summary>
        /// 子弹类型
        /// </summary>
        Bullet,

        /// <summary>
        /// 碰撞类型
        /// </summary>
        BodyCollider,

        /// <summary>
        /// 机关伤害类型
        /// </summary>
        Machine,
    }

    /// <summary>
    /// 伤害来源类型
    /// </summary>
    public enum DamageSourceType
    {
        /// <summary>
        /// 无
        /// </summary>
        SourceNone,
        /// <summary>
        /// 子弹
        /// </summary>
        SourceBullet,
        /// <summary>
        /// 激光
        /// </summary>
        SourceLightChain,
        /// <summary>
        /// 伤害点
        /// </summary>
        SourceShootPoint,
        /// <summary>
        /// Buffer
        /// </summary>
        SourceBuffer,
    }
    
    /// <summary>
    /// 伤害数据
    /// </summary>
    public class DamageInfo
    {
        /// <summary>
        /// 伤害值，负数表示是加血
        /// </summary>
        public float damage;

        /// <summary>
        /// 增加MP
        /// </summary>
        public float addMP;

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool isCrit;
        
        /// <summary>
        /// 是否miss
        /// </summary>
        public bool isMiss;
        
        /// <summary>
        /// 是否破防
        /// </summary>
        public bool isBreakDamage;

        /// <summary>
        /// 是否格挡
        /// </summary>
        public bool isBlockDamage;
        
        /// <summary>
        /// 是否导致死亡
        /// </summary>
        public bool isDead;
        
        /// <summary>
        /// 受击的方向
        /// </summary>
        public Vector3 impactDir;

        public DamageSourceType sourceType = DamageSourceType.SourceNone;

        public SkillMagicType magicType = SkillMagicType.SKILL_TYPE_NONE;
        
        public void Reset()
        {
            damage = 0;
            addMP = 0;
            isCrit = false;
            isMiss = false;
            isBreakDamage = false;
            isBlockDamage = false;
            isDead = false;
            magicType = SkillMagicType.SKILL_TYPE_NONE;
            sourceType = DamageSourceType.SourceNone;
        }
    }
}