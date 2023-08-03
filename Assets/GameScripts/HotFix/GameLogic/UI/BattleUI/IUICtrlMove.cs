using UnityEngine;

namespace GameLogic
{
    public interface IUICtrlMove
    {
        bool TryGetMoveDir(out Vector2 dir);
    }
}