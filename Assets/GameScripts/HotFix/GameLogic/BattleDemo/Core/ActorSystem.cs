using System;
using System.Collections.Generic;
using GameLogic.BattleDemo;
using TEngine;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLogic
{
    /// <summary>
    /// Actor角色系统。
    /// <remarks>控制可视化实体。</remarks>
    /// </summary>
    public class ActorSystem : SubSystem
    {
        private static readonly Dictionary<long, EntityVisual> EntityVisualPool = new Dictionary<long, EntityVisual>();
        private static readonly List<EntityVisual> ListEntityVisuals = new List<EntityVisual>();

        public static Transform BattleRoot;

        public EntityVisual CtrlActor { private set; get; }

        protected override void OnInit()
        {
            Log.Debug("SubSystem ActorSystem OnInit");
            if (BattleRoot == null)
            {
                BattleRoot = new GameObject("BattleRoot").transform;
                Object.DontDestroyOnLoad(BattleRoot);
            }

            RegisterEvent();
        }

        private void RegisterEvent()
        {
            GameEvent.AddEventListener<EntityLogic, bool>(Entity2VisualEvent.CreateActor, OnCreateActor);
            GameEvent.AddEventListener<long>(Entity2VisualEvent.DestroyActor, DestroyActor);
        }

        /// <summary>
        /// 获取当前所控制的Actor。
        /// </summary>
        /// <returns></returns>
        public EntityVisual GetCurrCtrlActor()
        {
            return CtrlActor;
        }

        public event Action<EntityVisual> OnEntityCreate;
        public event Action<EntityVisual> OnEntityDestroy;

        public List<EntityVisual> GetAllActor(ref List<EntityVisual> temp)
        {
            if (temp == null)
            {
                temp = new List<EntityVisual>();
            }

            temp.AddRange(ListEntityVisuals);
            return temp;
        }

        /// <summary>
        /// 获取这一类型的Actor。
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<EntityVisual> GetTypeActor(ref List<EntityVisual> temp, ActorEntityType type)
        {
            if (temp == null)
            {
                temp = new List<EntityVisual>();
            }

            foreach (var actor in ListEntityVisuals)
            {
                if (actor.Entity.GetActorEntityType() == type)
                {
                    temp.Add(actor);
                }
            }

            return temp;
        }

        private void OnCreateActor(EntityLogic entityLogic, bool isStartActor = false)
        {
            CreateActor(entityLogic, isStartActor);
        }

        /// <summary>
        /// 创建表现层Actor实体。
        /// </summary>
        /// <param name="entityLogic">逻辑层实体。</param>
        /// <param name="isStartActor">是否是控制角色。</param>
        /// <returns>表现层Actor实体。</returns>
        public EntityVisual CreateActor(EntityLogic entityLogic, bool isStartActor)
        {
            if (entityLogic == null)
            {
                Log.Error("create actor failed, entityLogic data is null");
                return null;
            }

            var actor = CreateActor(entityLogic);
            if (actor == null)
            {
                Log.Error("create actor failed, EntityLogic data is {0}", entityLogic);
                return null;
            }

            if (OnEntityCreate != null)
            {
                OnEntityCreate(actor);
            }

            if (isStartActor)
            {
                CtrlActor = actor;
                BattleSystem.CameraSystem.ctrlActor = actor;
            }

            Log.Debug("EntityVisual created: {0}", actor.Entity.RuntimeId);
            return actor;
        }

        private EntityVisual CreateActor(EntityLogic entityLogic)
        {
            if (entityLogic == null)
            {
                Log.Fatal($"Create Actor Failed");
                return null;
            }

            ActorEntityType actorType = entityLogic.GetActorEntityType();

            EntityVisual entityVisual = null;

            switch (actorType)
            {
                case ActorEntityType.Player:
                {
                    entityVisual = new GameObject($"{actorType}").AddComponent<PlayerActor>();
                    entityVisual.gameObject.transform.SetParent(BattleRoot.transform);
                    entityVisual.BindEntity(entityLogic);
                    entityVisual.OnAttach();
                    break;
                }
                default:
                {
                    Log.Error("unknown actor type:{0}", actorType);
                    break;
                }
            }

            if (entityVisual != null)
            {
                EntityVisualPool.Add(entityLogic.RuntimeId, entityVisual);
                ListEntityVisuals.Add(entityVisual);
            }

            return entityVisual;
        }

        /// <summary>
        /// 销毁Actor。
        /// </summary>
        /// <param name="runtimeId"></param>
        /// <returns></returns>
        private void DestroyActor(long runtimeId)
        {
            EntityVisualPool.TryGetValue(runtimeId, out EntityVisual entityVisual);
            if (entityVisual != null)
            {
                DestroyActor(entityVisual);
            }
        }
        
        /// <summary>
        /// 销毁Actor。
        /// </summary>
        /// <param name="entityVisual"></param>
        /// <returns></returns>
        private void DestroyActor(EntityVisual entityVisual)
        {
            var runtimeId = entityVisual.Entity.RuntimeId;

            Log.Debug("on destroy EntityVisual {0}", runtimeId);

            Log.Assert(EntityVisualPool.ContainsKey(runtimeId));

            if (OnEntityDestroy != null)
            {
                OnEntityDestroy(entityVisual);
            }

            EntityVisualPool.Remove(runtimeId);
            ListEntityVisuals.Remove(entityVisual);

            EntityLogicMgr.DestroyActor(runtimeId);
            Object.Destroy(entityVisual);
        }
    }
}