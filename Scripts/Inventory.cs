using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    public Vector2Int inventorySize;

    protected List<Item> items = new List<Item>();

    protected long cash;

    public delegate void onInventoryChanged();

    public onInventoryChanged inventoryChanged;

    public abstract bool addItem(Item i);

    public abstract void removeItem(Item i);

    public abstract void removeItem(Item i, int ammout);

    public abstract int getItemCount(int itemId);

    public abstract List<Item> getItemsArray();

    public abstract void clearInventory();

    public abstract void addCash(long ammout);

    public abstract long removeCash(long ammout);

    public abstract long getCash();

}
