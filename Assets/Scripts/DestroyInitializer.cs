using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyInitializer : MonoBehaviour
{
    public GameObject[] destroyObjects;
    
    void Start()
    {
        foreach (GameObject target in destroyObjects)
            DontDestroyOnLoad(target);

        SceneManager.LoadScene("Title");
    }
}
