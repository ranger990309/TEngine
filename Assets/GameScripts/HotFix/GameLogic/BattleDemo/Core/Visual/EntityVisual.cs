using TEngine;
using UnityEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 表现层实体。
    /// </summary>
    public abstract class EntityVisual : MonoBehaviour
    {
        public EntityLogic Entity { private set; get; }

        public void BindEntity(EntityLogic bindEntity)
        {
            Entity = bindEntity;
        }

        #region 事件

        private ActorEventDispatcher _event;

        public ActorEventDispatcher Event
        {
            get
            {
                if (_event == null)
                {
                    _event = MemoryPool.Acquire<ActorEventDispatcher>();
                }

                return _event;
            }
        }

        public virtual void OnAttach()
        {
        }
        
        public T Attach<T>() where T : EntityVisual, new()
        {
            if (Entity == null)
            {
                Log.Fatal("AddToEntity failed because of parent.Entity is null");
                return null;
            }

            T ret = gameObject.AddComponent<T>();
            ret.Entity = Entity;
            ret.OnAttach();
            return ret;
        }
        
        #endregion
    }
}