using UnityEngine;

namespace GameLogic
{
    public interface IUIBattleCtrl
    {
        bool TryGetMoveDir(out Vector2 dir);
    }
}