using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BlockDescription : MonoBehaviour
{
    public GameObject descriptionUI;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            PlaceDescription();
        });
    }

    public void PlaceDescription()
    {
        GameObject target = Instantiate(descriptionUI, transform.position + Vector3.up * 100f, Quaternion.identity);
        target.transform.SetParent(GameObject.Find("Canvas").transform);
        target.transform.GetChild(0).GetComponent<TMP_Text>().text = GetComponent<Block>().blockData.description;
    }
}
