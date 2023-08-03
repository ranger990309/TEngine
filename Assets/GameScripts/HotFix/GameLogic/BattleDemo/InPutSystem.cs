using TEngine;
using UnityEngine;

namespace GameLogic
{
    [Update]
    public class InPutSystem : SubSystem
    {
        private IUIBattleCtrl _mCtrlIuiBattle = null;

        public IUIBattleCtrl CtrlIuiBattle
        {
            get => _mCtrlIuiBattle;
            set => _mCtrlIuiBattle = value;
        }

        public Camera MainCamera { private set; get; }

        public bool IsEnterLevel = true;

        protected override void OnInit()
        {
            Log.Debug("SubSystem InputSystem OnInit");
            MainCamera = Camera.main;
            GameEvent.AddEventListener<uint>(BattleEvent.InputSkill, OnInputSkill);
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

                TestInputAttack();
            }
        }

        #region 移动逻辑

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
            if (_mCtrlIuiBattle != null)
            {
                return _mCtrlIuiBattle.TryGetMoveDir(out screenDir);
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
            ctrlActor.Entity.Event.SendEvent(BattleEvent.StartMove, moveDir);
            ctrlActor.Event.SendEvent(BattleEvent.StartMove, moveDir);
        }

        #endregion


        #region 攻击逻辑

        /// <summary>
        /// 主角事件攻击输入。
        /// <remarks>如果是多人联机，则可以封装一个RemoteInputComponent根据协议的输入层对实体和表现进行输入。</remarks>
        /// </summary>
        /// <param name="skillId">技能Id。</param>
        private void OnInputSkill(uint skillId)
        {
            var ctrlActor = BattleSystem.ActorSystem.GetCurrCtrlActor();
            if (ctrlActor == null)
            {
                return;
            }

            ctrlActor.Entity.Event.SendEvent(BattleEvent.DoSkill, skillId);
            ctrlActor.Event.SendEvent(BattleEvent.DoSkill, skillId);
        }

        /// <summary>
        /// 测试攻击输入。
        /// <remarks>这里是偷懒检测输入。</remarks>
        /// </summary>
        private void TestInputAttack()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameEvent.Send(BattleEvent.InputSkill, (uint)1001);
            }
        }

        #endregion
    }
}