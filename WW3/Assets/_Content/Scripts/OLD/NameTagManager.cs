using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameTagManager : MonoBehaviour
{
    public GameObject NameTagPrefab;
    public Vector3 offset;
    public GameObject[] cars;

    internal GameObject[] nameTags;

    // Start is called before the first frame update
    void Awake()
    {
        SpawnNameTags();
    }

    public void SpawnNameTags()
    {
        nameTags = new GameObject[cars.Length];

        var parent = GameObject.Find("Canvas").transform;

        for (int i = 0; i < nameTags.Length; i++)
        {
            nameTags[i] = Instantiate(NameTagPrefab, parent);
        }

        nameTags[0].GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("p1-name");
        nameTags[1].GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("p2-name");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            nameTags[i].transform.position = Camera.main.WorldToScreenPoint(cars[i].transform.position + offset);
        }
    }
}