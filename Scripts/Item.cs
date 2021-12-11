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

        public static Item Copy(Item i)
        {
            Item it = CreateInstance<Item>();
            it.id = i.id;
            it.howMuchStack = i.howMuchStack;
            it.itemIcon = i.itemIcon;
            it.itemSize = i.itemSize;
            it.itemType = i.itemType;
            it.name = i.name;
            it.pickup = i.pickup;
            it.stack = i.stack;

            return it;
        }
    }

}