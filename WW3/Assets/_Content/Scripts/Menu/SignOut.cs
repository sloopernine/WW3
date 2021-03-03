using Data;
using UnityEngine;

namespace Menu
{
    public class SignOut : MonoBehaviour
    {
        public void SignOutButton()
        {
            FirebaseManager.INSTANCE.Logout();
        }
    }
}