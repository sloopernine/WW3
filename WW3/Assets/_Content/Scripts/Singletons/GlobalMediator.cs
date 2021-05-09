using UnityEngine;

namespace Singletons
{
    [DefaultExecutionOrder(-9)]
    public class GlobalMediator : MonoBehaviour
    {
        public static GlobalMediator INSTANCE;

        private void Awake()
        {
            if (INSTANCE == null)
                INSTANCE = this;
            else
                Destroy(gameObject);
        }
    }
}