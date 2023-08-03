using TEngine;

namespace GameLogic
{
    public class Entity2VisualEvent
    {
        public static readonly int CreateActor = StringId.StringToHash("Entity2VisualEvent.CreateActor");
        
        public static readonly int DestroyActor = StringId.StringToHash("Entity2VisualEvent.DestroyActor");
        
        public static readonly int DestroyAllActor = StringId.StringToHash("Entity2VisualEvent.DestroyAllActor");
    }
}