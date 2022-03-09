using UnityEngine;

namespace Inventory
{

    /// <summary>
    ///  Edit this enum for your item's types
    /// </summary>
    public enum ItemType
    {
        nothing,
        meeleeWeapon4x2,
        notMeleWeapon4x2,
        something
    }

    [CreateAssetMenu(fileName = "Item", menuName = "thief01's objects/Item")]
    public class Item : ScriptableObject
    {
        public int id;
        public string name;
        public Sprite itemIcon;
        public GameObject pickup;

        public bool stack;
        public int howMuchStack;

        public Vector2Int itemSize;

        public ItemType itemType;

        public bool Drop(Transform parrent)
        {
            if(pickup!=null)
            {
                GameObject g = Instantiate(pickup);
                g.transform.position = parrent.position + parrent.forward * 0.5f;
                // g.GetComponent<Pickup>().item = this;
                return true;
            }
            return false;
        }

        public Item Copy()
        {
            Item it = CreateInstance<Item>();
            it.id = id;
            it.howMuchStack = howMuchStack;
            it.itemIcon = itemIcon;
            it.itemSize = itemSize;
            it.itemType = itemType;
            it.name = name;
            it.pickup = pickup;
            it.stack = stack;

            return it;
        }
    }

}