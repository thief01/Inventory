using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class UIDragableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private readonly Color DEFAULT_COLOR = Color.white;
        private readonly Color DRAGGING_COLOR = new Color(1, 1, 1, 0.5f);

        public UIItemParrentInformation ItemParrentInformation { get; private set; }

        public CanvasGroup cg { get; private set; }

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
            itemSprite.color = DRAGGING_COLOR;

            Vector3 diff = Input.mousePosition - transform.position;
            int xOffset = (int)diff.x / UIInventoryController.instance.predefineElements.gridSize.x;
            int yOffset = -(int)diff.y / UIInventoryController.instance.predefineElements.gridSize.y;
            //DragDropItemsController.instance.StartDragging(this, new Vector2Int(xOffset, -yOffset));
            DraggingInformation draggingInformation = new DraggingInformation() { DraggingObject = this, DraggingOffset = new Vector2Int(xOffset, yOffset) };
            UIDragDropItemController.instance.StartDragging(draggingInformation);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (c == null)
                c = FindObjectOfType<Canvas>();
            itemSprite.rectTransform.anchoredPosition += eventData.delta / c.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            itemSprite.color = DEFAULT_COLOR;
            cg.blocksRaycasts = true;

            //DragDropItemsController.instance.StopDragging(ItemParrentInformation.InventoryUI);
            UIDragDropItemController.instance.StopDragging();
        }

        public void SetItemParrentInformation(UIItemParrentInformation uIItemParrentInformation)
        {
            ItemParrentInformation = uIItemParrentInformation;
            itemSprite.rectTransform.sizeDelta = UIInventoryController.instance.predefineElements.gridSize * ItemParrentInformation.Item.itemSize;
        }

        public void ResetPosition()
        {
            transform.localPosition = new Vector2(ItemParrentInformation.Position.x, -ItemParrentInformation.Position.y) * UIInventoryController.instance.predefineElements.gridSize;
            itemSprite.rectTransform.sizeDelta = UIInventoryController.instance.predefineElements.gridSize * ItemParrentInformation.Item.itemSize;
        }
    }
}