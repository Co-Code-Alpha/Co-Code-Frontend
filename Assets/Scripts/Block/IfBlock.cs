using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class IfBlock : MonoBehaviour, IPointerClickHandler
{
    public ProblemManager problemManager;

    public GameObject ifPopup;
    public GameObject canvas;

    public bool condition;

    public TMP_InputField input;
    
    float clickTime = 0;

    void OnMouseDoubleClick()
    {
        ifPopup.SetActive(true);
        Vector2 mousePosition = Input.mousePosition;
        Vector2 offset = new Vector2(-5f, 5f);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ifPopup.transform.parent.GetComponent<RectTransform>(), mousePosition, null, out Vector2 localPoint);
        ifPopup.transform.localPosition = localPoint + offset;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - clickTime) < 0.3f)
        {
            OnMouseDoubleClick();
            clickTime = -1;
        }
        else
        {
            clickTime = Time.time;
        }
    }
    
    void Start()
    {
        problemManager = FindObjectOfType<ProblemManager>();
        canvas = GameObject.Find("Canvas");
        ifPopup = canvas.transform.Find("If Popup").gameObject;
    }

    void Update()
    {
        
    }

    public void ChangeCondition()
    {
        switch (problemManager.problemNum)
        {
            case 1:
                if (input.text == "True") condition = true;
                else condition = false;
                break;
            default:
                condition = false;
                break;
        }
    }
    
    
}
