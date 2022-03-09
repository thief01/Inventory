using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] 
        private int xSize;
        [SerializeField] 
        private int ySize;

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

        public event Action OnInventoryChanged = delegate { };

        private List<ItemsSlotData> itemsSlots = new List<ItemsSlotData>();

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

            itemsSlots.Add(new ItemsSlotData(i, place));
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool AddItemAtPosition(Item i, Vector2Int position)
        {
            if(CanMove(i, position))
            {
                itemsSlots.Add(new ItemsSlotData(i, position));
                OnInventoryChanged?.Invoke();
                return true;
            }
            return false;
        }

        public bool CanMove(int idSlot, Vector2Int to)
        {
            return !IsOutsideOfInventory(itemsSlots[idSlot].item, to) && !CheckSlots(itemsSlots[idSlot].item, to);
        }

        public bool CanMove(Item i, Vector2Int to)
        {
            return !IsOutsideOfInventory(i, to) && !CheckSlots(i, to);
        }

        public int GetItemCount(int itemId)
        {
            int count = 0;
            foreach (ItemsSlotData i in itemsSlots)
            {
                if (i.item.id == itemId)
                {
                    count++;
                }
            }
            return count;
        }

        public int FindSlotId(Item i)
        {
            return itemsSlots.FindIndex(it => it.item == i);
        }

        public List<ItemsSlotData> GetSlotsWithItems()
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
            if(itemsSlots.RemoveAll(ctg => ctg.item == item) > 0)
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

        public void Move(int idSlot, Vector2Int to)
        {
            if (CanMove(idSlot, to))
            {
                ForceMove(idSlot, to);
            }
        }    

        public void MoveToAnotherInventory(Inventory target, Item item, Vector2Int position)
        {
            if(target.AddItemAtPosition(item, position))
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
            itemsSlots[idSlot].position = to;
            OnInventoryChanged();
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
            foreach(ItemsSlotData isd in itemsSlots)
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