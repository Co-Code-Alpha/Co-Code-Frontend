using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class EditorCameraController : MonoBehaviour
{
    private CinemachineFreeLook cam;
    public bool isLensMode = false;
    public float zoomSpeed;

    void Awake()
    {
        CinemachineCore.GetInputAxis = ClickControl;
    }
    
    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();

    }

    public float ClickControl(string axis)
    {
        if (Input.GetMouseButton(1))
            return Input.GetAxis(axis);

        return 0;
    }

    void Update()
    {
        ScrollZoom(Input.GetAxis("Mouse ScrollWheel"));
        
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            isLensMode = !isLensMode;
            if (isLensMode)
            {
                cam.m_XAxis.m_InputAxisName = "Mouse X";
                cam.m_YAxis.m_InputAxisName = "Mouse Y";
            }
            else
            {
                cam.m_XAxis.m_InputAxisName = "";
                cam.m_YAxis.m_InputAxisName = "";
            }
        }*/
    }

    private void ScrollZoom(float input)
    {
        if (input != 0f)
        {
            float zoomAmount = input * zoomSpeed * Time.deltaTime;
            cam.m_Lens.OrthographicSize += zoomAmount;
            cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize, 5f, 30f);
        }
    }

    private bool GetCurrentMouseHover()
    {
        Vector2 mousePosition = Input.mousePosition;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        return results.Count > 0;
    }
}
