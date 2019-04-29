using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stock_booth : MonoBehaviour
{
    private bool close = false;

    public StockMarket sm;

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

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (close)
        {         
            if (Input.GetKeyDown("v") && !sm.Player.gameOver)
            { 
                sm.Sell("stock1key");
            }
            else if (Input.GetKeyDown("b") && !sm.Player.gameOver)
            {
                sm.Buy("stock1key");
            }
        }
    }
}
