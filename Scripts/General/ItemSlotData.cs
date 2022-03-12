using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class ItemSlotData
    {
        public Item Item { get; set; }
        public Vector2Int Position { get; set; }

        public ItemSlotData() { }

        public ItemSlotData(Item i, Vector2Int p)
        {
            Item = i.Copy();
            Position = p;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsInside(Item newItem, Vector2Int position)
        {
            if (newItem == Item)
                return false;
            // left top, right top, left down, right down
            Vector2Int[] allCorners = GetAllCorners(position, newItem.itemSize);
            Vector2Int[] squareCorners = GetCorners(this.Position, Item.itemSize);

            foreach (Vector2Int point in allCorners)
            {
                if (PointInSquare(point, squareCorners))
                {
                    return true;
                }
            }
            return false;
        }

        private bool PointInSquare(Vector2Int point, Vector2Int[] corners)
        {
            return point.x >= corners[0].x && point.x <= corners[1].x && point.y >= corners[0].y && point.y <= corners[1].y;
        }


        /// <summary>
        /// convert position + size to square
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Vector2Int[] GetCorners(Vector2Int position, Vector2Int size)
        {
            return new Vector2Int[] { position, position + size - Vector2Int.one };
        }

        /// <summary>
        /// Convert size and position to corners of square.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns> left top, right top, left down, right down</returns>
        private Vector2Int[] GetAllCorners(Vector2Int position, Vector2Int size)
        {
            return new Vector2Int[] { position, new Vector2Int(position.x + size.x - 1, position.y), new Vector2Int(position.x, position.y + size.y - 1), position + size - Vector2Int.one };
        }
    }
}