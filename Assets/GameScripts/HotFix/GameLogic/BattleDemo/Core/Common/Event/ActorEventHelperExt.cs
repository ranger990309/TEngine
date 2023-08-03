using GameConfig.Battle;
using UnityEngine;

namespace GameLogic.BattleDemo
{
    public static partial class ActorEventHelper
    {
        public static void SendCtrlMoveToPos(EntityLogic actor, Vector3 target, float distance)
        {
            actor.Event.SendEvent(ActorEventType.CtrlMoveToPos, target, distance);
        }

        public static void SendCtrlMove(EntityLogic actor, bool move, Vector3 moveDir)
        {
            actor.Event.SendEvent(ActorEventType.ActorCtrlMoveDir, move, moveDir);
        }

        public static void SendCtrlFlash(EntityLogic actor, bool flash, Vector3 moveDir)
        {
            actor.Event.SendEvent(ActorEventType.ActorCtrlFlashDir, flash, moveDir);
        }

        public static void SendSkillImpacted(EntityLogic target, EntityLogic caster, DamageInfo damageInfo)
        {
            target.Event.SendEvent(ActorEventType.SkillImpacted, caster, damageInfo);
        }

        public static void SendProcessDamage(EntityLogic target, EntityLogic caster, DamageInfo damageInfo)
        {
            target.Event.SendEvent(ActorEventType.ProcessDamage, caster, damageInfo);
        }

        public static void SendActorBuffAdd(EntityLogic actor, BufferItem buffItem)
        {
            actor.Event.SendEvent(ActorEventType.ActorBuffAdd, buffItem);
        }

        public static void SendActorBuffRmv(EntityLogic actor, int buffId)
        {
            actor.Event.SendEvent(ActorEventType.ActorBuffRmv, buffId);
        }

        public static void SendBuffStateChanged(EntityLogic actor, BuffStateID stateId, bool set)
        {
            actor.Event.SendEvent(ActorEventType.SendBuffStateChanged, stateId, set);
        }
    }
}