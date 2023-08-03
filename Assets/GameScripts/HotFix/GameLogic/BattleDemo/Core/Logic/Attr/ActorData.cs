using System;
using TEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// Actor属性数据管理。
    /// </summary>
    public class ActorData : EntityLogicComponent
    {
        public ActorAttrData AttrData => m_attrData;
        
        /// <summary>
        /// 属性数据
        /// </summary>
        private ActorAttrData m_attrData = new ActorAttrData();

        /// <summary>
        /// 初始数据
        /// </summary>
        internal ActorAttrData m_baseData = new ActorAttrData();

        private float _hp;

        public float Hp
        {
            set
            {
                if (Math.Abs(_hp - value) > 0.001f)
                {
                    float oldValue = _hp;
                    bool isDecrease = _hp > value;
                    _hp = value;
                    ActorEventHelper.Send(Owner, ActorEventType.ActorHpChg, isDecrease);
                    GameEvent.Send(ActorEventType.ActorHpChg, Owner, _hp, oldValue);
                }
            }

            get => _hp;
        }
        
        private float _mp;

        public float Mp
        {
            get => _mp;
            set
            {
                float oldValue = _mp;
                bool isDecrease = _mp > value;
                bool isChg = Math.Abs(_mp - value) > 0.001f;
                if (value < 0)
                {
                    _mp = 0;
                }
                else if (value > AttrData.MaxMp)
                {
                    _mp = AttrData.MaxMp;
                }
                else
                {
                    _mp = value;
                }

                if (isChg)
                {
                    ActorEventHelper.Send(Owner, ActorEventType.ActorMpChg, isDecrease);
                    
                    GameEvent.Send(ActorEventType.ActorMpChg,Owner,_mp,oldValue);
                }
            }
        }
    }
}