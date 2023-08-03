// ReSharper disable InconsistentNaming

using UnityEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 实体逻辑层位置。
    /// </summary>
    public class EntityTransform
    {
        public EntityLogic entity { private set; get; }

        public EntityTransform(EntityLogic entity)
        {
            this.entity = entity;
        }

        private Vector3 _position;

        public Vector3 position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    entity.Event.SendEvent(EntityVisualEvent.BASE_TRANSFROM_CHANGE);
                }
            }
        }

        private Quaternion m_rotation;

        public Quaternion rotation
        {
            get => m_rotation;
            set
            {
                m_rotation = value;
                entity.Event.SendEvent(EntityVisualEvent.BASE_TRANSFROM_CHANGE);
            }
        }

        public Vector3 forward
        {
            get => rotation * Vector3.forward;
            set
            {
                if (forward != value)
                {
                    rotation = Quaternion.LookRotation(value);
                    entity.Event.SendEvent(EntityVisualEvent.BASE_TRANSFROM_CHANGE);
                }
            }
        }

        private Vector3 m_scale = Vector3.one;

        public Vector3 Scale
        {
            get => m_scale;
            set
            {
                if (m_scale != value)
                {
                    m_scale = value;
                    entity.Event.SendEvent(EntityVisualEvent.BASE_TRANSFROM_CHANGE);
                }
            }
        }

        /// <summary>
        /// 立刻同步位置。
        /// <remarks>不需要平滑处理，一般是出生坐标用。</remarks>
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="initForward"></param>
        public void SetInitPos(Vector3 pos, Vector3 initForward)
        {
            _position = pos;
            rotation = Quaternion.LookRotation(initForward);
            entity.Event.SendEvent(EntityVisualEvent.BASE_TRANSFROM_INIT_POS, true, true);
        }
    }
}