using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float yOffset;
    public player target;
    public SpriteRenderer gameO;
    public SpriteRenderer help;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = target.transform.position.x;
        pos.y = target.transform.position.y + yOffset;
        transform.position = pos;
        if (target.gameOver)
        {
            gameO.transform.position = target.transform.position;
            gameO.sortingOrder = 100;
        }
        help.transform.position = target.transform.position;
        if (Input.GetKeyDown("h"))
        {
            help.sortingOrder = 100;
        }
        if (Input.GetKeyUp("h"))
        {
            help.sortingOrder = -110;
        }
    }
}
