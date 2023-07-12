using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public SlotHolder SlotHolder { get => slotHolder; }
        public event Action OnInventoryChanged = delegate { };

        public Vector2Int InventorySize
        {
            get
            {
                return new Vector2Int(xSize, ySize);
            }

            set
            {
                xSize = value.x;
                ySize = value.y;
            }
        }

        [SerializeField] 
        private int xSize;
        [SerializeField] 
        private int ySize;

        [SerializeField]
        private SlotHolder slotHolder;

        private List<ItemSlotData> itemsSlots = new List<ItemSlotData>();

        private void Awake()
        {
            ClearInventory();
        }

        public bool AddItem(Item i)
        {
            Vector2Int place = FindEmptyPlace(i);
            if (place.x == -1)
            {
                return false;
            }

            itemsSlots.Add(new ItemSlotData(i, place));
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool AddItemAtPosition(Item i, Vector2Int position)
        {
            if(CanSetAtPosition(i, position))
            {
                itemsSlots.Add(new ItemSlotData(i, position));
                OnInventoryChanged?.Invoke();
                return true;
            }
            return false;
        }

        public bool CanSetAtPosition(Item i, Vector2Int to)
        {
            return !IsOutsideOfInventory(i, to) && !CheckSlots(i, to);
        }

        public int GetItemCount(int itemId)
        {
            int count = 0;
            foreach (ItemSlotData i in itemsSlots)
            {
                if (i.Item.id == itemId)
                {
                    count++;
                }
            }
            return count;
        }

        public int FindSlotId(Item i)
        {
            return itemsSlots.FindIndex(it => it.Item == i);
        }

        public List<ItemSlotData> GetSlotsWithItems()
        {
            return itemsSlots;
        }

        public void ClearInventory()
        {
            itemsSlots.Clear();
            OnInventoryChanged();
        }

        public void RemoveItem(Item item)
        {
            if(itemsSlots.RemoveAll(ctg => ctg.Item == item) > 0)
            {
                OnInventoryChanged();
            }
        }

        public void DropItem(Item item)
        {
            if(item.Drop(transform))
            {
                RemoveItem(item);
            }
        }

        /// <summary>
        /// Move item to new position in this inventory
        /// </summary>
        /// <param name="item">Item from this inventory</param>
        /// <param name="to">New position of item</param>

        public void Move(Item item, Vector2Int to)
        {
            if(CanSetAtPosition(item, to))
            {
                ForceMove(item, to);
                OnInventoryChanged?.Invoke();
            }
        }

        /// <summary>
        /// Move item from this inventory to another inventory
        /// </summary>
        /// <param name="target"> move to target inventory</param>
        /// <param name="item"> item from this inventory</param>
        /// <param name="position"> position in new inventory</param>
        public void MoveToAnotherInventory(Inventory target, Item item, Vector2Int position)
        {
            if(target.AddItemAtPosition(item, position))
            {
                RemoveItem(item);
            }
        }

        /// <summary>
        /// Move item from this inventory to Armable slot
        /// </summary>
        /// <param name="simpleSlot">Armable slot target</param>
        /// <param name="item">Item from this inventory</param>
        public void MoveToArmableSlot(SimpleSlot simpleSlot, Item item)
        {
            if(simpleSlot.SetItem(item))
            {
                RemoveItem(item);
            }
        }

        /// <summary>
        /// "forceMove" doesn't check that there is empty space or no. It is just moving item from to. If you want use it be sure what you are doing.
        /// "forceMove" calls after check "canMove" in "move"'s function.
        /// </summary>
        /// <param name="idSlot"></param>
        /// <param name="to"></param>

        public void ForceMove(int idSlot, Vector2Int to)
        {
            itemsSlots[idSlot].Position = to;
            OnInventoryChanged();
        }

        public void ForceMove(Item item, Vector2Int to)
        {
            itemsSlots.Find(ctg => ctg.Item == item).Position = to;
        }

        private bool IsOutsideOfInventory(Item item, Vector2Int position)
        {
            if (position.x < 0 || position.y < 0)
                return true;
            if (position.x + item.itemSize.x > xSize || position.y + item.itemSize.y > ySize)
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="position"></param>
        /// <returns> true if inside</returns>

        private bool CheckSlots(Item item, Vector2Int position)
        {
            foreach(ItemSlotData isd in itemsSlots)
            {
                if (isd.IsInside(item, position))
                    return true;
            }
            return false;
        }

        private Vector2Int FindEmptyPlace(Item item)
        {
            for (int i = 0; i < InventorySize.y; i++)
            {
                for (int j = 0; j < InventorySize.x; j++)
                {
                    if (!IsOutsideOfInventory(item, new Vector2Int(j,i)) && !CheckSlots(item, new Vector2Int(j, i)))
                    {
                        return new Vector2Int(j, i);
                    }
                }
            }
            return -Vector2Int.one;
        }
    }
}