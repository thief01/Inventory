using UnityEngine;
using System;
using System.Collections.Generic;

namespace Inventory
{
    [System.Serializable]
    public class SlotHolder 
    {
        public event Action OnSlotsChange = delegate { };

        [Tooltip("Define your slots types, order does matter. Order will represent your order on UI.")]
        [SerializeField] 
        private List<SimpleSlot>slots = new List<SimpleSlot>(); // configure your slots in inspector
        
        private List<ItemSlotData> items = new List<ItemSlotData>();

        public bool SetInSlot(int slotID, Item item)
        {
            if (slotID >= slots.Count)
            {
                return false;
            }
            bool t = slots[slotID].SetItem(item.Copy());

            OnSlotsChange();
            return t;
        }

        public bool PossibleToSet(int slotID, Item item)
        {
            return slots[slotID].IsPossibleToSet(item);
        }

        public bool PossibleToSet(Item i , int slotPosition)
        {
            return slots[slotPosition].SlotType == i.itemType;
        }

        public List<ItemSlotData> GetItems()
        {
            return items;
        }

        public List<SimpleSlot> GetSlots()
        {
            return slots;
        }

        public SimpleSlot GetSlot(int slotID)
        {
            if (slotID >= slots.Count)
            {
                return new SimpleSlot();
            }

            return slots[slotID];
        }

        /// <summary>
        /// Move from specific slot to target inventory at position
        /// </summary>
        /// <param name="target"></param>
        /// <param name="from"></param>
        /// <param name="position"></param>

        public void MoveToInventory(Inventory target, Item item, Vector2Int position)
        {
            var from = slots.Find(ctg => ctg.HoldingItem == item);
            if(target.AddItemAtPosition(from.HoldingItem, position))
            {
                from.RemoveItem();
            }
        }

        /// <summary>
        /// Move from specific slot to specific slot
        /// </summary>
        /// <param name="targetSlot"></param>
        /// <param name="from"></param>
        public void MoveToSlot(SimpleSlot targetSlot, Item item)
        {
            var from = slots.Find(ctg => ctg.HoldingItem == item);
            if (targetSlot.SetItem(from.HoldingItem))
            {
                from.RemoveItem();
            }
        }
    }
}
