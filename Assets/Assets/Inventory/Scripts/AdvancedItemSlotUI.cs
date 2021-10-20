using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum DisplayerStatus
{
    possible,
    notPossible,
    disabled
}

public class AdvancedItemSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public Vector2Int position;
    [SerializeField] Image statusDisplayer;

    public void setGridSize(Vector2Int grid)
    {
        statusDisplayer.rectTransform.sizeDelta = grid;
    }

    public void setStatus(DisplayerStatus ds)
    {
        statusDisplayer.gameObject.SetActive(true);
        switch (ds)
        {
            case DisplayerStatus.possible:
                statusDisplayer.color = new Color(0, 255, 0, 120);
                break;
            case DisplayerStatus.notPossible:
                statusDisplayer.color = new Color(255, 0, 0, 120);
                break;
            case DisplayerStatus.disabled:
                statusDisplayer.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragDropItemsController.instance.dropedHere(position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragDropItemsController.instance.entered(position);
    }
}
