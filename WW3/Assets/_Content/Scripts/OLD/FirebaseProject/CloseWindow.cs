using UnityEngine;

namespace FirebaseProject
{
    public class CloseWindow : MonoBehaviour
    {
        public void CloseThisWindow()
        {
            if (this.gameObject.name == "RegistrationSuccessMessage")
            {
                MainMenu.instance.ShowLoginPanel();
            }
            this.gameObject.SetActive(false);   
        }
    }
}