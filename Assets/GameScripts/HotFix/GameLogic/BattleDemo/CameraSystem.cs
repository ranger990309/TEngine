using GameLogic.BattleDemo;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 相机系统。
    /// </summary>
    [Update]
    [LateUpdate]
    public class CameraSystem : SubSystem
    {
        public Camera Camera;
        public EntityVisual ctrlActor;
        public float CameraMoveSpeed = 55f;

        public float spring = 0.1f;

        protected override void OnInit()
        {
            Camera = Camera.main;
            Log.Debug("SubSystem CameraSystem OnInit");
        }

        public override void OnUpdate()
        {
        }

        public override void OnLateUpdate()
        {
            if (ctrlActor == null)
            {
                return;
            }
            
            var position = ctrlActor.transform.position;
            var position1 = Camera.transform.position;
            Vector3 targetPos = new Vector3(position.x, position.y, position1.z);
            
            Camera.transform.position =
                Vector3.Lerp(Camera.transform.position, targetPos, CameraMoveSpeed * spring * GameTime.deltaTime);
        }
    }
}