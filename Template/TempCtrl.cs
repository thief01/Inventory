using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class TempCtrl : MonoBehaviour
{

    [SerializeField] 
    Inventory.Inventory player;
    [SerializeField] 
    Inventory.Inventory enemy;
    [SerializeField] 
    SlotHolder playerS;
    [SerializeField] 
    SlotHolder enemyS;

    public int id;
    [SerializeField] Item[] tempItems;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            id--;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            id++;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.AddItem(tempItems[id]);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryUIController.instance.inventoryStatus)
            {
                InventoryUIController.instance.Close();
            }
            else
            {
                //AdvancedInvetoryUIController.instance.openPlayerInvetory(player, playerS);
                InventoryUIController.instance.openInvetories(player, playerS, enemy, enemyS, 0);
            }
        }
    }
}
