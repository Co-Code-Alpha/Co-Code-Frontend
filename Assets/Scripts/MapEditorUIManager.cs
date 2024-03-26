using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.MUIP;

public class MapEditorUIManager : MonoBehaviour
{
    private MapEditor editor;
    
    public GameObject[] objectList;
    public GameObject[] placedObjectList;

    public Transform placeableListContent;
    public GameObject placeablePrefab;
    
    public Transform placedListContent;
    public GameObject placedPrefab;

    public GameObject inputPanel;
    private MapObjectPrefab selectedModel;
    
    public TMP_InputField xInput;
    public TMP_InputField yInput;
    public TMP_InputField zInput;
    public CustomDropdown directionDropdown;
    public TMP_Text errorText;
    
    public int gridSize = 10;
    public LineRenderer linePrefab;
    public Material lineMaterial;
    
    void Awake()
    {
        editor = FindObjectOfType<MapEditor>();
    }

    void Start()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        for (int i = -gridSize; i <= gridSize; i++)
            DrawLine(new Vector3(i, 0, -gridSize), new Vector3(i, 0, gridSize));

        /*
        for (int i = -gridSize; i <= gridSize; i++)
            DrawLine(new Vector3(-gridSize, i, 0), new Vector3(gridSize, i, 0));
        */

        for (int i = -gridSize; i <= gridSize; i++)
            DrawLine(new Vector3(-gridSize, 0, i), new Vector3(gridSize, 0, i));
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        LineRenderer lineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void InitPlaceableUI(List<MapObjectPrefab> placeablePrefabs)
    {
        foreach (MapObjectPrefab prefab in placeablePrefabs)
        {
            GameObject instance = Instantiate(placeablePrefab, placeableListContent);
            instance.transform.GetChild(0).GetComponent<Image>().sprite = prefab.icon;
            instance.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectedModel = prefab;
                inputPanel.SetActive(true);
            });
        }
    }

    public void PlaceObject()
    {
        string xText = xInput.text;
        string yText = yInput.text;
        string zText = zInput.text;

        int x, y, z;

        if (int.TryParse(xText, out int xResult))
            x = xResult;
        else
        {
            errorText.text = "X 좌표가 정수가 아닙니다.";
            return;
        }

        if (int.TryParse(yText, out int yResult))
            y = yResult;
        else
        {
            errorText.text = "Y 좌표가 정수가 아닙니다.";
            return;
        }

        if (int.TryParse(zText, out int zResult))
            z = zResult;
        else
        {
            errorText.text = "Z 좌표가 정수가 아닙니다.";
            return;
        }

        if (editor.CheckPlaced(x, y, z))
        {
            errorText.text = "해당 좌표에 배치된 오브젝트가 존재합니다.";
            return;
        }



        MapObject newObject = new MapObject(x, y, z, directionDropdown.index, selectedModel.modelId);
        editor.AddObject(newObject);

        errorText.text = "";
        xInput.text = "";
        yInput.text = "";
        zInput.text = "";
        directionDropdown.index = 0;
        selectedModel = null;
        inputPanel.SetActive(false);
    }

    void Update()
    {
        
    }
}
