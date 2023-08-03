using GameConfig.Battle;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 定义对象的数值属性字段
    /// </summary>
    public class ActorAttrData
    {
        #region 属性值

        public float MaxHp;
        public float Attack;
        public float PhyDamage; //物理伤害（保留）
        public float PhyDef; //物理防御（保留）
        public float MagicDamage; //法术伤害（保留）
        public float MagicDef; //法术防御（保留）

        /// <summary>
        /// 移动速度
        /// </summary>
        public float MoveSpeed;

        /// <summary>
        /// 攻击速度
        /// </summary>
        public float AttackSpeed;

        /// <summary>
        /// 命中率
        /// </summary>
        public float Hit;

        /// <summary>
        /// 闪避率
        /// </summary>
        public float Dodge;

        /// <summary>
        /// 暴击率
        /// </summary>
        public float CriticalAtkRatio;

        /// <summary>
        /// San值（疯狂值）
        /// </summary>
        public float SanValue;

        /// <summary>
        /// 最大Mp
        /// </summary>
        public float MaxMp;

        /// <summary>
        /// 物理伤害倍率
        /// </summary>
        public float PhyDamageRatio;
        
        /// <summary>
        /// 魔法伤害倍率
        /// </summary>
        public float MagicDamageRatio;
        
        /// <summary>
        /// 暴击倍率
        /// </summary>
        public float CriticalRatio;
        
        /// <summary>
        /// 暴击减免
        /// </summary>
        public float CriticalReduce;

        #endregion
        
        internal void CalImpact(ActorAttrImpactData impact)
        {
            var dataType = impact.m_dataType;
            switch (dataType)
            {
                case ActorAttrDataType.MaxHp:
                    MaxHp = impact.CalAddVal(MaxHp);
                    break;
                case ActorAttrDataType.Attack:
                    Attack = impact.CalAddVal(Attack);
                    break;
                case ActorAttrDataType.PhyDamage:
                    PhyDamage = impact.CalAddVal(PhyDamage);
                    break;
                case ActorAttrDataType.PhyDef:
                    PhyDef = impact.CalAddVal(PhyDef);
                    break;
                case ActorAttrDataType.MagicDamage:
                    MagicDamage = impact.CalAddVal(MagicDamage);
                    break;
                case ActorAttrDataType.MagicDef:
                    MagicDef = impact.CalAddVal(MagicDef);
                    break;
                case ActorAttrDataType.MoveSpeed:
                    MoveSpeed = impact.CalAddVal(MoveSpeed);
                    break;
                case ActorAttrDataType.AttackSpeed:
                    AttackSpeed = impact.CalAddVal(AttackSpeed);
                    break;
                case ActorAttrDataType.Hit:
                    Hit = impact.CalAddVal(Hit);
                    break;
                case ActorAttrDataType.Dodge:
                    Dodge = impact.CalAddVal(Dodge);
                    break;
                case ActorAttrDataType.CriticalAtkRatio:
                    CriticalAtkRatio = impact.CalAddVal(CriticalAtkRatio);
                    break;
                case ActorAttrDataType.SanValue:
                    SanValue = impact.CalAddVal(SanValue);
                    break;
                case ActorAttrDataType.MaxMp:
                    MaxMp = impact.CalAddVal(MaxMp);
                    break;
                case ActorAttrDataType.PhyDamageRatio:
                    PhyDamageRatio = impact.CalAddVal(PhyDamageRatio);
                    break;
                case ActorAttrDataType.MagicDamageRatio:
                    MagicDamageRatio = impact.CalAddVal(MagicDamageRatio);
                    break;
                case ActorAttrDataType.CriticalRatio:
                    CriticalRatio = impact.CalAddVal(CriticalRatio);
                    break;
                case ActorAttrDataType.CriticalReduce:
                    CriticalReduce = impact.CalAddVal(CriticalReduce);
                    break;
            }
        }

        public void Set(ActorAttrData src)
        {
            MaxHp = src.MaxHp;
            Attack = src.Attack;
            PhyDamage = src.PhyDamage;
            PhyDef = src.PhyDef;
            MagicDamage = src.MagicDamage;
            MagicDef = src.MagicDef;
            MoveSpeed = src.MoveSpeed;
            AttackSpeed = src.AttackSpeed;
            Hit = src.Hit;
            Dodge = src.Dodge;
            CriticalAtkRatio = src.CriticalAtkRatio;
            SanValue = src.SanValue;
            MaxMp = src.MaxMp;
            PhyDamageRatio = src.PhyDamageRatio;
            MagicDamageRatio = src.MagicDamageRatio;
            CriticalRatio = src.CriticalRatio;
            CriticalReduce = src.CriticalReduce;
        }

        internal void AddAttrElem(uint attrID, float val, ActorAttrAddType type = ActorAttrAddType.ABSOLUTE_VAL)
        {
            var impactData = new ActorAttrImpactData();
            impactData.UpdateData((ActorAttrDataType)attrID, type, val);
            CalImpact(impactData);
        }
    }
}