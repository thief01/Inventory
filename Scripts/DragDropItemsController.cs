using UnityEngine;

namespace Inventory
{
    public class DragDropItemsController : MonoBehaviour
    {
        #region Singleton
        public static DragDropItemsController instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        #endregion

        private ItemUIDragable draggingItem;
        private InventoryUI draggedFrom;
        private Vector2Int offset;
        private int draggedFromSepcialSlot = -1;
        private bool dragging = false;
        private bool draggingFromSpecialSlot = false;

        public void StartDragging(ItemUIDragable aiud, Vector2Int offset)
        {
            draggedFrom = aiud.advancedInventoryUI;
            this.offset = offset;
            dragging = true;
            draggingItem = aiud;
            InventoryUIController.instance.BlockItems(false);
        }

        public void StopDragging(InventoryUI aiu)
        {
            dragging = false;
            draggingFromSpecialSlot = false;
            draggedFromSepcialSlot = -1;
            draggedFrom.ResetItems();
            aiu.ResetItems();
            InventoryUIController.instance.BlockItems(true);
        }

        public void DropedHere(Vector2Int position, InventoryUI target)
        {
            ResetStatuses(target);
            if (draggingFromSpecialSlot)
            {
                draggedFrom.slotHolder.MoveToInventoryFromSlot(target.inventory, draggingItem.item, position);
            }
            else
            {
                if (target == draggedFrom)
                    draggedFrom.inventory.Move(draggingItem.slot, position - offset);
                else
                    target.inventory.Move(draggedFrom.inventory, draggedFrom.inventory.FindSlotId(draggingItem.item), position - offset);
                draggedFrom.ResetItems();
                target.ResetItems();

            }
            dragging = false;
            draggingFromSpecialSlot = false;
            draggedFromSepcialSlot = -1;
            InventoryUIController.instance.BlockItems(true);
        }

        public void Entered(Vector2Int position, InventoryUI target)
        {
            try
            {
                // reseting status
                ResetStatuses(target);
                if (dragging)
                {
                    bool canMove = false;
                    if (target == draggedFrom && !draggingFromSpecialSlot)
                    {
                        canMove = target.inventory.CanMove(target.inventory.FindSlotId(draggingItem.item), position - offset);
                    }
                    else
                    {
                        canMove = target.inventory.CanMove(draggingItem.item, position - offset);
                    }
                    if (canMove)
                    {
                        target.SetDisplayerStatus(DisplayerStatus.possible, position - offset, draggingItem.item.itemSize);
                    }
                    else
                    {
                        target.SetDisplayerStatus(DisplayerStatus.notPossible, position - offset, draggingItem.item.itemSize);
                    }
                }
            }
            catch
            {
                ResetStatuses(target);
                dragging = false;
                draggingFromSpecialSlot = false;
                draggedFromSepcialSlot = -1;
            }
        }

        public void EnteredInSpecial(InventoryUI target, ItemSlotUI aisu)
        {
            ResetStatuses(target);
            if (dragging)
            {
                if (target.slotHolder.PossibleToSet(target.GetSlotID(aisu), draggingItem.item))
                {
                    target.setDisplayerStatus(DisplayerStatus.possible, target.GetSlotID(aisu));
                }
                else
                {
                    target.setDisplayerStatus(DisplayerStatus.notPossible, target.GetSlotID(aisu));
                }
            }
        }

        public void DropInSpecial(InventoryUI target, ItemSlotUI aisu)
        {
            ResetStatuses(target);
            if (draggedFrom == target && draggedFromSepcialSlot == draggedFrom.GetSlotID(aisu))
            {
                return;
            }
            if (draggingFromSpecialSlot)
            {
                draggedFrom.slotHolder.MoveFromSlotToSlot(target.slotHolder, draggedFromSepcialSlot, aisu.advancedSlotID);
            }
            else
            {
                target.slotHolder.MoveFromInventoryToSlot(draggedFrom.inventory, aisu.advancedSlotID, draggingItem.item);
            }
            draggingFromSpecialSlot = false;
            dragging = false;
            draggedFromSepcialSlot = -1;
            InventoryUIController.instance.BlockItems(true);
        }

        public void StartDragginFromSpecial(ItemUIDragable aiud, int id)
        {
            draggingFromSpecialSlot = true;
            dragging = true;
            draggedFrom = aiud.advancedInventoryUI;
            draggedFromSepcialSlot = id;
            draggingItem = aiud;
            InventoryUIController.instance.BlockItems(false);
        }

        public void ResetStatuses(InventoryUI target)
        {
            if (draggedFrom != null)
            {
                draggedFrom.SetDisplayerStatus(DisplayerStatus.disabled, Vector2Int.zero, draggedFrom.inventory.InventorySize);
                draggedFrom.ResetDisplayerStatus();
            }
            if (target != null)
            {
                target.SetDisplayerStatus(DisplayerStatus.disabled, Vector2Int.zero, target.inventory.InventorySize);
                target.ResetDisplayerStatus();
            }
        }
    }
}