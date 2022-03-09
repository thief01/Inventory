
namespace Inventory
{
    [System.Serializable]
    public class SimpleSlot
    {
        public Item HoldingItem;
        public ItemType SlotType;

        public bool SetItem(Item item)
        {
            if (IsPossibleToSet(item))
            {
                HoldingItem = item;
                return true;
            }
            return false;
        }

        public bool IsPossibleToSet(Item i)
        {
            return SlotType == i.itemType && HoldingItem == null;
        }

        public void RemoveItem()
        {
            HoldingItem = null;
        }
    }
}