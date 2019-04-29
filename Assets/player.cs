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
    public Sprite[] downSprAttack;
    public Sprite[] rightSprAttack;
    public Sprite[] leftSprAttack;
    public Sprite[] upSprAttack;
    public Sprite[] leftSpr;
    public Sprite[] rightSpr;
    private int frameCount = 0;
    private int sprIndex = 0;
    private int sprLen = 8;
    public int sprSpd = 5;
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
    public AudioClip gameOverclip;

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
        downSpr = Resources.LoadAll<Sprite>("playerDown");
        downSprAttack = Resources.LoadAll<Sprite>("playerDown_attack");
        rightSprAttack = Resources.LoadAll<Sprite>("playerRight_attack");
        leftSprAttack = Resources.LoadAll<Sprite>("playerLeft_attack");
        upSprAttack = Resources.LoadAll<Sprite>("playerUp_attack");
        leftSpr = Resources.LoadAll<Sprite>("playerLeft");
        rightSpr = Resources.LoadAll<Sprite>("playerRight");
        ow.LoadAudioData();
        oof.LoadAudioData();
        almost.LoadAudioData();
        murder.LoadAudioData();
        get_back.LoadAudioData();
        gameOverclip.LoadAudioData();
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
        if (Input.GetKeyDown("space"))
        {
            attack = true;
        }
        if (Input.GetKeyDown("q"))
        {
            if(!gameOver)
            {
                ass.PlayOneShot(gameOverclip, 0.5f);
            }
            gameOver = true;
        }
        float speed_time = speed * Time.deltaTime;

        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            pos.y += speed * Time.deltaTime;
            if(!attack)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = upSpr[sprIndex];
            }
            up = false;
            down = false;
            right = false;
            left = false;
            up = true;

        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            pos.y -= speed * Time.deltaTime;
            if (!attack)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = downSpr[sprIndex];
            }
            up = false;
            down = false;
            right = false;
            left = false;
            down = true;

        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
 
            if (up)
            {
                pos.y -= speed_time;
                pos.y += Mathf.Sqrt((speed_time * speed_time) / 1.5f);
            }
            if (down)
            {
                pos.y += speed * Time.deltaTime;
                pos.y -= Mathf.Sqrt((speed_time * speed_time) / 1.5f);
            }
            if (up || down)
            {
                pos.x += Mathf.Sqrt((speed_time * speed_time) / 1.5f);
            }
            else
            {
                pos.x += speed * Time.deltaTime;
            }

            if (!attack)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = rightSpr[sprIndex];
            }
            up = false;
            down = false;
            right = false;
            left = false;
            right = true;

        }
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            if (up)
            {
                pos.y -= speed * Time.deltaTime;
                pos.y += Mathf.Sqrt((speed_time * speed_time) / 1.5f);
            }
            if (down)
            {
                pos.y += speed * Time.deltaTime;
                pos.y -= Mathf.Sqrt((speed_time * speed_time) / 1.5f);

            }
            if (up || down)
            {
                pos.x -= Mathf.Sqrt((speed_time * speed_time) / 1.5f);
            }
            else
            {
                pos.x -= speed * Time.deltaTime;
            }
            if (!attack)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = leftSpr[sprIndex];
            }
            up = false;
            down = false;
            right = false;
            left = false;
            left = true;
        }
        if (attack)
        {
            if (up)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = upSprAttack[attackCounter];
            }
            else if (left)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = leftSprAttack[attackCounter];
            }
            else if (right)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = rightSprAttack[attackCounter];
            }
            else if (down)
            {
                spriteObj.GetComponent<SpriteRenderer>().sprite = downSprAttack[attackCounter];
            }
        }
        if(attack)
        {
            attackCounter++;
            if(attackCounter >= 9)
            {
                Vector2 place = cc.offset;
                if (up)
                {
                    place.y = 0.6f;
                }
                if (down)
                {
                    place.y = -0.6f;
                }
                 if (right)
                {
                    place.x = 0.6f;
                } 
                if(left){
                    place.x = -0.6f;
                }
                cc.offset = place;
                //start the attack
            }
            if(attackCounter > 27)
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
