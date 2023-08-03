using TEngine;
using UnityEngine;

namespace GameLogic.BattleDemo
{
    public class MoveComponent : EntityLogicComponent
    {
        public float MoveSpeed = 10f;

        public override void OnAttach()
        {
            Owner.Event.AddEventListener<Vector2>(BattleEvent.StartMove, OnMove, Owner);
            Owner.Event.AddEventListener(BattleEvent.StopMove, OnStopMove, Owner);
            base.OnCreate();
        }

        public static float MaxEqualDot = 1 * 99 * 0.01f;

        private void OnMove(Vector2 moveDir)
        {
            Owner.transform.position += new Vector3(moveDir.x, moveDir.y, 0) * GameTime.deltaTime * MoveSpeed;

            Owner.transform.rotation = Quaternion.Euler(0, moveDir.x > 0 ? 0 : 180, 0);
        }

        private void OnStopMove()
        {
        }
    }
}