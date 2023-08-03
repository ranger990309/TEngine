using System.Collections.Generic;
using GameConfig.Battle;
using TEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 技能释放组件。
    /// <remarks>技能施法器来根据技能ID，获取技能表现数据与技能逻辑数据，来创建技能。其下的元素与技能粒度的细分就不具体实现了。</remarks>
    /// </summary>
    public class SkillCasterComponent : EntityLogicComponent
    {
        private static uint _nextSkillGid = 1;
        
        /// <summary>
        /// 当前front播放的技能
        /// </summary>
        private SkillPlayData _currPlayData = null;

        /// <summary>
        /// 技能CD数据
        /// </summary>
        private Dictionary<uint, SkillCDInfo> _skillCDMap = new Dictionary<uint, SkillCDInfo>();

        public override void OnAttach()
        {
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            Owner.Event.AddEventListener<uint>(BattleEvent.DoSkill, DoSkill, Owner);
        }

        private void DoSkill(uint skillId)
        {
            PlaySkill(skillId);
        }

        /// <summary>
        /// 播放技能。
        /// </summary>
        /// <param name="skillId">技能Id。</param>
        /// <param name="target">目标。</param>
        /// <param name="checkCd">是否检测CD。</param>
        /// <param name="forceCaster">是否强制释放。</param>
        /// <returns>是否播放成功。</returns>
        internal void PlaySkill(uint skillId, EntityLogic target = null, bool forceCaster = false, bool checkCd = true)
        {
            Log.Assert(skillId > 0, $"ActorName: {Owner.GetActorName()}");
            Log.Debug("Start Play SKill[{0}]", skillId);

            var skillBaseConfig = ConfigLoader.Instance.Tables.TbSkill.Get((int)skillId);

            if (skillBaseConfig == null)
            {
                Log.Error("GetSkillBaseConfig Failed, invalid skillID: {0}", skillId);
                return;
            }

            if (Owner.ActorData.Mp < skillBaseConfig.CostMP)
            {
                Log.Warning("not enough mp {0}", skillId);
                return;
            }

            if (IsSkillInCD(skillId))
            {
                Log.Warning("skill is in cd {0}", skillId);
                return;
            }

            if (_currPlayData != null)
            {
                if (_currPlayData.skillId == skillId && _currPlayData.Status == SkillPlayStatus.PlayingAim)
                {
                    if (target != null)
                    {
                        // currPlaySkill.SetLockTarget(targetActor);
                        // currPlaySkill.SetDestPosition(targetActor.Position);
                    }

                    Log.Error("play same skill failed: {0}, curr is playing skillId: {0}", skillId, _currPlayData.skillId);
                }
                else if (forceCaster)
                {
                    // m_waitNextSkillPlayData.m_skillId = sourceSkillId;
                    // m_waitNextSkillPlayData.m_target = targetActor;
                }
                else
                {
                    Log.Error("play skill failed: {0}, curr is playing skillId: {0}", skillId, _currPlayData.skillId);
                }
                return;
            }
            
            ActorEventHelper.Send(Owner, ActorStateEvent.Actor_Skill_Cast);
            
            SkillPlayData playData = AllocPlayData((uint)skillId, skillBaseConfig);

            if (playData == null)
            {
                Log.Error("play skill error: {0}", skillId);
                return;
            }
            
            CostMp(skillBaseConfig);
            SetSkillCDInfo(skillBaseConfig);
            DoPlaySkill(playData, skillBaseConfig);
        }
        
        private void DoPlaySkill(SkillPlayData playData, SkillBaseConfig skillBaseConfig)
        {
            //构建Handle
            playData.startTime = GameTime.time;
            SetCurrPlayData(playData);
            
            playData.Status = SkillPlayStatus.PlayingFront;

            int skillIndex = 1;
            
            Owner.VisualEvent.SendEvent(Entity2VisualEvent.DoPlaySkill,skillIndex);

            if (true)
            {
                foreach (var attrDamageData in playData.skillBaseConfig.AttrDamageData)
                {
                    SkillDamageHelper.Instance.DoDamage(playData.CasterActor,playData.targetActor,attrDamageData);
                }
            }

            GameModule.Timer.AddTimer(args =>
            {
                Owner.VisualEvent.SendEvent(Entity2VisualEvent.DoPlaySkill,0);
            }, 0.3f, false);
        }
        
        private SkillPlayData AllocPlayData(uint skillId, SkillBaseConfig baseCfg)
        {

            SkillPlayData playData = MemoryPool.Acquire<SkillPlayData>();
            playData.skillGid= AllocNextSkillGid();
            playData.CasterActor = Owner;
            playData.skillId = skillId;
            playData.Status = SkillPlayStatus.PlayInit;
            playData.skillBaseConfig = baseCfg;
            return playData;
        }
        
        public static uint AllocNextSkillGid()
        {
            return _nextSkillGid++;
        }

        /// <summary>
        /// 技能施法器是否堵塞。
        /// </summary>
        /// <returns>是否堵塞。</returns>
        internal bool IsBusySkill()
        {
            return _currPlayData != null && _currPlayData.Status != SkillPlayStatus.PlayingToFree;
        }
        
        private void SetCurrPlayData(SkillPlayData playData)
        {
            // _currPlayData = playData;
        }

        #region SkillCD
        
        /// <summary>
        /// 当前技能是否处于CD状态。
        /// </summary>
        /// <param name="skillId">技能ID。</param>
        /// <returns>是否处于CD。</returns>
        public bool IsSkillInCD(uint skillId)
        {
            if (_skillCDMap.TryGetValue(skillId, out var info))
            {
                var left = info.cdTime - (GameTime.time - info.startTime);
                if (left > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 设置技能CD。
        /// </summary>
        /// <param name="skillBaseCfg">技能数值配置表。</param>
        public void SetSkillCDInfo(SkillBaseConfig skillBaseCfg)
        {
            if (skillBaseCfg.SkillCD <= 0)
            {
                return;
            }
            var cd = skillBaseCfg.SkillCD;
            if (_skillCDMap.TryGetValue((uint)skillBaseCfg.Id, out var cdInfo))
            {
                cdInfo.startTime = GameTime.time;
                cdInfo.cdTime = cd;
            }
            else
            {
                cdInfo = new SkillCDInfo { startTime = GameTime.time, cdTime = cd };
                _skillCDMap.Add((uint)skillBaseCfg.Id, cdInfo);
            }
        }

        #endregion
        
        private void CostMp(SkillBaseConfig skillBaseCfg)
        {
            Owner.ActorData.Mp -= skillBaseCfg.CostMP;
        }
    }
}