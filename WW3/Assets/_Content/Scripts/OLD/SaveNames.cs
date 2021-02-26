using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveNames : MonoBehaviour
{
    public TMP_InputField name1;
    public TMP_InputField name2;
    
    // Start is called before the first frame update
    void Start()
    {
        name1.text = PlayerPrefs.GetString("p1-name");
        name2.text = PlayerPrefs.GetString("p2-name");
    }

    public void SavePlayerNames()
    {
        PlayerPrefs.SetString("p1-name", name1.text);
        PlayerPrefs.SetString("p2-name", name2.text);
        
        Debug.Log("New name set: " + PlayerPrefs.GetString("p1-name"));
        Debug.Log("New name set: " + PlayerPrefs.GetString("p2-name"));
    }
}