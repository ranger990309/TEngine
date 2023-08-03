using TEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 逻辑层实体。
    /// </summary>
    public abstract class EntityLogic : Entity
    {
        /// <summary>
        /// 逻辑层实体类型。
        /// </summary>
        /// <returns></returns>
        public abstract ActorEntityType GetActorEntityType();

        /// <summary>
        /// 是否是战斗起始的Actor。
        /// <remarks>,比如双方参与战斗的玩家，或者技能编辑器里的Caster。</remarks>
        /// </summary>
        public bool IsStartActor;

        public EntityCreateData CreateData { private set; get; }

        public EntityTransform transform;

        public virtual string GetActorName()
        {
            return string.Empty;
        }

        #region 缓存常用组件

        public ActorData ActorData { protected set; get; }

        public BuffComponent BuffComponent { protected set; get; }
        public SkillCasterComponent SkillCaster { protected set; get; }

        #endregion


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

        #endregion

        #region 生命周期

        internal bool LogicCreate(EntityCreateData entityCreateData)
        {
            CreateData = entityCreateData;
            transform = new EntityTransform(this);
            if (entityCreateData.HasBornPos)
            {
                transform.SetInitPos(entityCreateData.BornPos, entityCreateData.BornForward);
            }
            OnLogicCreate();
            return true;
        }

        protected virtual void OnLogicCreate()
        {
        }

        internal void LogicDestroy()
        {
            OnLogicDestroy();
            if (CreateData != null)
            {
                MemoryPool.Release(CreateData);
            }
        }

        protected virtual void OnLogicDestroy()
        {
        }

        #endregion

        #region 附加组件
        public T Attach<T>() where T : EntityLogicComponent, new()
        {
            if (this.IsDisposed)
            {
                Log.Fatal("AddToEntity failed because of entity is disposed");
                return null;
            }

            T ret = AddComponent<T>();
            ret.OnAttach();
            return ret;
        }
        
        public void Detach<T>() where T : EntityLogicComponent, new()
        {
            if (this.IsDisposed)
            {
                Log.Fatal("AddToEntity failed because of entity is disposed");
                return;
            }

            RemoveComponent<T>();
        }
        #endregion
    }
}