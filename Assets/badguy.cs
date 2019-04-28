using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badguy : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform target;
    public int drawOrder;
    private bool follow;
    private bool meander;
    private int counter;
    private float meanderx;
    private float meandery;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().freezeRotation = true;
        follow = false;
        meander = false;
        counter = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "EdgeCollider" && !follow)
        {
            meander = false;
        } 
        if(other.name == "Player")
        {
            Debug.Log("Gotcha bitch");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!follow)
        {
            if (Mathf.FloorToInt(Random.Range(0.0f, 1000.0f)) == 0)
            {
                follow = true;
                counter = 1000;
            }
            else
            {
                if(meander)
                {
                    Vector3 pos = transform.position;
                    Vector3 dest = pos;
                    dest.x += meanderx;
                    dest.y += meandery;
                    pos = Vector3.MoveTowards(pos, dest, 0.05f);
                    transform.position = pos;
                    if(pos == dest)
                    {
                        follow = true;
                        counter = 1000;
                    }
                }
                else
                {
                    Vector3 pos = transform.position;
                    meanderx = Random.Range(-2.0f, 2.0f) + pos.x;
                    meandery = Random.Range(-2.0f, 2.0f) + pos.y;
                    meander = true;
                }
            }
        }
        else
        {
            counter--;
            if(counter <= 0)
            {
                follow = false;
                meander = false;
            }
            GetComponentInChildren<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y) + drawOrder;
            Vector3 goodguy = target.position;
            Vector3 pos = transform.position;
            pos = Vector3.MoveTowards(pos, goodguy, 0.05f);
            float randx = Random.Range(-0.005f, 0.005f);
            float randy = Random.Range(-0.005f, 0.005f);
            pos.x += randx;
            pos.y += randy;
            transform.position = pos;
        }
    }
}
