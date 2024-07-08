using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorObjectClickManager : MonoBehaviour
{
    public delegate void ClickAction(Vector3 pos, GameObject target);
    public event ClickAction OnClicked;

    private MapEditorUIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<MapEditorUIManager>();
        OnClicked += OnClickHandler;
    }

    void OnClickHandler(Vector3 pos, GameObject target)
    {
        Debug.Log("Object Clicked: " + gameObject.name);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    OnClicked?.Invoke(hit.point, hit.collider.gameObject);
                }
            }
        }
    }
}
