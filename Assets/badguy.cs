using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badguy : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y);
        //;Vector3 goodguy = target.position;
        Vector3 pos = transform.position;
        if (Input.GetKey("up"))
        {
            pos.y += speed * Time.deltaTime;
        }
        if (Input.GetKey("down"))
        {
            pos.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey("right"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("left"))
        {
            pos.x -= speed * Time.deltaTime;
        }
        transform.position = pos;
    }
}
