using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdvancedItemUIDragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] Color t_d;
    [SerializeField] Color t_d2;
    [SerializeField] Image itemSprite;
    Canvas c;
    AdvancedInventoryUI adu;
    
    public Item item;
    public int slot;

    public Vector2Int position;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 diff = Input.mousePosition-transform.position;
        int xOffset = (int)diff.x / adu.gridSize.x;
        int yOffset = (int)diff.y / adu.gridSize.y;
        DragDropItemsController.instance.startDragging(this,new Vector2Int(xOffset, -yOffset));
        itemSprite.color = t_d;
        cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (c == null)
            c = FindObjectOfType<Canvas>();
        itemSprite.rectTransform.anchoredPosition += eventData.delta / c.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragDropItemsController.instance.stopDragging();
        itemSprite.color = t_d2;
        cg.blocksRaycasts = true;
    }

    public void setItem(Item i, Vector2Int p, AdvancedInventoryUI adu)
    {
        itemSprite.color = t_d2;
        itemSprite.rectTransform.sizeDelta = adu.gridSize * i.itemSize;
        if(i!=null)
        {
            item = i;
            itemSprite.sprite = i.itemIcon;
            position = p;
            this.adu = adu;
        }
    }

}
