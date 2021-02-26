using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace FirebaseProject
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager INSTANCE;

        private FirebaseUtility _firebaseUtility;

        private DataWrapper _dataWrapper = new DataWrapper();

        public DataWrapper DataWrapper
        {
            get => _dataWrapper;
            set
            {
                Save();
                _dataWrapper = value;
            }
        }

        private const string PLAYERDATA = "PlayerData";

        private Player _player;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        
            if (INSTANCE == null)
            {
                INSTANCE = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            _firebaseUtility = GetComponent<FirebaseUtility>();
        }

        public void ThisIsThePlayer(Player player)
        {
            _player = player;
            
            //Load(_player);
        }

        public void LoginPlayer(string email, string password)
        {
            _firebaseUtility.LoginPlayer(email, password);
            Load();
        }

        public void RegisterPlayer(string email, string password)
        {
            _firebaseUtility.RegisterPlayer(email, password);
        }
        
        public void Save()
        {
            string data = JsonUtility.ToJson(_dataWrapper);
            
            SaveToFile(PLAYERDATA, data);
            _firebaseUtility.Save(data);
        }

        public void SaveWithPrefix(string prefix, string data)
        {
            _firebaseUtility.SaveWithPrefix(prefix, data);
        }

        public void Load()
        {
            string localData = ReadFile(PLAYERDATA);
            string onlineData = _firebaseUtility.Load();
            string workData = "";
            
            // Compare data and determine what data to use, or if we need to create data
            if (localData != onlineData)
            {
                if (onlineData != "")
                {
                    workData = onlineData;
                }
                else if(localData != "")
                {
                    workData = localData;
                }
                else
                {
                    // NO SAVED DATA, DO SOMETHING ABOUT IT (give some kind of default values?)
                }
            }

            _dataWrapper = JsonUtility.FromJson<DataWrapper>(workData);
        }

        public string LoadWithPrefix(string prefix)
        {
            return _firebaseUtility.LoadWithPrefix(prefix);
        }
        
        private void SaveToFile(string fileName, string jsonString)
        {
            // Open a file in write mode. This will create the file if it's missing.
            // It is assumed that the path already exists.
            using (var stream = File.OpenWrite(Application.persistentDataPath + "\\" + fileName))
            {
                // Truncate the file if it exists (we want to overwrite the file)
                stream.SetLength(0);

                // Convert the string into bytes. Assume that the character-encoding is UTF8.
                // Do you not know what encoding you have? Then you have UTF-8
                var bytes = Encoding.UTF8.GetBytes(jsonString);

                // Write the bytes to the hard-drive
                stream.Write(bytes, 0, bytes.Length);

                // The "using" statement will automatically close the stream after we leave
                // the scope - this is VERY important
            }
        }
        
        private string ReadFile(string fileName)
        {
            // Open a stream for the supplied file name as a text file
            using (var stream = File.OpenText(Application.persistentDataPath + "\\" + fileName))
            {
                // Read the entire file and return the result. This assumes that we've written the
                // file in UTF-8
                return stream.ReadToEnd();
            }
        }
    }
}