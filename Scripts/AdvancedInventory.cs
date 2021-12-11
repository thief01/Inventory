using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedInventory : Inventory
{
    public static AdvancedInventory instance;
    int[,] slots;
    private void Awake()
    {
        slots = new int[inventorySize.x, inventorySize.y];
        if (instance == null)
            instance = this;
        clearInventory();
    }

    #region Overrides and components of overrides

    public override void addCash(long ammout)
    {
        cash += ammout;
    }

    public override bool addItem(Item i)
    {
        Vector2Int place = findEmptyPlace(i);
        if (place.x == -1)
        {
            return false;
        }
        takeSpace(place, i.itemSize, items.Count);
        items.Add(i);

        inventoryChanged.Invoke();
        return true;
    }

    void takeSpace(Vector2Int where, Vector2Int size, int what)
    {
        for(int i=0; i<size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                slots[i + where.x, j + where.y] = what;
            }
        }
    }

    Vector2Int findEmptyPlace(Item item)
    {
        for (int i = 0; i < inventorySize.y; i++)
        {
            for (int j = 0; j < inventorySize.x; j++)
            {
                if(slots[j,i]==-1)
                {
                    if (checkSlot(j, i, item.itemSize))
                    {
                        return new Vector2Int(j, i);
                    }
                }
            }
        }
        return -Vector2Int.one;
    }

    bool checkSlot(int x, int y, Vector2Int size, int additional=-1)
    {
        if (x + size.x-1 >= inventorySize.x || y + size.y-1 >= inventorySize.y)
            return false;
        if (x < 0 || y < 0)
            return false;
        for(int i=0; i<size.x; i++)
        {
            for(int j=0; j<size.y; j++)
            {
                if (slots[i+x, j+y] != -1 && slots[i+x,j+y] != additional)
                    return false;
            }
        }
        return true;
    }

    public override void clearInventory()
    {
        items.Clear();
        for (int i = 0; i < slots.GetLength(0); i++)
        {
            for (int j = 0; j < slots.GetLength(0); j++)
            {
                slots[i, j] = -1;
            }
        }
        if(inventoryChanged !=null)
           inventoryChanged.Invoke();
    }
    public override long getCash()
    {
        return cash;
    }

    public override List<Item> getItemsArray()
    {
        return items;
    }

    public override long removeCash(long ammout)
    {
        if (ammout > cash)
            return cash;
        else
            cash -= ammout;
        return ammout;
    }

    public override void removeItem(Item i)
    {
        int id = items.IndexOf(i);

        items.Remove(i);
        correctList(id);
        inventoryChanged.Invoke();
    }

    void correctList(int from)
    {
        for (int k = from; k < items.Count + 1; k++)
        {
            for (int i = 0; i < inventorySize.x; i++)
            {
                for (int j = 0; j < inventorySize.y; j++)
                {
                    if(slots[i,j]==k)
                    {
                        slots[i, j]--;
                    }
                }
            }
        }
    }

    public override void removeItem(Item i, int ammout)
    {
        for(int j=0; j<ammout; j++)
        {
            removeItem(i);
        }
    }

    public override int getItemCount(int itemId)
    {
        int count = 0;
        foreach(Item i in items)
        {
            if(i.id==itemId)
            {
                count++;
            }
        }
        return count;
    }
    #endregion

    #region Advanced Inventory

    public int[,] getSlots()
    {
        return slots;
    }

    public bool canMove(int idSlot, Vector2Int to)
    {
        Vector2Int size = items[idSlot].itemSize;

        return checkSlot(to.x, to.y, size, idSlot);
    }

    public void move(int idSlot, Vector2Int to)
    {
        if(canMove(idSlot, to))
        {
            forceMove(idSlot, to);
        }
    }
    /// <summary>
    /// "forceMove" doesn't check that there is empty space or no. It is just moving item from to. If you want use it be sure what you are doing.
    /// "forceMove" calls after check "canMove" in "move"'s function.
    /// </summary>
    /// <param name="idSlot"></param>
    /// <param name="to"></param>

    public void forceMove(int idSlot, Vector2Int to)
    {
        Vector2Int actually = findSlotPosition(idSlot);
        if (actually.x == -1)
            return;
        Vector2Int size = items[idSlot].itemSize;
        List<Vector2Int> blocks = new List<Vector2Int>();
        for(int i=0; i<size.x; i++)
        {
            for(int j=0; j<size.y; j++)
            {
                if(!blocks.Exists(v => v== new Vector2Int(i+actually.x, j+actually.y)))
                    slots[i + actually.x, j + actually.y] = -1;

                slots[i + to.x, j + to.y] = idSlot;
                blocks.Add(new Vector2Int(i + to.x, j + to.y));
            }
        }
        inventoryChanged.Invoke();
    }

    Vector2Int findSlotPosition(int idSlot)
    {
        for(int i=0; i<inventorySize.x; i++)
        {
            for(int j=0; j<inventorySize.y; j++)
            {
                if(slots[i,j]== idSlot)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return -Vector2Int.one;
    }

    #endregion
}
