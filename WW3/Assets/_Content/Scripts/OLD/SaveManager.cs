using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;

[Serializable]
public class JsonWrapper
{
    public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
}

public class SaveManager : MonoBehaviour
{
    private JsonWrapper jsonWrapper = new JsonWrapper();

    private const string PLAYERNAMES = "PlayerNames";
    private const string PLAYERDATA = "PlayerData";


    public void SavePlayerNickname()
    {
        TextMeshProUGUI input1;
        input1 = GameObject.FindGameObjectWithTag("InputField1").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI input2;
        input2 = GameObject.FindGameObjectWithTag("InputField2").GetComponent<TextMeshProUGUI>();
        
        
    }
    
    public void SavePlayerData()
    {
        AddPlayerDataToWrapper();

        string jsonString = JsonUtility.ToJson(jsonWrapper);
     
        Debug.Log(jsonString);
        
        SaveToFile(PLAYERDATA, jsonString);
        SaveOnline(PLAYERDATA, jsonString);
    }

    public void AddPlayerDataToWrapper()
    {
        jsonWrapper.playerInfo.Clear();

        int counter = 0;
        
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerInfo playerToAdd = new PlayerInfo();
            
            playerToAdd.ObjectName = player.name;
            playerToAdd.Position = player.transform.position;
            playerToAdd.direction = Tools.WrapAngle(player.transform.eulerAngles.z);
            
            jsonWrapper.playerInfo.Add(playerToAdd);

            counter++;
        }
    }
    
    public void LoadPlayerData()
    {
        string jsonStringFile = ReadFile(PLAYERDATA);
        string jsonStringOnline = ReadOnline(PLAYERDATA);

        if (jsonStringFile != jsonStringOnline)
        {
            Debug.Log("Not the same data! Cheater!");
        }
        
        jsonWrapper = JsonUtility.FromJson<JsonWrapper>(jsonStringFile);

        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < player.Length; i++)
        {
            player[i].name = jsonWrapper.playerInfo[i].ObjectName;
            player[i].transform.position = jsonWrapper.playerInfo[i].Position;
            player[i].transform.eulerAngles = new Vector3(0, 0, jsonWrapper.playerInfo[i].direction);
        }
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
        
        // StreamReader stream;
        //
        // try
        // {
        //     stream = File.OpenText(Application.persistentDataPath + "\\" + fileName);
        // }
        // catch (Exception e)
        // {
        //     File.Create(Application.persistentDataPath + "\\" + fileName);
        //     stream = File.OpenText(Application.persistentDataPath + "\\" + fileName);
        // }
        //
        // stream.Close();
        // return stream.ReadToEnd();
    }
    
    public string ReadOnline(string name)
    {
        // var request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/" + name);
        // var response = (HttpWebResponse)request.GetResponse();
        //
        // // Open a stream to the server so we can read the response data it sent back from our GET request
        // using (var stream = response.GetResponseStream())
        // {
        //     using (var reader = new StreamReader(stream))
        //     {
        //         // Read the entire body as a string
        //         var body = reader.ReadToEnd();
        //
        //         return body;
        //     }
        // }

        return "";
    }

    //Saves the playerInfo string on the server.
    public void SaveOnline(string fileName, string saveData)
    {
        // //url
        // var request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/" + fileName);
        // request.ContentType = "application/json";
        // request.Method = "PUT";
        //
        // using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        // {
        //     streamWriter.Write(saveData);
        // }
        //
        // var httpResponse = (HttpWebResponse)request.GetResponse();
        // using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        // {
        //     var result = streamReader.ReadToEnd();
        // }

        SaveToFirebase(saveData);
    }

    private void SaveToFirebase(string data)
    {
        throw new NotImplementedException();
    }
}
