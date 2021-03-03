using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Data
{
    public class FirebaseLoad : MonoBehaviour
    {
        public string LoadData(string path)
        {
            string returnValue = "";
            
            var db = FirebaseDatabase.DefaultInstance;

            var dataTask = db.RootReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    Debug.Log("Load successful");
                    
                    //here we get the result from our database.
                    DataSnapshot snap = task.Result;

                    returnValue = snap.GetRawJsonValue();
                }
            });

            return returnValue;
        }
        
        public string LoadDataWithPrefix(string path, string prefix)
        {
            string returnValue = "";
            
            var db = FirebaseDatabase.DefaultInstance;

            var dataTask = db.RootReference.Child(path).Child(prefix).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    Debug.Log("Load successful");
                    
                    //here we get the result from our database.
                    DataSnapshot snap = task.Result;

                    returnValue = snap.GetRawJsonValue();
                }
            });

            return returnValue;
        }
    }
}