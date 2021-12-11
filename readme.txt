Hi here is my inventory which i wrote.

License for this is: Creative Commons Zero v1.0 Universal,
If you want use it in your game do it, but if you want publish your game write my nickname "thief01" in credtis. Thx.

Everything what u need are c# files. But i'm not sure that my code won't crash when u configure it bad. So the best option is use prefabs which i made, in files exists few prefabs. I will describe it here.

ExampleInventory - it is full configured inventory you can see how i did this.

InventoryController - he has to exist in world because by him you can open inventories, and there exist variable which you define grid size, also you have to assign sketch of inventories like player inventory and array of inventories (you can have a lot of sketch like in Soldiers: Heroes of World War II.

ItemSketch - it is dragable prefab with image renderer it represents items. You can change default color and color while dragging, image you set in ScritableObject.

Slot - simple slot for inventory, their size is defined in InventoryController.
SpecialSlot - It is hmm. slot where u can put weapon, your character must have component "SlotHolder" and there you define what type slot will hold. Enum of types is in Item.cs there you define your types like "armor" "weapon" etc. also you have to define size of special slot. Also you have to assing your specialSlots in AdvancedInventoryUI.

The most important scripts are SlotHolder and AdvancedInventory, AdvancedInventory is holding only eq. SlotHolder is holding special slots for weapons etc. and for UI AdvancedInventoryUI.