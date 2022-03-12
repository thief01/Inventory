using UnityEngine;

namespace Inventory
{
    public class UIItemParrentInformation
    {
        public Inventory ParrentInventory { get; set; }

        public UIInventory InventoryUI { get; set; }

        public Item Item { get; set; }

        public bool SlotHolder { get; set; }

        public Vector2Int Position { get; set; }
    }
}