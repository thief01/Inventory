using UnityEngine;

namespace Inventory
{
    public class UIInventoryController : MonoBehaviour
    {
        #region Singleton

        public static UIInventoryController instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        #endregion

        [System.Serializable]
        public struct PredefineElements
        {
            [Tooltip("Generated element, slot which is able to holding item.")]
            public GameObject generatedSlot;
            [Tooltip("Generated element, item which represent specify item form inventory")]
            public GameObject itemSketch;

            public Vector2Int gridSize;
        }
        public PredefineElements predefineElements;

        //[SerializeField]
        //private InventoryUI inventories;
        public UIInventory playerInventory;
        public UIInventory[] inventories;
        public bool inventoryStatus = false;
        private int openedInventory = -1;

        public void OpenPlayerInvetory(Inventory playerInventory)
        {
            inventoryStatus = true;
            this.playerInventory.gameObject.SetActive(true);
            this.playerInventory.Open(playerInventory);
        }

        public void OpenInvetories(Inventory playerInventory, Inventory otherInventory, int inventoryID)
        {
            OpenPlayerInvetory(playerInventory);
            if (inventoryID >= inventories.Length)
                return;
            inventories[inventoryID].gameObject.SetActive(true);
            inventories[inventoryID].Open(otherInventory);
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