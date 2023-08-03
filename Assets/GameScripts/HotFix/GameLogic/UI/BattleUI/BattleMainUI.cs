using UnityEngine;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class BattleMainUI : UIWindow
    {
        private Touch m_touchView;

        #region 脚本工具生成的代码

        private RectTransform m_rectContainer;
        private GameObject m_itemTouch;

        public override void ScriptGenerator()
        {
            m_rectContainer = FindChildComponent<RectTransform>("m_rectContainer");
            m_itemTouch = FindChild("m_rectContainer/m_itemTouch").gameObject;
        }

        #endregion

        public override void BindMemberProperty()
        {
            m_touchView = CreateWidget<Touch>(m_itemTouch);
        }
    }
}