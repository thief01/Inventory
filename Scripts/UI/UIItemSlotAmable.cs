using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class UIItemSlotAmable : UIItemSlot
    {
        [SerializeField]
        protected Image slotIcon;
        private void Awake()
        {
            UIItemSlotType = AdvancedItemSlotUIType.armableSlot;
            statusDisplayer.gameObject.SetActive(true);
            SetStatus(DisplayerStatus.disabled);

            statusDisplayer.rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        }

        public void SetItemOn(bool active)
        {
            slotIcon?.gameObject.SetActive(active);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            UIDragDropItemController.instance.ExitFrom(this);
            var draggingObject = UIDragDropItemController.instance.DraggingInformation;
            if (draggingObject != null)
            {
                SetStatus(InventoryUI.Inventory.SlotHolder.PossibleToSet(Position.x, draggingObject.DraggingObject.ItemParrentInformation.Item) ? DisplayerStatus.possible : DisplayerStatus.notPossible);
            }
        }

        public override void OnDrop(PointerEventData eventData)
        {
            var draggingObject = UIDragDropItemController.instance.DraggingInformation;
            if (draggingObject != null)
            {
                if (draggingObject.DraggingObject.ItemParrentInformation.SlotHolder)
                {
                    draggingObject.DraggingObject.ItemParrentInformation.ParrentInventory.SlotHolder.MoveToSlot(InventoryUI.Inventory.SlotHolder.GetSlot(Position.x), draggingObject.DraggingObject.ItemParrentInformation.Item);
                }
                else
                {
                    draggingObject.DraggingObject.ItemParrentInformation.ParrentInventory.MoveToArmableSlot(InventoryUI.Inventory.SlotHolder.GetSlot(Position.x), draggingObject.DraggingObject.ItemParrentInformation.Item);
                }
            }
            UIDragDropItemController.instance.StopDragging();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            SetStatus(DisplayerStatus.disabled);
        }
    }
}