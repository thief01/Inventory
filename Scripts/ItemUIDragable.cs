using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class ItemUIDragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [HideInInspector] 
        public int itemInSpecialSlot { get; set; } = -1;
        [HideInInspector] 
        public InventoryUI advancedInventoryUI { get; set; }
        [HideInInspector] 
        public Item item { get; set; }
        [HideInInspector] 
        public int slot { get; set; }
        [HideInInspector] 
        public Vector2Int position { get; set; }
        public CanvasGroup cg { get; set; }

        [SerializeField] 
        private Color defaultColor;
        [SerializeField] 
        private Color colorWhileDragging;
        [SerializeField] 
        private Image itemSprite;
        private Canvas c;

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            cg.blocksRaycasts = false;
            if (itemInSpecialSlot != -1)
            {
                DragDropItemsController.instance.StartDragginFromSpecial(this, itemInSpecialSlot);
            }
            else
            {
                Vector3 diff = Input.mousePosition - transform.position;
                int xOffset = (int)diff.x / InventoryUIController.instance.gridSize.x;
                int yOffset = (int)diff.y / InventoryUIController.instance.gridSize.y;
                DragDropItemsController.instance.StartDragging(this, new Vector2Int(xOffset, -yOffset));
                itemSprite.color = colorWhileDragging;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (c == null)
                c = FindObjectOfType<Canvas>();
            itemSprite.rectTransform.anchoredPosition += eventData.delta / c.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragDropItemsController.instance.StopDragging(advancedInventoryUI);
            itemSprite.color = defaultColor;
            cg.blocksRaycasts = true;
        }

        public void SetItem(Item i, Vector2Int p, InventoryUI adu)
        {
            itemSprite.color = defaultColor;
            itemSprite.rectTransform.sizeDelta = InventoryUIController.instance.gridSize * i.itemSize;
            if (i != null)
            {
                item = i;
                itemSprite.sprite = i.itemIcon;
                position = p;
                advancedInventoryUI = adu;
            }
        }

        public void SetItem(Item i, InventoryUI adu, int specialSlot)
        {
            itemSprite.color = defaultColor;
            itemSprite.rectTransform.sizeDelta = InventoryUIController.instance.gridSize * i.itemSize;
            if (i != null)
            {
                item = i;
                itemSprite.sprite = i.itemIcon;
                advancedInventoryUI = adu;
                itemInSpecialSlot = specialSlot;
            }
        }

    }
}