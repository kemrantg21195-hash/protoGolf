using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwapper : MonoBehaviour
{
    public GameObject[] oldtItems;      
    public GameObject[] newItems;   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwapAllItems();
        }
    }

    void SwapAllItems()
    {  
        for (int i = 0; i < oldtItems.Length; i++)
        {
            if (oldtItems[i] != null)
            {
                Transform hand = oldtItems[i].transform.parent;
              
                int randomIndex = Random.Range(0, newItems.Length);
             
                GameObject randItem = newItems[randomIndex];
                GameObject newItem = Instantiate(randItem, oldtItems[i].transform.position, oldtItems[i].transform.rotation);

                newItem.transform.SetParent(hand);

                Destroy(oldtItems[i]);

                oldtItems[i] = newItem;
            }
        }
    }
}