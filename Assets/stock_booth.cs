using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stock_booth : MonoBehaviour
{
    private bool close = false;
    public AudioSource audioclipbuy;
    public AudioSource audioclipsell;

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
        GetComponentInChildren<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y) - 3;

        if (close)
        {         
            if (Input.GetKeyDown("v"))
            {
                audioclipsell.Play();
            }
            else if (Input.GetKeyDown("b"))
            {
                audioclipbuy.Play();
            }
        }
    }
}
