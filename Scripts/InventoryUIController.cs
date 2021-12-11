using UnityEngine;

namespace Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        #region Singleton

        public static InventoryUIController instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        #endregion

        public Vector2Int gridSize;
        public InventoryUI playerInventory;
        public InventoryUI[] inventories;
        public bool inventoryStatus = false;
        private int openedInventory = -1;

        public void openPlayerInvetory(Inventory playerInventory, SlotHolder sh)
        {
            inventoryStatus = true;
            this.playerInventory.gameObject.SetActive(true);
            this.playerInventory.Open(playerInventory, sh);
        }

        public void openInvetories(Inventory playerInventory, SlotHolder sh, Inventory otherInventory, SlotHolder sh2, int inventoryID)
        {
            openPlayerInvetory(playerInventory, sh);
            if (inventoryID >= inventories.Length)
                return;
            inventories[inventoryID].gameObject.SetActive(true);
            inventories[inventoryID].Open(otherInventory, sh2);
            openedInventory = inventoryID;
        }

        public void Close()
        {
            inventoryStatus = false;
            playerInventory.gameObject.SetActive(false);
            if (openedInventory != -1)
            {
                inventories[openedInventory].gameObject.SetActive(false);
                openedInventory = -1;
            }
        }

        public void BlockItems(bool active)
        {
            playerInventory.SetBlockFromItems(active);
            if (openedInventory != -1)
            {
                inventories[openedInventory].SetBlockFromItems(active);
            }
        }
    }
}