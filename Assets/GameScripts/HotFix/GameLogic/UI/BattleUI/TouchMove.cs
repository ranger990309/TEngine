using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 移动操作相关
    /// </summary>
    class TouchMove : UIWidget, IUICtrlMove
    {
        private GameObject m_arrow;

        private EventTrigger m_touchEventTrigger;
        private RectTransform m_touchRect;
        private RectTransform m_touchMoveBackground;
        private RectTransform m_touchMoveButton;

        #region 参数

        private float m_touchMoveBtnDist = 50f;
        private float m_touchMinForwardMoveDist = 10f;  //最小距离，低于这个距离则不做移动处理
        private float m_touchMinBackMoveDist = 10f;     //最小距离，低于这个距离则不做移动处理
        private float m_touchMinLeftMoveDist = 10f;     //最小距离，低于这个距离则不做移动处理

        #endregion

        #region 移动控制

        private int m_moveTouchFingerId = -1;
        private bool m_moveTouchPress = false;
        private Vector2 m_moveTouchCenterPos = Vector2.zero;
        private Vector2 m_moveTouchInitPos = Vector2.zero;
        private Vector2 m_moveScreenDir =  Vector2.zero;
        private Camera m_moveTouchCamera;

        #region 键盘控制

        private bool m_keyControl = false;
        private Vector2 m_keyMoveScreenDir =  Vector2.zero;

        #endregion

        #endregion

        public override void BindMemberProperty()
        {
            m_touchRect = FindChild("TouchMove") as RectTransform;
            m_touchEventTrigger = m_touchRect.GetComponent<EventTrigger>();
            m_touchMoveBackground = FindChild(m_touchRect, "TouchBG") as RectTransform;
            m_touchMoveButton = FindChild(m_touchRect, "TouchBG/TouchButton") as RectTransform;
            m_arrow = FindChild("TouchMove/TouchBG/ContainerArrow").gameObject;
        }

        public override void OnCreate()
        {
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener(OnPointDown);
            m_touchEventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener(OnPointUp);
            m_touchEventTrigger.triggers.Add(entry);

            // InPutSys.Instance.MoveCtrlUI = this;
        }

        public override void OnDestroy()
        {
            m_touchEventTrigger.triggers.Clear();

            // InPutSys.Instance.MoveCtrlUI = null;
        }

        public override void RegisterEvent()
        {
            base.RegisterEvent();
            // AddUIEvent(IBattleLogic_Event.BreakTouchMove, BreakMove);
        }

        public override void OnRefresh()
        {
            m_arrow.SetActive(false);
        }

        private Vector2 FindTouchPosByFingerId(int id)
        {
            if (TryGetTouchByFingerId(id, out var touch))
            {
                return touch.position;
            }

            var mousePos = Input.mousePosition;
            return new Vector2(mousePos.x, mousePos.y);
        }

        private bool TryGetTouchByFingerId(int id, out Touch touch)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                touch = Input.GetTouch(i);
                if (touch.fingerId == id)
                {
                    return true;
                }
            }

            touch = new Touch();
            return false;
        }

        private bool IsTouchValid(int fingerId)
        {
            bool isEditorPlatform = Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor;
            if (!isEditorPlatform)
            {
                if (TryGetTouchByFingerId(fingerId, out var touch))
                {
                    return true;
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        Vector2 ConvToTouchMoveLocalPos(int fingerId, Camera camera)
        {
            Vector2 pos = FindTouchPosByFingerId(fingerId);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_touchRect, pos,
                camera, out var localPos);
            return localPos;
        }

        void OnPointDown(BaseEventData param)
        {
            PointerEventData eventData = param as PointerEventData;
            if (eventData != null && eventData.pointerId != m_moveTouchFingerId && IsTouchValid(m_moveTouchFingerId))
            {
                return;
            }

            m_moveTouchPress = true;
            if (eventData != null)
            {
                m_moveTouchFingerId = eventData.pointerId;
                m_moveTouchCamera = eventData.enterEventCamera;
                var uiCamera = eventData.enterEventCamera;
                m_moveTouchCenterPos = ConvToTouchMoveLocalPos(m_moveTouchFingerId, uiCamera);
            }

            if (m_moveTouchInitPos == Vector2.zero)
            {
                var pos = m_touchMoveBackground.localPosition;
                m_moveTouchInitPos = new Vector2(pos.x, pos.y);
            }

            m_touchMoveBackground.localPosition = new Vector3(m_moveTouchCenterPos.x, m_moveTouchCenterPos.y,
                m_touchMoveBackground.localPosition.z);
            m_moveScreenDir = Vector2.zero;

            // GameEvent.Get<IBattleLogic>().StartTouchMove();
        }

        void OnPointUp(BaseEventData param)
        {
            if (param is PointerEventData eventData && m_moveTouchPress && eventData.pointerId != m_moveTouchFingerId)
            {
                return;
            }

            DoMovePointUp();
        }

        void DoMovePointUp()
        {
            m_moveTouchPress = false;
            if (m_moveTouchInitPos == Vector2.zero)
            {
                var pos = m_touchMoveBackground.localPosition;
                m_moveTouchInitPos = new Vector2(pos.x, pos.y);
            }

            m_touchMoveBackground.localPosition = new Vector3(m_moveTouchInitPos.x, m_moveTouchInitPos.y, 0);
            m_touchMoveButton.localPosition = new Vector3(0, 0, m_touchMoveButton.localPosition.z);
            m_moveScreenDir = Vector2.zero;
            m_moveTouchCamera = null;
            m_moveTouchFingerId = -1;

            // InPutSys.Instance.OnUIManualStop();
            m_arrow.SetActive(false);
        }

        void CheckMoveTouchFinger()
        {
            if (m_moveTouchPress && !IsTouchValid(m_moveTouchFingerId))
            {
                Log.Error("invalid fingerId[{0}]", m_moveTouchFingerId);
                DoMovePointUp();
            }
        }

        void UpdateTouchMovePos()
        {
            if (m_moveTouchPress)
            {
                Vector2 localPos = ConvToTouchMoveLocalPos(m_moveTouchFingerId, m_moveTouchCamera);
                Vector2 touchOffset = localPos - m_moveTouchCenterPos;
                Vector2 offset = touchOffset;
                if (offset.sqrMagnitude > m_touchMoveBtnDist * m_touchMoveBtnDist)
                {
                    offset = offset.normalized * m_touchMoveBtnDist;
                }

                m_touchMoveButton.localPosition = new Vector3(offset.x, offset.y, m_touchMoveButton.localPosition.z);
                m_moveScreenDir = Vector2.zero;

                if (touchOffset.y >= m_touchMinForwardMoveDist ||
                    touchOffset.y <= -m_touchMinBackMoveDist)
                {
                    m_moveScreenDir.y = offset.y;
                }

                if (touchOffset.x <= -m_touchMinLeftMoveDist ||
                    touchOffset.x >= m_touchMinLeftMoveDist)
                {
                    m_moveScreenDir.x = offset.x;
                }

                m_arrow.SetActive(true);

                Vector2 vec2 = offset.normalized;
                var angle = Vector3.Angle(Vector3.up, new Vector3(vec2.x, vec2.y, 0));
                if (offset.normalized.x > 0)
                {
                    angle = 360 - angle;
                }

                m_arrow.transform.localEulerAngles = new Vector3(0, 0, angle);
            }
        }

        public override void OnUpdate()
        {
            TProfiler.BeginSample("CheckMoveTouchFinger");
            CheckMoveTouchFinger();
            TProfiler.EndSample();

            TProfiler.BeginSample("UpdateTouchMovePos");
            UpdateTouchMovePos();
            TProfiler.EndSample();

            TProfiler.BeginSample("UpdateKeyMove");
            UpdateKeyMove();
            TProfiler.EndSample();
        }

        private void UpdateKeyMove()
        {
            var keyH = Input.GetAxis("Horizontal");
            var keyV = Input.GetAxis("Vertical");

            var keyControls = Math.Abs(keyH) > 0.01f || Math.Abs(keyV) > 0.01f;
            if (m_keyControl != keyControls)
            {
                m_keyControl = keyControls;
                if (!m_keyControl)
                {
                    // InPutSys.Instance.OnUIManualStop();
                }
            }

            if (keyControls)
            {
                m_keyMoveScreenDir.Set(keyH, keyV);
            }
        }

        /// <summary>
        /// 获取移动的控制
        /// </summary>
        /// <param name="screenDir"></param>
        /// <returns></returns>
        public bool TryGetMoveDir(out Vector2 screenDir)
        {
            if (m_moveTouchPress && m_moveScreenDir != Vector2.zero)
            {
                screenDir = m_moveScreenDir;
                return true;
            }

            if (m_keyControl)
            {
                screenDir = m_keyMoveScreenDir;
                return true;
            }

            screenDir = Vector2.zero;
            return false;
        }

        private void BreakMove()
        {
            DoMovePointUp();
        }
    }
}