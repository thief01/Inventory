# Inventory

It is inventory stylized like sacred or commandos games. Here you can see how it is working. Under gifs is small description about inventory and use.

<img src="https://i.imgur.com/EAlMbE9.gif"> 

<img src="https://i.imgur.com/nTYZlVv.gif"> 

<img src="https://i.imgur.com/wuTkSM9.gif"> 

## Operation of inventory <a name="Operation of inventory"></a>

Inventory is holding your items which you can move them to slots(class SlotHolder), also you can move to another inventory.
If you want more than 2 inventories at one screen and keep operation between them you have to do change in "InventoryUIController" in fucntion open(Inventory, Inventory); you have to add one more var type of inventory, and you have to close him in Close(); function



## SlotHolder <a name="Slots Holder"></a>

SlotsHolder is holding your special slots like for sword/gun/armor etc. everything what you need is change Enum in file "Item.cs", there you have to define your types of items.

## Prefabs/Using <a name="Slots Holder"></a>

After your changes you need to attach Inventory.cs and SlotHolder.cs to the object and define size of inventory and count of slots also their types.
At the first run/show inventory, scripts will generate your inventory/normal slots, but special slots you have to do with yourself.
And use my prefabs edit them for yourself.
