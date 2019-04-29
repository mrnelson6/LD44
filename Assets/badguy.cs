using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badguy : MonoBehaviour
{
    private float speed = 0.09f;
    public player target;
    public int drawOrder;
    private bool follow;
    private bool meander;
    private int counter;
    private float meanderx;
    private float meandery;
    private int sprIndex = 0;
    private float direction;
    public GameObject spriteObj;
    public GameObject empty;
    private bool attack;
    private int attackCounter;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Sprite[] upSpr;
    public Sprite[] downSpr;
    public Sprite[] leftSpr;
    public Sprite[] rightSpr;
    private int frameCount;

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
        meanderx = (Random.Range(-20.0f, 3.0f));
        meandery = (Random.Range(-20.0f, 20.0f));
        frameCount = 0;
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

    void Awake()
    {
        upSpr = Resources.LoadAll<Sprite>("enemyUp");
        downSpr = Resources.LoadAll<Sprite>("enemyDown");
        leftSpr = Resources.LoadAll<Sprite>("enemyLeft");
        rightSpr = Resources.LoadAll<Sprite>("enemyRight");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Attack"&& target.attackCounter > 15 )
        {
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

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (target.gameOver)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (!follow)
            {
                if (meander)
                {
                    Vector3 pos = transform.position;
                    Vector3 dest = pos;
                    dest.x = meanderx;
                    dest.y = meandery;
                    Vector3 temp;
                    temp.x = meanderx;
                    temp.y = meandery;
                    temp.z = 0;
                    temp.Normalize();
                    direction = Vector3.SignedAngle(temp, Vector3.right, Vector3.forward);
                    pos = Vector3.MoveTowards(pos, dest, speed);
                    transform.position = pos;
                    Vector3 distance = pos - dest;
                    int chance = Random.Range(0, 10000);
                    if (distance.magnitude < 3.0f || chance == 1)
                    {
                        follow = true;
                        counter = 1000;
                        meander = false;
                    }
                }
                else
                {
                    Vector3 pos = transform.position;
                    meanderx = Random.Range(-20.0f, 3.0f);
                    meandery = Random.Range(-20.0f, 20.0f);
                    meander = true;
                }
            }
            else
            {
                counter--;
                if (counter <= 0)
                {
                    follow = false;
                    meander = false;
                }
                GetComponentInChildren<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y) + drawOrder;
                Vector3 goodguy;
                if (target == null)
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
                pos = Vector3.MoveTowards(pos, goodguy, speed);
                transform.position = pos;
                direction = Vector3.SignedAngle(directionVec, Vector3.right, Vector3.forward);
            }
            if (frameCount % 5 == 0)
            {
                sprIndex = (sprIndex + 1) % 8;
            }
            if (direction < -135 || direction > 135)
            {
                sprIndex = 4;
                GetComponent<SpriteRenderer>().sprite = leftSpr[sprIndex];
            }
            else if (direction <= 135 && direction > 45)
            {
                GetComponent<SpriteRenderer>().sprite = downSpr[sprIndex];
                sprIndex = 0;
            }
            else if (direction <= 45 && direction > -45)
            {
                GetComponent<SpriteRenderer>().sprite = rightSpr[sprIndex];
                sprIndex = 8;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = upSpr[sprIndex];
                sprIndex = 12;
            }
            if (attack)
            {
                attackCounter++;
                //Used for animating atttacks
                //if (attackCounter < 8)
                //{
                //    sprIndex++;
                //}
                //else if (attackCounter < 15)
                //{
                //    sprIndex += 2;
                //}
                //else 
                if (attackCounter < 20)
                {
                    //sprIndex += 3;
                    if (attackCounter == 18)
                    {
                        target.damage();
                    }
                }
                else if (attackCounter > 27)
                {
                    attackCounter = 0;
                }
            }
            else
            {
                attackCounter = 0;
            }
            //spriteObj.GetComponent<SpriteRenderer>().sprite = badguyspr[sprIndex];
        }
    }
}
