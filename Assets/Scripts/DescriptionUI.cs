using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionUI : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

}
