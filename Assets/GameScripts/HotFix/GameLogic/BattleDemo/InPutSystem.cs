using TEngine;
using UnityEngine;

namespace GameLogic
{
    [Update]
    public class InPutSystem : SubSystem
    {
        private IUICtrlMove m_moveCtrlUI = null;

        public IUICtrlMove MoveCtrlUI
        {
            get => m_moveCtrlUI;
            set => m_moveCtrlUI = value;
        }

        public Camera MainCamera { private set; get; }

        public bool IsEnterLevel = true;

        protected override void OnInit()
        {
            Log.Debug("SubSystem InputSystem OnInit");
            MainCamera = Camera.main;
        }

        public void OnUIManualStop()
        {
            StopManualMove();
        }

        private void StopManualMove()
        {
            var ctrlActor = BattleSystem.ActorSystem.GetCurrCtrlActor();
            if (ctrlActor == null)
            {
                return;
            }
            ctrlActor.Entity.Event.SendEvent(BattleEvent.StopMove);
            ctrlActor.Event.SendEvent(BattleEvent.StopMove);
            GameEvent.Send(BattleEvent.StopMove);
        }

        public override void OnUpdate()
        {
            if (IsEnterLevel)
            {
                TProfiler.BeginSample("UpdateManualMove");
                UpdateManualMove();
                TProfiler.EndSample();
            }
        }

        private void UpdateManualMove()
        {
            if (TryGetManualMoveScreenDir(out var manualDrag))
            {
                TProfiler.BeginSample("StartMoveToScreenDir");
                StartMoveToScreenDir(manualDrag.x, manualDrag.y, 0);
                TProfiler.EndSample();
            }
            else
            {
                StopManualMove();
            }
        }

        private bool TryGetManualMoveScreenDir(out Vector2 screenDir)
        {
            if (m_moveCtrlUI != null)
            {
                return m_moveCtrlUI.TryGetMoveDir(out screenDir);
            }

            screenDir = Vector2.zero;
            return false;
        }

        private bool GetCurrentCameraRotation(out float sin, out float cos)
        {
            if (MainCamera != null)
            {
                GetTransformEulerRotation(MainCamera.transform, out sin, out cos);
                return true;
            }

            sin = cos = 0;
            return false;
        }

        public static void GetTransformEulerRotation(Transform trans, out float sin, out float cos)
        {
            float cameraYRotationAngle = trans.eulerAngles.y * Mathf.Deg2Rad;
            sin = Mathf.Sin(cameraYRotationAngle);
            cos = Mathf.Cos(cameraYRotationAngle);
        }

        /// <summary>
        /// 按照界面上的Dir的方向开始移动。
        /// </summary>
        /// <param name="dragX"></param>
        /// <param name="dragY"></param>
        /// <param name="isFlash"></param>
        public void StartMoveToScreenDir(float dragX, float dragY, byte isFlash)
        {
            TProfiler.BeginSample("GetCurrentCameraRotation");
            var ret = GetCurrentCameraRotation(out var sin, out var cos);
            TProfiler.EndSample();
            if (!ret)
            {
                GameEvent.Send(BattleEvent.StopMove, false);
                return;
            }

            var moveDriftX = dragX * cos + dragY * sin;
            var moveDriftZ = dragY * cos - dragX * sin;

            var moveDir = new Vector2();
            moveDir.Set(moveDriftX, moveDriftZ);
            moveDir.Normalize();

            var ctrlActor = BattleSystem.ActorSystem.GetCurrCtrlActor();
            if (ctrlActor == null)
            {
                return;
            }
            GameEvent.Send(BattleEvent.StartMove, true, moveDir);
            ctrlActor.Entity.Event.SendEvent(BattleEvent.StartMove,moveDir);
            ctrlActor.Event.SendEvent(BattleEvent.StartMove,moveDir);
        }
    }
}