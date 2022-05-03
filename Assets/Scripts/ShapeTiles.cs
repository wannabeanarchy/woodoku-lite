using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShapeTiles : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvasObject;
    private CanvasGroup canvasObjectGroup;
    private List<GameObject> tilesList;
    private Dictionary<int, List<Coordinate>> tilesListRotation;
    private bool isDrag = false;

    [HideInInspector] public bool isActive { get; set; }
    [HideInInspector] public int index { get; set; }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasObject = FindObjectOfType<Canvas>();
        canvasObjectGroup = FindObjectOfType<CanvasGroup>();
        canvasObjectGroup.blocksRaycasts = true;
        isActive = true;
    }

    public void GenerateTilesList(GameObject dragTile)
    {
        tilesList = new List<GameObject>();

        if (dragTile == null)
            dragTile = this.transform.GetChild(0).gameObject;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.GetComponent<Tile>().SetXY(dragTile);
            tilesList.Add(this.transform.GetChild(i).gameObject);
        }
    }

    public void GenerateTilesAllRotations()
    {
        tilesListRotation = new Dictionary<int, List<Coordinate>>();

        for (int r = 0; r < 4; r++)
        {
            List<Coordinate> _tmpCoordinate = new List<Coordinate>();

            foreach (var t in tilesList)
            {
                t.transform.position = RotateShape(t.transform.position, this.transform.position, -90f);
            }
            GenerateTilesList(null);

            var _dragTile = this.transform.GetChild(0).gameObject;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Coordinate coordinate = new Coordinate();
                int x, y;
                this.transform.GetChild(i).gameObject.GetComponent<Tile>().SetXY(_dragTile, out x, out y);
                coordinate.x = x;
                coordinate.y = y;
                _tmpCoordinate.Add(coordinate);
            }

            tilesListRotation.Add(r, _tmpCoordinate);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!isActive)
            return;

        if (!isDrag)
        {
            GameLogic.onSoundClick.Invoke();
            foreach (var t in tilesList)
            {
                t.transform.position = RotateShape(t.transform.position, this.transform.position, -90f);
            }
            GenerateTilesList(null);
        }

    }

    private Vector3 RotateShape(Vector3 point1, Vector3 point2, float angle)
    {
        angle *= Mathf.Deg2Rad;

        float x = Mathf.RoundToInt(Mathf.Cos(angle) * (point1.x - point2.x) - Mathf.Sin(angle) * (point1.y - point2.y) + point2.x);
        float y = Mathf.RoundToInt(Mathf.Sin(angle) * (point1.x - point2.x) + Mathf.Cos(angle) * (point1.y - point2.y) + point2.y);

        return new Vector3(x, y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isActive)
            return; 

        isDrag = true;
        canvasObjectGroup.blocksRaycasts = false;
        Vector3 l_VectorOffset = new Vector3(eventData.position.x, eventData.position.y, 0) - eventData.rawPointerPress.transform.position - eventData.rawPointerPress.transform.localPosition / 2;
        eventData.pointerPress.transform.position = (eventData.pointerPress.transform.position + l_VectorOffset);

        GenerateTilesList(eventData.pointerEnter);
        GameLogic.onSoundClick.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive)
            return; 

        GenerateTilesList(eventData.rawPointerPress);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.anchoredPosition += eventData.delta / canvasObject.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasObjectGroup.blocksRaycasts = true;

        if (!isActive)
            return;

        isDrag = false;

        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = new Vector3(0.6f, 0.6f, 1f);
        GameLogic.onSoundClick.Invoke();
    }
 
    public List<GameObject> GetTiles()
    {
        return tilesList;
    }

    public bool CheckShape()
    {
        if (!isActive)
            return true;

        foreach (var shapes in tilesListRotation)
        {
            if (!GameLogic.checkShapeTiles(shapes.Value))
            {
                return true;
            }
        }

        return false;
    }
}

