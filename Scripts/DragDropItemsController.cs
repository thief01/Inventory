using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropItemsController : MonoBehaviour
{
    public static DragDropItemsController instance;
    AdvancedInventoryUI adu;

    private void Awake()
    {
        adu = GetComponent<AdvancedInventoryUI>();
        if (instance == null)
            instance = this;
    }

    [SerializeField] AdvancedInventory inventory;
    AdvancedItemUIDragable aiud;
    Vector2Int offset;
    bool dragging = false;

    public void startDragging(AdvancedItemUIDragable aiud, Vector2Int offset)
    {
        this.offset = offset;
        dragging = true;
        this.aiud = aiud;
    }

    public void stopDragging()
    {
        dragging = false;
        adu.resetItems();
    }

    public void dropedHere(Vector2Int position)
    {
        inventory.move(aiud.slot,position-offset);
        adu.resetItems();
        dragging = false;
    }

    public void entered(Vector2Int position)
    {
        adu.setDisplayerStatus(DisplayerStatus.disabled, Vector2Int.zero, inventory.inventorySize);
        if(dragging)
        {
            if(inventory.canMove(aiud.slot, position-offset))
            {
                adu.setDisplayerStatus(DisplayerStatus.possible, position-offset, aiud.item.itemSize);
            }
            else
            {
                adu.setDisplayerStatus(DisplayerStatus.notPossible, position-offset, aiud.item.itemSize);
            }
        }
    }
}
