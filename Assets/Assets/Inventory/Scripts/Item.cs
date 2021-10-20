using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "thief01's objects/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string name;
    public Sprite itemIcon;

    public bool stack;
    public int howMuchStack;

    public Vector2Int itemSize;
}
