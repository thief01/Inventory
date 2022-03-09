using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class TempCtrl : MonoBehaviour
{
    public int id;
    [SerializeField] Item[] tempItems;
    [SerializeField] Inventory.Inventory inventory;

    [SerializeField]
    Inventory.Inventory player;
    [SerializeField]
    Inventory.Inventory enemy;
    [SerializeField]
    SlotHolder playerS;
    [SerializeField]
    SlotHolder enemyS;
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
            inventory.AddItem(tempItems[id]);
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
                InventoryUIController.instance.OpenInvetories(player, playerS, enemy, enemyS, 0);
            }
        }
    }
}
