namespace GameLogic.BattleDemo
{
    public static partial class ActorEventHelper
    {
        public static void Send(EntityLogic actor, int eventId)
        {
            actor.Event.SendEvent(eventId);
        }

        public static void Send<T>(EntityLogic actor, int eventId, T info)
        {
            actor.Event.SendEvent<T>(eventId, info);
        }

        public static void Send<T, TU>(EntityLogic actor, int eventId, T info1, TU info2)
        {
            actor.Event.SendEvent<T, TU>(eventId, info1, info2);
        }

        public static void Send<T, TU, TV>(EntityLogic actor, int eventId, T info1, TU info2, TV info3)
        {
            actor.Event.SendEvent<T, TU, TV>(eventId, info1, info2, info3);
        }

        public static void Send<T, TU, TV, TW>(EntityLogic actor, int eventId, T info1, TU info2, TV info3, TW info4)
        {
            actor.Event.SendEvent<T, TU, TV, TW>(eventId, info1, info2, info3, info4);
        }
    }
}