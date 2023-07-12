using UnityEngine;
using System;

namespace Inventory
{
    public class DraggingInformation
    {
        public UIDragableItem DraggingObject { get; set; }

        public Vector2Int DraggingOffset { get; set; } 
    }
    public class UIDragDropItemController : MonoBehaviour
    {
        #region Singleton
        public static UIDragDropItemController instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        #endregion

        public event Action OnResetStatus = delegate { };
        public event Action OnEndDrag = delegate { };

        public DraggingInformation DraggingInformation { get; private set; }
        public UIDragableItem draggingItem { get; private set; }

        private Vector2Int offset;
        private int draggedFromSepcialSlot = -1;
        private bool dragging = false;

        private UIItemParrentInformation draggingObject;

        public void StartDragging(DraggingInformation draggingInformation)
        {
            //draggingItem = draggingObject;
            DraggingInformation = draggingInformation;
        }

        public void StopDragging()
        {
            draggingObject = null;
            DraggingInformation = null;
            OnResetStatus?.Invoke();
            OnEndDrag?.Invoke();
        }

        public void ExitFrom(UIItemSlot itemSlotUI)
        {
            itemSlotUI.InventoryUI.ResetDisplayerStatus();
            OnResetStatus?.Invoke();
        }
    }
}