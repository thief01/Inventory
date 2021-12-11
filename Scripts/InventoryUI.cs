using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private Transform slotsParent;
        [SerializeField]
        private Transform itemsParent;
        [SerializeField]
        private GameObject slot;
        [HideInInspector]
        public Inventory inventory { get; set; }
        [HideInInspector]
        public SlotHolder slotHolder { get; set; }
        [SerializeField]
        private GameObject itemSketch;
        [SerializeField]
        private ItemSlotUI[] specialSlots;

        private List<ItemUIDragable> items = new List<ItemUIDragable>();
        private List<ItemUIDragable> itemsInSpecialSlot = new List<ItemUIDragable>();

        private ItemSlotUI[,] slots;
        private Vector2Int invSize;
        // Start is called before the first frame update
        private void Start()
        {
            invSize = inventory.InventorySize;
            slots = new ItemSlotUI[invSize.x, invSize.y];

            for (int i = 0; i < invSize.y; i++)
            {
                for (int j = 0; j < invSize.x; j++)
                {
                    GameObject g = Instantiate(slot, slotsParent);
                    g.transform.localPosition = new Vector3(j * InventoryUIController.instance.gridSize.x, -i * InventoryUIController.instance.gridSize.y);
                    slots[j, i] = g.GetComponent<ItemSlotUI>();
                    slots[j, i].SetGridSize(InventoryUIController.instance.gridSize);
                    slots[j, i].position = new Vector2Int(j, i);
                    slots[j, i].advancedInventoryUI = this;
                    g.GetComponent<RectTransform>().sizeDelta = InventoryUIController.instance.gridSize;
                }
            }
            //open(inventory, slotHolder);
            for (int i = 0; i < specialSlots.Length; i++)
            {
                specialSlots[i].advancedSlotID = i;
            }
        }

        public void SetDisplayerStatus(DisplayerStatus ds, Vector2Int start, Vector2Int size)
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    if (i + start.x >= slots.GetLength(0) || j + start.y >= slots.GetLength(1))
                        continue;
                    if (start.x + i < 0 || start.y + j < 0)
                        continue;
                    slots[i + start.x, j + start.y].setStatus(ds);
                }
            }
        }

        public void setDisplayerStatus(DisplayerStatus ds, int specialSlot)
        {
            specialSlots[specialSlot].setStatus(ds);
        }

        public void ResetDisplayerStatus()
        {
            for (int i = 0; i < specialSlots.Length; i++)
            {
                specialSlots[i].setStatus(DisplayerStatus.disabled);
            }
        }

        public void ResetItems()
        {
            SetDisplayerStatus(DisplayerStatus.disabled, Vector2Int.zero, inventory.InventorySize);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].transform.localPosition = (Vector2)items[i].position * InventoryUIController.instance.gridSize * new Vector2(1, -1);
            }
            for (int i = 0; i < itemsInSpecialSlot.Count; i++)
            {
                itemsInSpecialSlot[i].transform.position = specialSlots[itemsInSpecialSlot[i].itemInSpecialSlot].transform.position;
            }
        }

        public void Open(Inventory ai, SlotHolder sh)
        {
            inventory = ai;
            slotHolder = sh;
            inventory.OnInventoryChanged += UpdateInventory;
            slotHolder.OnSlotsChange += UpdateInventory;
            UpdateInventory();
        }

        public int GetSlotID(ItemSlotUI adisu)
        {
            for (int i = 0; i < specialSlots.Length; i++)
            {
                if (adisu == specialSlots[i])
                {
                    return i;
                }
            }
            return -1;
        }

        public void SetBlockFromItems(bool active)
        {
            for (int i = 0; i < itemsInSpecialSlot.Count; i++)
            {
                itemsInSpecialSlot[i].cg.blocksRaycasts = active;
            }
            for (int i = 0; i < items.Count; i++)
            {
                items[i].cg.blocksRaycasts = active;
            }
        }

        private void UpdateInventory()
        {
            ClearSlots();
            int[,] _slots = inventory.GetSlots();
            List<Item> items = inventory.GetItemsArray();
            List<int> addedItems = new List<int>();
            for (int i = 0; i < invSize.y; i++)
            {
                for (int j = 0; j < invSize.x; j++)
                {
                    if (_slots[j, i] >= items.Count)
                    {
                        Debug.LogError("Slot has wrong value.");
                        continue;
                    }
                    slots[j, i].DebugSlot(_slots[j, i]);
                    if (_slots[j, i] == -1)
                        continue;
                    if (addedItems.Contains(_slots[j, i]))
                        continue;
                    GameObject g = Instantiate(itemSketch, itemsParent);
                    g.transform.localPosition = new Vector2(j, -i) * InventoryUIController.instance.gridSize;
                    ItemUIDragable aiud = g.GetComponent<ItemUIDragable>();
                    aiud.SetItem(items[_slots[j, i]], new Vector2Int(j, i), this);
                    aiud.advancedInventoryUI = this;
                    aiud.slot = _slots[j, i];
                    this.items.Add(aiud);
                    addedItems.Add(_slots[j, i]);
                }
            }
            SimpleSlot[] ss = slotHolder.GetSlots();
            for (int i = 0; i < ss.Length; i++)
            {
                if (ss[i].HoldingItem != null)
                {
                    GameObject g = Instantiate(itemSketch, itemsParent);
                    g.transform.position = specialSlots[i].transform.position;
                    ItemUIDragable aiud = g.GetComponent<ItemUIDragable>();
                    aiud.SetItem(ss[i].HoldingItem, this, i);
                    itemsInSpecialSlot.Add(aiud);
                    specialSlots[i].SetItemOn(false);
                }
                else
                {
                    specialSlots[i].SetItemOn(true);
                }
            }
        }

        private void ClearSlots()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
            }
            items.Clear();
            for (int i = 0; i < itemsInSpecialSlot.Count; i++)
            {
                Destroy(itemsInSpecialSlot[i].gameObject);
            }
            itemsInSpecialSlot.Clear();
        }
    }
}