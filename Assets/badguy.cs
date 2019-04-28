using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badguy : MonoBehaviour
{
    public float speed = 5.0f;
    public player target;
    public int drawOrder;
    private bool follow;
    private bool meander;
    private int counter;
    private float meanderx;
    private float meandery;
    public Sprite[] badguyspr;
    private int sprIndex = 0;
    private float direction;
    public GameObject spriteObj;
    public GameObject empty;
    private bool attack;
    private int attackCounter;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb.freezeRotation = true;
        follow = false;
        meander = false;
        counter = 0;
        attackCounter = 0;
        direction = 0;
        attack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "EdgeCollider" && !follow)
        {
            meander = false;
        } 
        if(other.name == "Player")
        {
            attack = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Attack"&& target.attackCounter > 20 )
        {
            Debug.Log("hi");
            //Destroy(rb.gameObject);
            //Destroy(sr.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if(attackCounter > 30)
            {
                target.almost_damage();
            }
            attack = false;
        }
    }

    void Awake()
    {
        badguyspr = Resources.LoadAll<Sprite>("badguySprite");
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
                    dest.x += meanderx * Time.deltaTime;
                    dest.y += meandery * Time.deltaTime;
                    Vector3 temp;
                    temp.x = meanderx;
                    temp.y = meandery;
                    temp.z = 0;
                    temp.Normalize();
                    direction = Vector3.SignedAngle(temp, Vector3.right, Vector3.forward);
                    pos = Vector3.MoveTowards(pos, dest, 0.05f);
                    transform.position = pos;
                    if (pos == dest)
                    {
                        follow = true;
                        counter = 1000;
                    }
                }
                else
                {
                    Vector3 pos = transform.position;
                    meanderx = (Random.Range(-2.0f, 2.0f) + pos.x) * Time.deltaTime;
                    meandery = (Random.Range(-2.0f, 2.0f) + pos.y) * Time.deltaTime;
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
            Vector3 goodguy;
            if (target.transform == null)
            {
                goodguy = empty.transform.position;
            }
            else
            {
                goodguy = target.transform.position;
            }
            Vector3 pos = transform.position;
            Vector3 directionVec = goodguy - pos;
            directionVec.Normalize();
            pos = Vector3.MoveTowards(pos, goodguy, 0.05f);
            transform.position = pos;
            direction = Vector3.SignedAngle(directionVec, Vector3.right, Vector3.forward);
        }

        if(direction < -135 || direction > 135)
        {
            sprIndex = 4;
        } else if (direction <= 135 && direction > 45)
        {
            sprIndex = 0;
        }
        else if (direction <= 45 && direction > -45)
        {
            sprIndex = 8;
        } else
        {
            sprIndex = 12;
        }
        if (attack)
        {
            attackCounter++;
            if (attackCounter < 20)
            {
                sprIndex++;
            }
            else if (attackCounter < 40)
            {
                sprIndex += 2;
            }
            else if (attackCounter < 60)
            {
                sprIndex += 3;
                if(attackCounter == 50)
                {
                    target.damage();
                }
            }
            else if (attackCounter > 80)
            {
                attackCounter = 0;
            }
        }
        else
        {
            attackCounter = 0;
        }
        spriteObj.GetComponent<SpriteRenderer>().sprite = badguyspr[sprIndex];
    }
}
