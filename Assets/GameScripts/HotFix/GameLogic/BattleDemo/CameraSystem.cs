using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 相机系统。
    /// </summary>
    [Update]
    [LateUpdate]
    public class CameraSystem : SubSystem
    {
        protected override void OnInit()
        {
            Log.Debug("SubSystem CameraSystem OnInit");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
        }
    }
}