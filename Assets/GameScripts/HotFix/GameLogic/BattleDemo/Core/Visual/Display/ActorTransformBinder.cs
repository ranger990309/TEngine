using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameLogic.BattleDemo
{
    public class ActorTransformBinder
    {
        protected List<Transform> m_listTrans = new List<Transform>();
        private EntityTransform m_entityTrans;
        private float m_smoothDurTime;
        
        private float m_smoothPosSpeed;
        private float m_smoothStartTime = 0;
        
        /// <summary>
        /// 位置变化的回调
        /// </summary>
        private Action m_chgCallBack;
        /// <summary>
        /// 位置变化的回调
        /// </summary>
        private Action<Quaternion> m_chgRotCallBack;

        public void Create(EntityTransform entityTrans, float smootDurTime)
        {
            m_entityTrans = entityTrans;
            m_smoothDurTime = smootDurTime;
        }
        
        public void AddTrans(Transform trans,bool autoFlip = false)
        {
            m_listTrans.Add(trans);

            if (m_entityTrans != null)
            {
                var entityPos = m_entityTrans.position;
                var entityForward = m_entityTrans.forward;
                var scale = m_entityTrans.Scale;
                var rot = Quaternion.LookRotation(entityForward);
                if (autoFlip)
                {
                    if (entityForward.x < 0)
                    {
                        rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y,180f);
                    }
                }
                SetTransformPos(entityPos, rot, scale);
            }
        }
        
        public void RmvTrans(Transform trans)
        {
            m_listTrans.Remove(trans);
        }
        
        private void SetTransformPos(Vector3 pos, Quaternion rot, Vector3 scale,bool isInit = false)
        {
            DoSetTransformPos(pos, rot, scale, isInit);
            if (m_chgCallBack != null)
            {
                m_chgCallBack();
            }
            if (m_chgRotCallBack != null)
            {
                m_chgRotCallBack(rot);
            }
        }

        protected virtual void DoSetTransformPos(Vector3 pos, Quaternion rot, Vector3 scale, bool isInit = false)
        {
            for (int i = 0; i < m_listTrans.Count; i++)
            {
                var trans = m_listTrans[i];
                trans.position = pos;
                trans.localRotation = rot;
                trans.localScale = scale;
            }
        }
        
        
        private void SetTransformPos(Vector3 pos)
        {
            for (int i = 0; i < m_listTrans.Count; i++)
            {
                var trans = m_listTrans[i];
                trans.localPosition = pos;
            }

            if (m_chgCallBack != null)
            {
                m_chgCallBack();
            }
        }
        
        /// <summary>
        /// 直接校对位置，不做任何平滑处理,一般用在出生位置上。
        /// </summary>
        public void SyncInit()
        {
            if (m_listTrans.Count <= 0 || m_entityTrans == null)
            {
                return;
            }

            var entityPos = m_entityTrans.position;
            var entityForward = m_entityTrans.forward;
            var scale = m_entityTrans.Scale;
            SetTransformPos(entityPos, Quaternion.LookRotation(entityForward), scale, true);
        }

        /// <summary>
        /// 开始同步。
        /// </summary>
        public void Sync()
        {
            if (m_listTrans.Count <= 0 || m_entityTrans == null)
            {
                return;
            }

            var entityPos = m_entityTrans.position;
            var entityForward = m_entityTrans.forward;
            var entityScale = m_entityTrans.Scale;

            //如果缓动为0，则直接同步
            var smoothDurTime = m_smoothDurTime;
            if (smoothDurTime <= 0.0001f)
            {
                SetTransformPos(entityPos, Quaternion.LookRotation(entityForward), entityScale);
                return;
            }

            var firstPos = m_listTrans[0].position;
            var vec = entityPos - firstPos;
            m_smoothPosSpeed = vec.magnitude / smoothDurTime;
            m_smoothStartTime = Time.time - Time.deltaTime;
        }

        public bool UpdateTransformPos()
        {
            if (m_listTrans.Count == 0)
            {
                return false;
            }
            var firstTrans = m_listTrans[0];
            if (firstTrans == null)
            {
                return false;
            }

            //判断距离
            var currPos = firstTrans.position;
            var dest = m_entityTrans.position;
            var vec = dest - currPos;
            var stepDist = m_smoothPosSpeed * Time.deltaTime;

            bool reachPos = false;
            Vector3 newPos;

            if (vec.sqrMagnitude <= stepDist * stepDist)
            {
                newPos = dest;
                reachPos = true;
            }
            else
            {
                newPos = currPos + vec.normalized * stepDist;
            }

            if (m_smoothStartTime > 0.001f)
            {
                Vector3 newScale;
                Quaternion newRot;
                var targetForward  = m_entityTrans.forward;
                var targetRot = Quaternion.LookRotation(targetForward);
                var targetScale = m_entityTrans.Scale;

                var elapseTime = Time.time - m_smoothStartTime;
                var t = elapseTime / m_smoothDurTime;
                if (t >= 1)
                {
                    newRot = Quaternion.LookRotation(targetForward);
                    newScale = targetScale;
                    m_smoothStartTime = 0;
                }
                else
                {
                    newRot = Quaternion.Slerp(firstTrans.rotation, targetRot, t);
                    newScale = Vector3.Slerp(firstTrans.localScale, targetScale, t);
                }
                
                SetTransformPos(newPos, targetRot, targetScale);
            }
            else
            {
                SetTransformPos(newPos);
            }
            
            var continueUpdate = !reachPos || m_smoothStartTime > 0.001f;
            if (!continueUpdate)
            {
                return false;
            }

            return true;
        }

        public void SetTransPosChgCallBack(Action callback)
        {
            m_chgCallBack = callback;
        }

        public void SetTransRotChgCallBack(Action<Quaternion> callback)
        {
            m_chgRotCallBack = callback;
        }
    }
}