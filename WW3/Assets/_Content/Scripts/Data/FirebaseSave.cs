using System.Collections;
using Firebase.Database;
using UnityEngine;

namespace Data
{
    public class FirebaseSave : MonoBehaviour
    {
        public void SaveData(string path, string data)
        {
            StartCoroutine(SaveDataToFirebase(path, data));
        }
        
        public IEnumerator SaveDataToFirebase(string path, string data)
        {
            var dataTask = FirebaseDatabase.DefaultInstance.RootReference.Child(path).SetRawJsonValueAsync(data);
            yield return new WaitUntil(() => dataTask.IsCompleted);

            if (dataTask.Exception != null)
                Debug.LogWarning(dataTask.Exception);
        }
    }
}