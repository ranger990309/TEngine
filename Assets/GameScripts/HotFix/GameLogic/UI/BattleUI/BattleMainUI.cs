using UnityEngine;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class BattleMainUI : UIWindow
    {
        private TouchMove m_touchView;
        
        #region 脚本工具生成的代码
        private RectTransform m_rectContainer;
        private GameObject m_itemTouch;
        public override void ScriptGenerator()
        {
            m_rectContainer = FindChildComponent<RectTransform>("m_rectContainer");
            m_itemTouch = FindChild("m_rectContainer/m_itemTouch").gameObject;
        }
        #endregion
        #region 事件
        #endregion

        public override void BindMemberProperty()
        {
            m_touchView = CreateWidget<TouchMove>(m_itemTouch);
        }
    }
}
