using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Inventory
{
    public enum DisplayerStatus
    {
        possible,
        notPossible,
        disabled
    }

    public enum AdvancedItemSlotUIType
    {
        invetorySpace,
        armableSlot
    }

    
    public class UIItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected readonly Color[] COLORS_FOR_STATUSES = { new Color(0, 255, 0, 120), new Color(255, 0, 0, 120), new Color(0, 0, 0, 0) };
        public UIInventory InventoryUI { get; set; }
        public Vector2Int Position { get; set; }
        public AdvancedItemSlotUIType UIItemSlotType { get; set; }

        [SerializeField]
        protected Image statusDisplayer;
        
        private void Awake()
        {
            UIItemSlotType = AdvancedItemSlotUIType.invetorySpace;
            SetStatus(DisplayerStatus.disabled);

            GetComponent<RectTransform>().sizeDelta = UIInventoryController.instance.predefineElements.gridSize;
            statusDisplayer.rectTransform.sizeDelta = UIInventoryController.instance.predefineElements.gridSize;
        }

        public bool ImInRange(Vector2Int start, Vector2Int size)
        {
            return start.x <= Position.x && start.y <= Position.y && start.x + size.x - 1 >= Position.x && start.y + size.y - 1 >= Position.y;
        }

        public void SetStatus(DisplayerStatus ds)
        {
            statusDisplayer.color = COLORS_FOR_STATUSES[(int)ds];
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            var inventoryPosition = GetInventoryPosition();
            var draggingInformation = UIDragDropItemController.instance.DraggingInformation;
            if (draggingInformation != null)
            {
                if (draggingInformation.DraggingObject.ItemParrentInformation.SlotHolder)
                {
                    draggingInformation.DraggingObject.ItemParrentInformation.ParrentInventory.SlotHolder.MoveToInventory(InventoryUI.Inventory, draggingInformation.DraggingObject.ItemParrentInformation.Item, inventoryPosition);
                }
                else
                {
                    draggingInformation.DraggingObject.ItemParrentInformation.ParrentInventory.MoveToAnotherInventory(InventoryUI.Inventory, draggingInformation.DraggingObject.ItemParrentInformation.Item, inventoryPosition);
                }
            }
            UIDragDropItemController.instance.StopDragging();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            UIDragDropItemController.instance.ExitFrom(this);
            var inventoryPosition = GetInventoryPosition();
            var draggingInformation = UIDragDropItemController.instance.DraggingInformation;
            if (draggingInformation != null)
            {
                InventoryUI.SetDisplayerStatus(InventoryUI.Inventory.CanSetAtPosition(draggingInformation.DraggingObject.ItemParrentInformation.Item, inventoryPosition) ? DisplayerStatus.possible : DisplayerStatus.notPossible, inventoryPosition,
                    draggingInformation.DraggingObject.ItemParrentInformation.Item.itemSize);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            UIDragDropItemController.instance.ExitFrom(this);
        }

        private Vector2Int GetInventoryPosition()
        {
            if(UIDragDropItemController.instance.DraggingInformation !=null)
                return Position - UIDragDropItemController.instance.DraggingInformation.DraggingOffset;
            return Vector2Int.zero;
        }
    }
}