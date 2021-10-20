using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedInventoryUI : MonoBehaviour
{
    [SerializeField] Transform slotsParent;
    [SerializeField] Transform itemsParent;
    public Vector2Int gridSize;
    [SerializeField] GameObject slot;
    [SerializeField] AdvancedInventory inventory;
    [SerializeField] GameObject itemSketch;

    List<AdvancedItemUIDragable> items = new List<AdvancedItemUIDragable>();

    AdvancedItemSlotUI[,] slots;
    Vector2Int invSize;

    void updateInventory()
    {
        clearSlots();
        int[,] _slots = inventory.getSlots();
        List<Item> items = inventory.getItemsArray();
        List<int> addedItems= new List<int>();
        for (int i = 0; i < invSize.y; i++)
        {
            for (int j = 0; j < invSize.x; j++)
            {
                if(_slots[j,i] >= items.Count)
                {
                    Debug.LogError("Slot has wrong value.");
                    continue;
                }
                if (_slots[j, i] == -1)
                    continue;
                if (addedItems.Contains(_slots[j, i]))
                    continue;
                GameObject g = Instantiate(itemSketch, itemsParent);
                g.transform.localPosition = new Vector2(j, -i) * gridSize;
                AdvancedItemUIDragable aiud = g.GetComponent<AdvancedItemUIDragable>();
                aiud.setItem(items[_slots[j, i]], new Vector2Int(j, i), this);
                aiud.slot = _slots[j, i];
                this.items.Add(aiud);
                addedItems.Add(_slots[j, i]);
            }
        }
    }

    void clearSlots()
    {
        for(int i=0; i<items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();
    }

    public void setDisplayerStatus(DisplayerStatus ds, Vector2Int start, Vector2Int size)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (i + start.x >= slots.GetLength(0) || j + start.y >= slots.GetLength(1))
                    continue;
                if (start.x + i < 0 || start.y + j < 0)
                    continue;
                slots[i+start.x, j+start.y].setStatus(ds);
            }
        }
    }

    public void resetItems()
    {
        setDisplayerStatus(DisplayerStatus.disabled, Vector2Int.zero, inventory.inventorySize);
        for(int i=0; i<items.Count; i++)
        {
            items[i].transform.localPosition = (Vector2)items[i].position * gridSize* new Vector2(1,-1);
        }
    }

    public void openInventory(AdvancedInventory ai)
    {
        inventory = ai;
        invSize = inventory.inventorySize;
        if(slots==null)
            slots = new AdvancedItemSlotUI[invSize.x, invSize.y];

        for (int i = 0; i < invSize.y; i++)
        {
            for (int j = 0; j < invSize.x; j++)
            {
                GameObject g = Instantiate(slot, slotsParent);
                g.transform.localPosition = new Vector3(j * gridSize.x, -i * gridSize.y);
                slots[j, i] = g.GetComponent<AdvancedItemSlotUI>();
                slots[j, i].setGridSize(gridSize);
                slots[j, i].position = new Vector2Int(j, i);
                g.GetComponent<RectTransform>().sizeDelta = gridSize;
            }
        }
        inventory.inventoryChanged += updateInventory;
    }
}
