using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCtrl : MonoBehaviour
{
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
            AdvancedInventory.instance.addItem(tempItems[id]);
        }
    }
}
