using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class UIInventory : MonoBehaviour
    {
        private const string PARRENT_IS_NULL = " parrent this game object will be setted as parrent.";
        public Inventory Inventory { get; private set; }

        [SerializeField][Tooltip("Parrent of slots, to keep order and render piority.")]
        private Transform slotsParrent;
        [SerializeField][Tooltip("Parrent of dragable items, to keep order and render piority.")]
        private Transform itemsParrent;

        private List<UIDragableItem> items = new List<UIDragableItem>();
        private List<UIDragableItem> itemsInSpecialSlot = new List<UIDragableItem>();


        [SerializeField][Tooltip("Set ur slots which will represent for example place for weapon. Order has matter.")]
        private List<UIItemSlotAmable> armableSlots = new List<UIItemSlotAmable>();
        private List<UIItemSlot> defaultSlots = new List<UIItemSlot>();
        private Vector2Int lastInventorySize = Vector2Int.zero;

        // Start is called before the first frame update
        private void Awake()
        {
            if(slotsParrent==null)
            {
                slotsParrent = transform;
                Debug.LogWarning("Slots " + PARRENT_IS_NULL);
            }
            if(itemsParrent==null)
            {
                itemsParrent = transform;
                Debug.LogWarning("Items " + PARRENT_IS_NULL);
            }

            for(int i =0; i<armableSlots.Count; i++)
            {
                armableSlots[i].InventoryUI = this;
                armableSlots[i].Position = new Vector2Int(i,0);
            }
        }

        private void OnEnable()
        {
            UIDragDropItemController.instance.OnResetStatus += ResetDisplayerStatus;
            UIDragDropItemController.instance.OnEndDrag += UpdateInventory;
        }

        private void OnDisable()
        {
            UIDragDropItemController.instance.OnResetStatus -= ResetDisplayerStatus;
            UIDragDropItemController.instance.OnEndDrag += UpdateInventory;
        }

        public void Open(Inventory inventory)
        {
            this.Inventory = inventory;
            InitInventory();
            this.Inventory.OnInventoryChanged += UpdateInventory;
            inventory.SlotHolder.OnSlotsChange += UpdateInventory;
            UpdateInventory();
        }

        public void Close()
        {
            Inventory.OnInventoryChanged -= UpdateInventory;
            Inventory.SlotHolder.OnSlotsChange -= UpdateInventory;
            gameObject.SetActive(false);
        }

        private void InitInventory()
        {
            var gridSize = UIInventoryController.instance.predefineElements.gridSize;
            var inventorySize = Inventory.InventorySize;
            var slotSketch = UIInventoryController.instance.predefineElements.generatedSlot;

            if (lastInventorySize == inventorySize)
                return;

            lastInventorySize = inventorySize;
            for (int i=0; i<inventorySize.y; i++)
            {
                for(int j=0; j<inventorySize.x; j++)
                {
                    GameObject g = Instantiate(slotSketch, slotsParrent);
                    g.transform.localPosition = new Vector3(j * gridSize.x, -i * gridSize.y);
                    defaultSlots.Add(g.GetComponent<UIItemSlot>());
                    defaultSlots[defaultSlots.Count - 1].Position = new Vector2Int(j, i);
                    defaultSlots[defaultSlots.Count - 1].InventoryUI = this;
                }
            }
        }

        public void SetDisplayerStatus(DisplayerStatus ds, Vector2Int start, Vector2Int size)
        {
            for(int i=0; i<defaultSlots.Count; i++)
            {
                if (defaultSlots[i].ImInRange(start, size))
                    defaultSlots[i].SetStatus(ds);
            }
        }

        public void SetDisplayerStatus(DisplayerStatus ds, int specialSlot)
        {
            armableSlots[specialSlot].SetStatus(ds);
        }

        public void ResetDisplayerStatus()
        {
            for (int i = 0; i < defaultSlots.Count; i++)
            {
                defaultSlots[i].SetStatus(DisplayerStatus.disabled);
            }
            for (int i = 0; i < armableSlots.Count; i++)
            {
                armableSlots[i].SetStatus(DisplayerStatus.disabled);
            }
        }

        public void ResetItems()
        {
            SetDisplayerStatus(DisplayerStatus.disabled, Vector2Int.zero, Inventory.InventorySize);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].ResetPosition();
            }
            for (int i = 0; i < itemsInSpecialSlot.Count; i++)
            {
                itemsInSpecialSlot[i].transform.position = armableSlots[itemsInSpecialSlot[i].ItemParrentInformation.Position.x].transform.position;
            }
        }

        public int GetSlotID(UIItemSlot adisu)
        {
            for (int i = 0; i < armableSlots.Count; i++)
            {
                if (adisu == armableSlots[i])
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
            var gridSize = UIInventoryController.instance.predefineElements.gridSize;
            var itemSketch = UIInventoryController.instance.predefineElements.itemSketch;

            ClearSlots();

            List<ItemSlotData> isd = Inventory.GetSlotsWithItems();

            foreach(ItemSlotData isdd in isd)
            {
                GameObject g = Instantiate(itemSketch, itemsParrent);
                g.transform.localPosition = new Vector2(isdd.Position.x, -isdd.Position.y) * gridSize;
                UIDragableItem aiud = g.GetComponent<UIDragableItem>();
                UIItemParrentInformation uIItemParrentInformation = new UIItemParrentInformation() { InventoryUI = this, Item = isdd.Item, ParrentInventory = Inventory, Position = isdd.Position, SlotHolder = false };
                aiud.SetItemParrentInformation(uIItemParrentInformation);
                this.items.Add(aiud);
            }

            List<SimpleSlot> ss = Inventory.SlotHolder.GetSlots();
            for (int i = 0; i < ss.Count; i++)
            {
                if (i >= armableSlots.Count)
                {
                    Debug.LogWarning("Armabmle slots in UI Doesn't match with slots holder.");
                    break;
                }

                if (ss[i].HoldingItem != null)
                {
                    GameObject g = Instantiate(itemSketch, itemsParrent);
                    g.transform.position = armableSlots[i].transform.position;
                    UIDragableItem aiud = g.GetComponent<UIDragableItem>();
                    UIItemParrentInformation uIItemParrentInformation = new UIItemParrentInformation() { InventoryUI = this, Item = ss[i].HoldingItem, ParrentInventory = Inventory, Position = new Vector2Int(i,0), SlotHolder = true };
                    aiud.SetItemParrentInformation(uIItemParrentInformation);
                    itemsInSpecialSlot.Add(aiud);
                    armableSlots[i].SetItemOn(false);
                }
                else
                {   
                    armableSlots[i].SetItemOn(true);
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