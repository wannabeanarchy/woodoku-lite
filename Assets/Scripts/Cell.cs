using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    [HideInInspector] public Coordinate coordinate;


    public bool IsEmptyCell()
    {
        return (transform.childCount == 0) || (transform.childCount > 0 && !transform.GetChild(0).GetComponent<Tile>().isActive);
    } 

    public void OnDrop(PointerEventData eventData)
    { 
        if (eventData.pointerDrag != null)
        { 
            GameLogic.TryInsertShapeIntoCells(GetComponentInParent<GridGenerator>(), eventData.pointerDrag.GetComponent<ShapeTiles>(), this);
        }
    } 
}
