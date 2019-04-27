using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stock_booth : MonoBehaviour
{
    private bool close = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            close = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            close = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (close)
        {         
            if (Input.GetKeyDown("v"))
            {
                Debug.Log("sell");
            }
            else if (Input.GetKeyDown("b"))
            {
                Debug.Log("Buy");
            }
        }
    }
}
