using UnityEngine;

namespace Menu
{
    public class MessagePanel : MonoBehaviour
    {
        public MainMenuManager.MenuState fallbackState;

        public void BackButton()
        {
            MainMenuManager.INSTANCE.ChangeState(fallbackState);
        }
    }
}