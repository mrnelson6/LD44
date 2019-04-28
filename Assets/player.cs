using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed = 25.0f;
    public int drawOrder;
    public Rigidbody2D rb;
    public float CurrentLiquid = 40f;
    public Dictionary<string, float> CurrentInvested = new Dictionary<string, float>(){["stock1key"] = 40f };
    public GameObject spriteObj;
    public Sprite[] upSpr;
    public Sprite[] downSpr;
    public Sprite[] leftSpr;
    public Sprite[] rightSpr;
    private int frameCount = 0;
    private int sprIndex = 0;
    private int sprLen = 4;
    private int sprSpd = 10;
    // Start is called before the first:w frame update
    void Start()
    {
        rb.freezeRotation = true;
    }

    void Awake()
    {
        upSpr = Resources.LoadAll<Sprite>("playerUp");
        downSpr = Resources.LoadAll<Sprite>("playerDown");
        leftSpr = Resources.LoadAll<Sprite>("playerLeft");
        rightSpr = Resources.LoadAll<Sprite>("playerRight");
    }

    // Update is called once per frame
    void Update()
    {
        //sprite frame animation
        if (frameCount % sprSpd == 0)
        {
            sprIndex = (sprIndex + 1) % sprLen;
        }

        GetComponentInChildren<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y) + drawOrder;

        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            pos.y += speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = upSpr[sprIndex];
        }
        if (Input.GetKey("s"))
        {
            pos.y -= speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = downSpr[sprIndex];
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = rightSpr[sprIndex];
        }
        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = leftSpr[sprIndex];
        }
        transform.position = pos;
        frameCount++;
    }
}
