using UnityEngine;
using System;

namespace Inventory
{
    public class SlotHolder : MonoBehaviour
    {
        [SerializeField] 
        private SimpleSlot[] slots; // configure your slots in inspector

        public event Action OnSlotsChange = delegate { };

        public bool SetInSlot(int slotID, Item item)
        {
            if (slotID >= slots.Length)
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

        public SimpleSlot[] GetSlots()
        {
            return slots;
        }

        public SimpleSlot GetSlot(int slotID)
        {
            if (slotID >= slots.Length)
            {
                return new SimpleSlot();
            }

            return slots[slotID];
        }

        public void MoveToInventoryFromSlot(Inventory to, Item it, Vector2Int position)
        {
            int slotID = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (it == slots[i].HoldingItem)
                {
                    slotID = i;
                }
            }
            if (to.AddItem(slots[slotID].HoldingItem))
            {
                to.Move(to.GetSlotsWithItems().Count - 1, position);
                RemoveFromSlot(slotID);
                OnSlotsChange();
            }
        }

        public void MoveFromInventoryToSlot(Inventory from, int slotID, Item it)
        {
            if (SetInSlot(slotID, it))
            {
                from.RemoveItem(it);
                OnSlotsChange();
            }
        }

        public void MoveFromSlotToSlot(SlotHolder target, int fromID, int targetID)
        {
            if (target.SetInSlot(targetID, slots[fromID].HoldingItem))
            {
                RemoveFromSlot(fromID);
            }
        }

        public void RemoveFromSlot(int slotID)
        {
            if (slotID >= slots.Length)
            {
                return;
            }
            slots[slotID].RemoveItem();
            OnSlotsChange();
        }
    }
}
