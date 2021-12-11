using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class ItemsSlotData
    {
        public Item item;
        public Vector2Int position;
    }

    public class Inventory : MonoBehaviour
    {
        private const string ASSIGNED_ERROR = "Item couldn't be dropped because pickup variable isn't assigned.";

        [SerializeField ] 
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
        private List<Item> items = new List<Item>();

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
            items.Add(Item.Copy(i));
            ItemsSlotData isd = new ItemsSlotData();
            isd.item = items[items.Count - 1];
            isd.position = place;
            itemsSlots.Add(isd);

            OnInventoryChanged();
            return true;
        }

        public bool CanMove(int idSlot, Vector2Int to)
        {
            Vector2Int size = items[idSlot].itemSize;

            return CheckSlot(GenerateList(idSlot), to, size);
        }

        public bool CanMove(Item i, Vector2Int to)
        {
            Vector2Int size = i.itemSize;

            return CheckSlot(GenerateList(), to, size);
        }

        public bool Move(Inventory from, int idSlot, Vector2Int to)
        {
            if (CanMove(from.items[idSlot], to))
            {
                AddItem(from.items[idSlot]);
                from.RemoveItem(from.items[idSlot]);
                Move(items.Count - 1, to);

                return true;
            }
            return false;
        }

        public int GetItemCount(int itemId)
        {
            int count = 0;
            foreach (Item i in items)
            {
                if (i.id == itemId)
                {
                    count++;
                }
            }
            return count;
        }

        public int FindSlotId(Item i)
        {
            return items.FindIndex(it => it == i);
        }

        public int[,] GetSlots()
        {
            int[,] simpleSlots = new int[InventorySize.x, InventorySize.y];
            for (int i = 0; i < InventorySize.x; i++)
            {
                for (int j = 0; j < InventorySize.y; j++)
                {
                    simpleSlots[i, j] = -1;
                }
            }
            for (int i = 0; i < itemsSlots.Count; i++)
            {
                for (int j = itemsSlots[i].position.x; j < itemsSlots[i].item.itemSize.x + itemsSlots[i].position.x; j++)
                {
                    for (int k = itemsSlots[i].position.y; k < itemsSlots[i].item.itemSize.y + itemsSlots[i].position.y; k++)
                    {
                        simpleSlots[j, k] = i;
                    }
                }
            }
            return simpleSlots;
        }

        public void ClearInventory()
        {
            items.Clear();
            itemsSlots.Clear();

            OnInventoryChanged();
        }

        public void RemoveItem(Item item)
        {
            int id = items.IndexOf(item);

            items.RemoveAt(id);
            itemsSlots.RemoveAt(id);
            OnInventoryChanged();
        }

        public void RemoveItem(Item i, int ammout)
        {
            for (int j = 0; j < ammout; j++)
            {
                RemoveItem(i);
            }
        }

        public void DropItem(Item item)
        {
            if (item.pickup != null)
            {
                GameObject g = Instantiate(item.pickup);
                g.transform.position = transform.position + transform.forward * 0.5f;
            }
            else
            {
                Debug.LogWarning(ASSIGNED_ERROR);
            }
        }

        public void Move(int idSlot, Vector2Int to)
        {
            if (CanMove(idSlot, to))
            {
                ForceMove(idSlot, to);
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

        public List<Item> GetItemsArray()
        {
            return items;
        }

        private bool[,] GenerateList(int exclude = -1)
        {
            bool[,] simpleSlots = new bool[InventorySize.x, InventorySize.y];
            foreach (ItemsSlotData isd in itemsSlots)
            {
                for (int i = isd.position.x; i < isd.item.itemSize.x + isd.position.x; i++)
                {
                    for (int j = isd.position.y; j < isd.item.itemSize.y + isd.position.y; j++)
                    {
                        if (exclude != -1 && isd == itemsSlots[exclude])
                        {
                            continue;
                        }
                        simpleSlots[i, j] = true;
                    }
                }
            }
            return simpleSlots;
        }

        private bool CheckSlot(bool[,] simpleSlots, Vector2Int pos, Vector2Int size)
        {
            if (pos.x + size.x - 1 >= InventorySize.x || pos.y + size.y - 1 >= InventorySize.y)
                return false;
            if (pos.x < 0 || pos.y < 0)
                return false;
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    if (simpleSlots[i + pos.x, j + pos.y])
                        return false;
                }
            }
            return true;
        }

        private Vector2Int FindEmptyPlace(Item item)
        {
            bool[,] simpleSlots = GenerateList();
            for (int i = 0; i < InventorySize.y; i++)
            {
                for (int j = 0; j < InventorySize.x; j++)
                {
                    if (!simpleSlots[j, i])
                    {
                        if (CheckSlot(simpleSlots, new Vector2Int(j, i), item.itemSize))
                        {
                            return new Vector2Int(j, i);
                        }
                    }
                }
            }
            return -Vector2Int.one;
        }
    }
}