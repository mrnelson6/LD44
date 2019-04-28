using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed = 25.0f;
    public int drawOrder;
    public Rigidbody2D rb;
    public float CurrentLiquid = 50f;
    public Dictionary<string, float> CurrentInvested = new Dictionary<string, float>(){["stock1key"] = 50f };
    public GameObject spriteObj;
    public Sprite[] upSpr;
    public Sprite[] downSpr;
    public Sprite[] leftSpr;
    public Sprite[] rightSpr;
    private int frameCount = 0;
    private int sprIndex = 0;
    private int sprLen = 8;
    public int sprSpd = 5;
    private int sprLenAttack = 28;
    public int sprSpdAttack = 2;
    private int sprIndexAttack = 0;
    private AudioSource ass;
    public bool gameOver;

    public int attackCounter;
    private bool attack;
    private bool up;
    private bool down;
    private bool right;
    private bool left;
    public AudioClip ow;
    public AudioClip oof;
    public AudioClip almost;
    public AudioClip murder;
    public AudioClip get_back;

    public CircleCollider2D cc;
    // Start is called before the first:w frame update
    void Start()
    {
        rb.freezeRotation = true;
        ass = GetComponent<AudioSource>();
        gameOver = false;
        attackCounter = 0;
        attack = false;
        up = false;
        down = false;
        right = false;
        left = false;
        cc = GetComponentInChildren<CircleCollider2D>();
    }

    void Awake()
    {
        upSpr = Resources.LoadAll<Sprite>("playerUp");
        downSpr = Resources.LoadAll<Sprite>("playerDown_attack");
        leftSpr = Resources.LoadAll<Sprite>("playerLeft");
        rightSpr = Resources.LoadAll<Sprite>("playerRight");
        ow.LoadAudioData();
        oof.LoadAudioData();
        almost.LoadAudioData();
        murder.LoadAudioData();
        get_back.LoadAudioData();
    }

    // Update is called once per frame
    void Update()
    {
        //sprite frame animation
        if (frameCount % sprSpd == 0)
        {
            sprIndex = (sprIndex + 1) % sprLen;
        }
        //sprite frame animation for attacks
        if (frameCount % sprSpdAttack == 0)
        {
            sprIndexAttack = (sprIndexAttack + 1) % sprLenAttack;
        }

        GetComponentInChildren<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y) + drawOrder;

        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            pos.y += speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = upSpr[sprIndex];
            up = true;
            down = false;
            right = false;
            left = false;
        }
        if (Input.GetKey("s"))
        {
            pos.y -= speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = downSpr[sprIndex];
            up = false;
            down = true;
            right = false;
            left = false;
            spriteObj.GetComponent<SpriteRenderer>().sprite = downSpr[sprIndexAttack];
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = rightSpr[sprIndex];
            up = false;
            down = false;
            right = true;
            left = false;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
            spriteObj.GetComponent<SpriteRenderer>().sprite = leftSpr[sprIndex];
            up = false;
            down = false;
            right = false;
            left = true;
        }
        if(Input.GetKeyDown("space"))
        {
            attack = true;
        }
        if(attack)
        {
            attackCounter++;
            if(attackCounter >= 15)
            {
                Vector2 place = cc.offset;
                if (up)
                {
                    place.y = 1;
                }
                if (down)
                {
                    place.y = -1;
                }
                 if (right)
                {
                    place.x = 1;
                } 
                if(left){
                    place.y = -1;
                }
                cc.offset = place;
                //start the attack
            }
            if(attackCounter > 25)
            {
                attack = false;
                attackCounter = 0;
            }
        }
        transform.position = pos;
        frameCount++;
    }

    public void almost_damage()
    {
        if(!ass.isPlaying)
        {
            int rng = Random.Range(0, 8);
            if (rng == 0)
            {
                ass.PlayOneShot(get_back, 0.5f);
            }
            else if (rng == 1)
            {
                ass.PlayOneShot(murder, 0.5f);
            }
            else if (rng == 2)
            {
                ass.PlayOneShot(almost, 0.5f);
            }
        }
    }

    public void damage()
    {
        CurrentLiquid--;
        if (CurrentLiquid == 0)
        {
            gameOver = true;
        }
        else
        {
            if (!ass.isPlaying)
            {
                int rng = Random.Range(0, 8);
                if (rng == 0)
                {
                    ass.PlayOneShot(oof, 0.5f);
                }
                else if (rng == 1)
                {
                    ass.PlayOneShot(ow, 0.5f);
                }
            }
        }
    }
}
