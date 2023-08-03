using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic.BattleDemo
{
    /// <summary>
    /// 外观显示组件。
    /// </summary>
    public class DisplayComponent : EntityVisual
    {
        public DisplayInfo displayInfo;

        private GameObject model;

        private ActorTransformBinder _binder = new ActorTransformBinder();

        private Animator _animator;
        
        public override void OnAttach()
        {
            // _binder.Create(Entity.transform, Time.fixedDeltaTime * 1.5f);
            // _binder.AddTrans(gameObject.transform);
            // _binder.SetTransPosChgCallBack(OnTransPosChg);
            // _binder.SetTransRotChgCallBack(OnTransRotChg);

            Entity.Event.AddEventListener(EntityVisualEvent.BASE_TRANSFROM_CHANGE, OnEntityTransformChanged, Entity);
            Entity.Event.AddEventListener(EntityVisualEvent.BASE_TRANSFROM_INIT_POS, OnEntityTransformInit, Entity);
            gameObject.GetComponent<PlayerActor>().Event.AddEventListener<Vector2>(BattleEvent.StartMove,OnStartMove,this);
            gameObject.GetComponent<PlayerActor>().Event.AddEventListener(BattleEvent.StopMove,OnStopMove,this);
        }

        private void OnDestroy()
        {
            if (displayInfo == null)
            {
                return;
            }

            MemoryPool.Release(displayInfo);
        }

        public async UniTaskVoid SetDisplay(DisplayInfo display)
        {
            this.displayInfo = display;
            if (model != null)
            {
                Destroy(model);
            }
            model = await GameModule.Resource.LoadGameObjectAsync(this.displayInfo.Location, CancellationToken.None);
            model.transform.SetParent(gameObject.transform);
            _animator = model.GetComponent<Animator>();
        }

        private void OnTransPosChg()
        {
        }

        private void OnTransRotChg(Quaternion rot)
        {
        }

        private void OnEntityTransformChanged()
        {
            Log.Info("OnEntityTransformChanged" + Entity.transform.position);
            
            var currForward = gameObject.transform.forward;
        }

        private void OnEntityTransformInit()
        {
            Log.Info("OnEntityTransformInit" + Entity.transform.position);
        }

        private void Update()
        {
            TProfiler.BeginSample("Display.SyncEntity");
            SyncEntity();
            TProfiler.EndSample();
        }

        private void SyncEntity()
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, Entity.transform.position, 0.1f);
            gameObject.transform.rotation = Quaternion.LookRotation(Entity.transform.forward);
        }

        private void OnStartMove(Vector2 dir)
        {
            if (_animator == null)
            {
                return;
            }
            _animator.SetBool(AnimatorParamDefine.Moving,true);
        }
        
        private void OnStopMove()
        {
            if (_animator == null)
            {
                return;
            }
            _animator.SetBool(AnimatorParamDefine.Moving,false);
        }
    }
}