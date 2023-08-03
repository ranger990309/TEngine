using System;
using GameBase;
using GameConfig.Battle;
using UnityEngine;
using TEngine;
using Random = UnityEngine.Random;

namespace GameLogic.BattleDemo
{
    public class SkillDamageHelper:Singleton<SkillDamageHelper>
    {
        private static ActorAttrData tmpSourceAttrData = new ActorAttrData();
        private DamageInfo m_damageInfo = new DamageInfo();
        private HitInfo m_hitInfo = new HitInfo();
        
        public virtual void DoDamage(EntityLogic caster,EntityLogic target,SkillAttrDamageData damageAttr)
        {
            if (target == null)
            {
                Log.Debug("目标不存在");
                return;
            }
            
            m_damageInfo.Reset();

            if (target.ActorData.Hp < 0)
            {
                return;
            }

            var hitInfo = m_hitInfo;
            var dmgInfo = m_damageInfo;

            m_hitInfo.sourceActor = caster;
            m_hitInfo.targetActor = target;

            CalWeaponDamage(dmgInfo,hitInfo,damageAttr);
            
            if (dmgInfo != null)
            {
                ActorEventHelper.SendSkillImpacted(target, caster, dmgInfo);
            }
        }

        private void CalWeaponDamage(DamageInfo damageInfo,HitInfo hitInfo,SkillAttrDamageData damageAttr)
        {
            damageInfo.Reset();
            
            SkillMagicType magicType = damageAttr.MagicType;
            
            var targetActor = hitInfo.targetActor;
            var sourceActor = hitInfo.sourceActor;
            
            if (sourceActor == null || targetActor == null)
            {
                return;
            }
            
            var sourceAttrData = tmpSourceAttrData;
            var targetAttrData = targetActor.ActorData.AttrData;
            sourceAttrData.Set(sourceActor.ActorData.AttrData);
            
            if (!CheckHit(sourceAttrData, targetAttrData))
            {
                damageInfo.isMiss = true;
                return;
            }
            
            bool isCritical = CheckCritical(sourceAttrData, targetAttrData);
            damageInfo.isCrit = isCritical;
            
            var normalDamage = CalcNormalDamage(sourceAttrData, targetAttrData,damageAttr);
            
            if (isCritical)
            {
                damageInfo.damage = CalcCriticalDamage(normalDamage, sourceAttrData);

                Log.Info("使用会心伤害公式，会心伤害:{0}", damageInfo.damage);
            }
            else
            {
                //普通伤害
                damageInfo.damage = normalDamage;

                Log.Info("使用普通伤害公式，普通伤害:{0}", damageInfo.damage);
            }
        }

        #region 计算命中与伤害
        private bool CheckHit(ActorAttrData sourceAttrData, ActorAttrData targetAttrData)
        {
            float totalHitRatio = 0f;
            var hitFixRatio = 0.5f;
 
            var hitRatioExchange = (sourceAttrData.Hit + hitFixRatio) /
                                   (targetAttrData.Dodge + sourceAttrData.Hit + hitFixRatio);
            
            totalHitRatio = hitRatioExchange + sourceAttrData.Hit - targetAttrData.Dodge;

            Log.Info($"命中率:{totalHitRatio}");
            
            return CheckRate(totalHitRatio);
        }
        
        private bool CheckCritical(ActorAttrData sourceAttrData, ActorAttrData targetAttrData)
        {
            var criticalFixRatio = 0f;
   
            float totalCriticalRatio = criticalFixRatio + sourceAttrData.CriticalAtkRatio;

            Log.Info("会心概率:{0}", totalCriticalRatio);

            return CheckRate(totalCriticalRatio);
        }

        private float CalcNormalDamage(ActorAttrData sourceAttrData, ActorAttrData targetAttrData, SkillAttrDamageData damageAttr)
        {
            float normalDamage = 0f;
            float attackRatio = 1f;
            
            if (damageAttr.MagicType == SkillMagicType.SKILL_TYPE_DMG_PHY)
            {
                var baseDamage = CalcPhyBaseDamage(damageAttr, sourceAttrData, targetAttrData);

                normalDamage = baseDamage * attackRatio;
            }
            else
            {
                var baseDamage = CalcMagicBaseDamage(damageAttr, sourceAttrData, targetAttrData);
                
                normalDamage = baseDamage * attackRatio;
            }
            
            return normalDamage;
        }

        private float CalcPhyBaseDamage(SkillAttrDamageData damageAttr, ActorAttrData sourceAttrData, ActorAttrData targetAttrData)
        {
            var atkSum = sourceAttrData.PhyDamage * (1 /*+ sourceAttrData.PhyAtkRate*/); //攻击方物理攻击总值
            var defSum = targetAttrData.PhyDef * (1 /*+ targetAttrData.PhyDefRate*/); //防守方物理防御总值
            
            var minDamageRatio = 0.1f;
            var minDamage = atkSum * minDamageRatio; //保底伤害

            var calcDamage = (atkSum - defSum) * damageAttr.Param1 + damageAttr.Param2;
            var baseDamage = Math.Max(minDamage, calcDamage);

            Log.Info("[计算物理基础伤害] 基础伤害总值:{0}。 物理攻击总值:{1}，保底系数:{2}，攻击方物理攻击总值:{3}，防御方物理防御总值:{4}， ",
                baseDamage, atkSum, minDamageRatio, atkSum, defSum);

            return baseDamage;
        }
        
        private float CalcMagicBaseDamage(SkillAttrDamageData damageAttr, ActorAttrData sourceAttrData, ActorAttrData targetAttrData)
        {
            var atkSum = sourceAttrData.MagicDamage * (1 /*+ sourceAttrData.MgcAtkRate*/); //攻击方魔法攻击总值
            var defSum = targetAttrData.MagicDef * (1 /*+ targetAttrData.MagicDefRate*/); //防守方魔法防御总值

            var minDamageRatio = 0.1f;;
            var minDamage = atkSum * minDamageRatio; //保底伤害

            var calcDamage = (atkSum - defSum) * damageAttr.Param1 + damageAttr.Param2;
            var baseDamage = Math.Max(minDamage, calcDamage);

            Log.Info("[计算法术基础伤害] 基础伤害总值:{0}。 魔法攻击总值:{1}，保底系数:{2}，攻击方魔法攻击总值:{3}，防御方魔法防御总值:{4}",
                baseDamage, atkSum, minDamageRatio, atkSum, defSum);

            return baseDamage;
        }
        
        private static float CalcCriticalDamage(float normalDamage, ActorAttrData sourceAttrData)
        {
            var huiXinDmg = normalDamage * Mathf.Clamp(sourceAttrData.CriticalRatio,2f,9999f);
            return huiXinDmg;
        }


        private int m_checkCount = 0;
        
        /// <summary>
        /// 判断概率。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool CheckRate(float val)
        {
            var currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()+ + m_checkCount;
            m_checkCount++;
            UnityEngine.Random.InitState((int)currentTime);
            var condition = Random.Range(0f, 1f);
            if (condition < val)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}