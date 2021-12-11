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
        invetory,
        specialSlot
    }

    public class ItemSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        [HideInInspector]
        public int advancedSlotID { get; set; } = -1;
        public InventoryUI advancedInventoryUI { get; set; }
        public Vector2Int position { get; set; }
        public AdvancedItemSlotUIType advancedItemSlotUIType { get; set; }
        [SerializeField] 
        private TextMeshProUGUI tm;
        [SerializeField]
        private Image statusDisplayer;
        [SerializeField]
        private Image slotIcon;

        public void SetGridSize(Vector2Int grid)
        {
            statusDisplayer.rectTransform.sizeDelta = grid;
        }

        public void DebugSlot(int i)
        {
            tm.text = i.ToString();
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

        public void SetItemOn(bool active)
        {
            if (slotIcon)
                slotIcon.gameObject.SetActive(active);
        }

        public void OnDrop(PointerEventData eventData)
        {
            switch (advancedItemSlotUIType)
            {
                case AdvancedItemSlotUIType.invetory:
                    DragDropItemsController.instance.DropedHere(position, advancedInventoryUI);
                    break;
                case AdvancedItemSlotUIType.specialSlot:
                    DragDropItemsController.instance.DropInSpecial(advancedInventoryUI, this);
                    break;
                default:
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (advancedItemSlotUIType)
            {
                case AdvancedItemSlotUIType.invetory:
                    DragDropItemsController.instance.Entered(position, advancedInventoryUI);
                    break;
                case AdvancedItemSlotUIType.specialSlot:
                    DragDropItemsController.instance.EnteredInSpecial(advancedInventoryUI, this);
                    break;
                default:
                    break;
            }
        }
    }
}