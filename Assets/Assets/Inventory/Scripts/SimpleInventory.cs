using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : Inventory
{
    int maxSize;
    private void Awake()
    {
        maxSize = inventorySize.x * inventorySize.y;
    }

    public override void addCash(long ammout)
    {
        cash += ammout;
    }

    public override bool addItem(Item i)
    {
        if(canAddItem())
        {
            items.Add(i);
            return true;
        }
        return false;
    }

    bool canAddItem()
    {
        if(maxSize==0)
        {
            return true;
        }

        if(maxSize>items.Count)
        {
            return true;
        }

        return false;
    }

    public override void clearInventory()
    {
        items.Clear();
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
        items.Remove(i);
    }

    public override void removeItem(Item i, int ammout)
    {
        for(int j=0; j<ammout; j++)
        {
            items.Remove(i);
        }
    }

    public override int getItemCount(int itemId)
    {
        throw new System.NotImplementedException();
    }
}
