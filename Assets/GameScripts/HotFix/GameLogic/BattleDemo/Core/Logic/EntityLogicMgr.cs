using System;
using System.Collections.Generic;
using TEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 实体类型。
    /// </summary>
    public enum ActorEntityType
    {
        None,
        Player,
        Monster,
        Pet,
        Npc,
    }
    
    /// <summary>
    /// 逻辑层实体管理器。
    /// </summary>
    public class EntityLogicMgr
    {
        private static readonly Dictionary<long, EntityLogic> EntityLogicPool = new Dictionary<long, EntityLogic>();
        private static readonly List<EntityLogic> ListEntityLogics = new List<EntityLogic>();

        public static Scene BattleRoot;
     
        public static event Action<EntityLogic> OnEntityCreate;
        public static event Action<EntityLogic> OnEntityDestroy;

        /// <summary>
        /// 获取所有的逻辑实体。
        /// </summary>
        /// <param name="temp">ref接收数组。</param>
        /// <returns>所有的逻辑实体。</returns>
        public static List<EntityLogic> GetAllActor(ref List<EntityLogic> temp)
        {
            if (temp == null)
            {
                temp = new List<EntityLogic>();
            }
            temp.AddRange(ListEntityLogics);
            return temp;
        }
        
        /// <summary>
        /// 获取某一类型的所有逻辑实体。
        /// </summary>
        /// <param name="temp">ref接收数组。</param>
        /// <param name="type">所有的逻辑实体。</param>
        /// <returns></returns>
        public static List<EntityLogic> GetTypeActor(ref List<EntityLogic> temp,ActorEntityType type)
        {
            if (temp == null)
            {
                temp = new List<EntityLogic>();
            }

            foreach (var actor in ListEntityLogics)
            {
                if (actor.GetActorEntityType() == type)
                {
                    temp.Add(actor);
                }
            }
            return temp;
        }

        /// <summary>
        /// 逻辑层根据实体创建参数创建实体。
        /// </summary>
        /// <param name="entityCreateData">实体创建参数。</param>
        /// <param name="isStartActor">是否是控制角色。</param>
        /// <returns>逻辑层实体实例。</returns>
        public static EntityLogic CreateEntityLogic(EntityCreateData entityCreateData, bool isStartActor = false)
        {
            if (entityCreateData == null)
            {
                Log.Error("create actor failed, create data is null");
                return null;
            }
            var actor = CreateActorEntityObject(entityCreateData.actorEntityType);
            if (actor == null)
            {
                Log.Error("create actor failed, create data is {0}", entityCreateData);
                return null;
            }

            actor.IsStartActor = isStartActor;
            if (!actor.LogicCreate(entityCreateData))
            {
                DestroyActor(actor);
                return null;
            }

            if (OnEntityCreate != null)
            {
                OnEntityCreate(actor);
            }
            
            // 抛出事件到Visual表现层创建表现层实体并进行绑定。
            GameEvent.Send(Entity2VisualEvent.CreateActor,actor,isStartActor);

            Log.Debug("EntityLogic created: {0}", actor.RuntimeId);
            return actor;
        }
        
        private static EntityLogic CreateActorEntityObject(ActorEntityType actorType)
        {
            EntityLogic entityLogic = null;

            if (BattleRoot == null)
            {
                BattleRoot = Scene.Create("BattleRoot");
            }

            switch (actorType)
            {
                case ActorEntityType.Player:
                {
                    entityLogic = Entity.Create<PlayerEntity>(BattleRoot);
                    break;
                }
                default:
                {
                    Log.Error("unknown actor type:{0}", actorType);
                    break;
                }
            }

            if (entityLogic != null)
            {
                EntityLogicPool.Add(entityLogic.RuntimeId, entityLogic);
                ListEntityLogics.Add(entityLogic);
            }
            return entityLogic;
        }
        
        /// <summary>
        /// 根据运行时ID销毁实体。 
        /// </summary>
        /// <param name="runtimeId"></param>
        /// <returns></returns>
        public static bool DestroyActor(long runtimeId)
        {
            EntityLogicPool.TryGetValue(runtimeId, out EntityLogic entityLogic);
            if (entityLogic != null)
            {
                return DestroyActor(entityLogic);
            }
            return false;
        }
        
        /// <summary>
        /// 根据逻辑实体实例销毁逻辑实体。
        /// </summary>
        /// <param name="entityLogic"></param>
        /// <returns></returns>
        public static bool DestroyActor(EntityLogic entityLogic)
        {
            Log.Debug("on destroy entityLogic {0}", entityLogic.RuntimeId);


            var runtimeId = entityLogic.RuntimeId;
            Log.Assert(EntityLogicPool.ContainsKey(runtimeId));

            if (OnEntityDestroy != null)
            {
                OnEntityDestroy(entityLogic);
            }

            entityLogic.LogicDestroy();
            EntityLogicPool.Remove(runtimeId);
            ListEntityLogics.Remove(entityLogic);
            return true;
        }
    }
}