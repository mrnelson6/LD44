using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StockMarket : MonoBehaviour
{
    public AudioClip audioclipbuy;
    public AudioClip audioclipsell;
      public AudioClip shoes;
    public AudioClip savings;
      public AudioClip my_wife;
    public AudioClip moving_up;
    public AudioClip intro;
    public AudioClip bell;
    public GameObject ambient;
    private AudioSource bckg;

    public player Player;
    private float defaultUnityToBuy = 2f;
    public Text LiquidText;
    public Text InvestedText;
    public Text TotalText;
    public Image InvestedHeart;
    public Image InvestedGreenArrow;
    public Image InvestedRedArrow;
    public Image LiquidHeart;
    public Image LiquidGreenArrow;
    public Image LiquidRedArrow;
    public Image Monitor;
    public Image StartingGraphLineImage;

    private Sprite SMLineGreenGreat;
    private Sprite SMLineGreenGood;
    private Sprite SMLineYellowOK;
    private Sprite SMLineOrangeBad;
    private Sprite SMLineRedYikes;

    private List<Image> onScreenGraphLines = new List<Image>();
    private Vector3 startingPositionGraphLines;
    private Quaternion startingRotationGraphLines;
    private float prevInvested = 50f;
    private float prevLiquid = 50f;
    private float derMarketChangeAggregator = 0f;

    private bool currentlyAnimatingBuyEffect = false;
    private int BuyAnimateNumFramesAggr = 0;
    private bool currentlyAnimatingSellEffect = false;
    private int SellAnimateNumFramesAggr = 0;
    private int numFramesToApplyEffect = 20;
    private Vector3 InitialLiquidHeartPos;
    private Vector3 InitialInvestedHeartPos;
    private AudioSource ass;
    private float imageLength;
    private int audioDone;
    private bool prevAudio;
    public bool start;
    public bool skip;

    
    private float currentDerivativeMarketChange = 0f;

    private void Awake()
    {
        this.SMLineGreenGreat = Resources.Load<Sprite>("SMLine1");
        this.SMLineGreenGood = Resources.Load<Sprite>("SMLine2");
        this.SMLineYellowOK = Resources.Load<Sprite>("SMLine3");
        this.SMLineOrangeBad = Resources.Load<Sprite>("SMLine4");
        this.SMLineRedYikes = Resources.Load<Sprite>("SMLine5");
        audioclipbuy.LoadAudioData();
        audioclipsell.LoadAudioData();
    }

    private void Start()
    {
        this.onScreenGraphLines.Add(this.StartingGraphLineImage);
        this.startingPositionGraphLines = new Vector3(this.StartingGraphLineImage.transform.position.x,
                                                      this.StartingGraphLineImage.transform.position.y);
        this.startingRotationGraphLines = new Quaternion(this.StartingGraphLineImage.transform.rotation.x,
            this.StartingGraphLineImage.transform.rotation.y,
            this.StartingGraphLineImage.transform.rotation.z,
            this.StartingGraphLineImage.transform.rotation.w);
        this.InitialLiquidHeartPos = new Vector3(this.LiquidHeart.transform.position.x,
                                                   this.LiquidHeart.transform.position.y);
        this.InitialInvestedHeartPos = new Vector3(this.InvestedHeart.transform.position.x,
                                                   this.InvestedHeart.transform.position.y);
        ass = GetComponent<AudioSource>();
        bckg = ambient.GetComponent<AudioSource>();
        audioDone = 0;
        start = false;
        skip = false;
        prevAudio = false;
        ass.PlayOneShot(intro, 1.0f);
        this.UpdateLiquidAndInvestedTextAndImage();
        this.imageLength = this.StartingGraphLineImage.rectTransform.sizeDelta.x;
        InvokeRepeating("UpdateLiquidAndInvestedTextAndImage", 1f, 1f);
    }

    void Update()
    {
        if(Input.GetKey("space") && !skip)
        {
            audioDone = 3;
            bckg.Play();
            ass.Stop();
            start = true;
            skip = true;
        }
        if (ass.isPlaying == false && prevAudio == true)
        {
            audioDone++;
            if(audioDone == 1)
            {
                ass.PlayOneShot(bell, 0.5f);
                start = true;
            }
            else if(audioDone == 2)
            {
                bckg.Play();
            }
        }
        prevAudio = ass.isPlaying;
        if (!Player.gameOver && start)
        {
            this.UpdateMarketChange();
            this.ApplyMoveGraphEffect();
            this.ApplyMarketChangeToPlayer();
            this.AnimateBuyEffect();
            this.AnimateSellEffect();
        }
    }

    void UpdateLiquidAndInvestedTextAndImage()
    {
        LiquidText.text = String.Format("{0:0}", Player.CurrentLiquid);
        InvestedText.text = String.Format("{0:0}", Player.CurrentInvested["stock1key"]);
        TotalText.text = String.Format("{0:0}", Player.CurrentLiquid + Player.CurrentInvested["stock1key"]);
        this.InvestedRedArrow.enabled = false;
        this.InvestedGreenArrow.enabled = false;
        this.LiquidGreenArrow.enabled = false;
        this.LiquidRedArrow.enabled = false;
        if(this.Player.CurrentInvested["stock1key"] < this.prevInvested)
        {
            this.InvestedRedArrow.enabled = true;
        }
        if(this.Player.CurrentInvested["stock1key"] > this.prevInvested)
        {
            this.InvestedGreenArrow.enabled = true;
        }
        if(this.Player.CurrentLiquid < this.prevLiquid)
        {
            this.LiquidRedArrow.enabled = true;
        }
        if(this.Player.CurrentLiquid > this.prevLiquid)
        {
            this.LiquidGreenArrow.enabled = true;
        }
        this.prevInvested = Player.CurrentInvested["stock1key"];
        this.prevLiquid = Player.CurrentLiquid;
    }

    private void ApplyMarketChangeToPlayer(){
        if (this.Player.CurrentInvested["stock1key"] <= 0f)
        {
            this.Player.CurrentInvested["stock1key"] = 0f;
        }
        else
        {
            this.Player.CurrentInvested["stock1key"] += this.currentDerivativeMarketChange;
        }
    }

    private void UpdateMarketChange()
    {
        this.currentDerivativeMarketChange = (float)UnityEngine.Random.Range(-1f, 1f);
    }

    private void ApplyMoveGraphEffect()
        ///Super hacky but it workz
    {

        this.derMarketChangeAggregator += this.currentDerivativeMarketChange;
        var lastDrawnGraphLine = this.onScreenGraphLines.Last<Image>();
        var dxToApply = -1f;
        var dyToApply = 0f;
        var LeftEdgeOfMonitor = (Monitor.transform.position.x - Monitor.rectTransform.sizeDelta.x / 2f) + 20f;
        var TopEdgeOfMonitor = (Monitor.transform.position.y + Monitor.rectTransform.sizeDelta.y / 2f) - 20f;
        var BottomEdgeOfMonitor = (Monitor.transform.position.y - Monitor.rectTransform.sizeDelta.y / 2f) + 20f; 
        if(lastDrawnGraphLine.transform.position.x < (this.startingPositionGraphLines.x - this.imageLength)){
            var clonePos = new Vector3(this.startingPositionGraphLines.x,
                                       lastDrawnGraphLine.transform.position.y);
            clonePos.y += this.derMarketChangeAggregator;
            var clone = Instantiate(lastDrawnGraphLine, clonePos, this.startingRotationGraphLines);
            clone.transform.Rotate(new Vector3(0, 1, 1), this.derMarketChangeAggregator * 5f);
            var pos = clone.transform.position;
            clone.transform.position = pos;
            clone.transform.SetParent(lastDrawnGraphLine.transform.parent);
            this.onScreenGraphLines.Add(clone);
            if(clone.transform.position.y > TopEdgeOfMonitor)
            {
                dyToApply = -10f;
            }
            if(clone.transform.position.y < BottomEdgeOfMonitor)
            {
                dyToApply = 10f;
            }

            //Color section
            var greatThresh = 4f;
            var goodThresh = 2f;
            var okThresh = -2;
            var badThresh = -4f;
            if (this.derMarketChangeAggregator >= greatThresh)
            {
                clone.sprite = this.SMLineGreenGreat;
                int chance = UnityEngine.Random.Range(0, 10);
                if (chance == 0 && !ass.isPlaying)
                {
                    ass.PlayOneShot(moving_up, 0.5f);
                }
                else if (chance == 1 && !ass.isPlaying)
                {
                    ass.PlayOneShot(shoes, 0.5f);
                }
            } else if(derMarketChangeAggregator < greatThresh && 
                      derMarketChangeAggregator >= goodThresh){
                clone.sprite = this.SMLineGreenGood;
            } else if(derMarketChangeAggregator < goodThresh && 
                      derMarketChangeAggregator >= okThresh){
                clone.sprite = this.SMLineYellowOK;
            } else if (derMarketChangeAggregator < okThresh && 
                       derMarketChangeAggregator >= badThresh)
            {
                clone.sprite = this.SMLineOrangeBad;
            } else if (derMarketChangeAggregator < badThresh)
            {
                int chance = UnityEngine.Random.Range(0, 10);
                if(chance == 0 && !ass.isPlaying)
                {
                    ass.PlayOneShot(savings, 0.5f);
                } else if (chance == 1 && !ass.isPlaying)
                {
                    ass.PlayOneShot(my_wife, 0.5f);
                }
                clone.sprite = this.SMLineRedYikes;
            } else
            {
                clone.sprite = this.SMLineYellowOK;
            }
            //end color section

            this.derMarketChangeAggregator = 0f;
        }
        var imagesToRemove = new List<Image>();
        foreach (var image in this.onScreenGraphLines) {
            var pos = image.transform.position;
            pos.x += dxToApply;
            pos.y += dyToApply;
            image.transform.position = pos;
            if (image.transform.position.x < LeftEdgeOfMonitor){
                imagesToRemove.Add(image);
           }
        }
        foreach(var image in imagesToRemove)
        {
            this.onScreenGraphLines.Remove(image);
            DestroyImmediate(image.gameObject);
        }

    }

    private float GetMarketRateAmount(){
        return defaultUnityToBuy;
    }

    public void Buy(string stockKey)
    {
        var amount = this.GetMarketRateAmount();
        if(this.Player.CurrentLiquid > amount)
        {
            if (!ass.isPlaying)
            {
                ass.PlayOneShot(audioclipbuy, 0.5f);
            }
            this.Player.CurrentLiquid -= amount;
            this.Player.CurrentInvested[stockKey] += amount;
        }
        if (!this.currentlyAnimatingBuyEffect)
        {
            this.TriggerAnimateBuyEffect();
        }
    }

    public void TriggerAnimateBuyEffect()
    {
        this.currentlyAnimatingBuyEffect = true;
        this.BuyAnimateNumFramesAggr = 0;
        this.InitialLiquidHeartPos = this.LiquidHeart.transform.position;
    }

    public void AnimateBuyEffect()
    {
        if (this.currentlyAnimatingBuyEffect)
        {
            this.BuyAnimateNumFramesAggr++;
            if (this.BuyAnimateNumFramesAggr > this.numFramesToApplyEffect)
            {
                this.currentlyAnimatingBuyEffect = false;
                this.LiquidHeart.transform.position = this.InitialLiquidHeartPos;
            }
            else
            {
                var pos = this.LiquidHeart.transform.position;
                pos.y -= 3;
                this.LiquidHeart.transform.position = pos;
            }
        }
    }

    public void Sell(string stockKey)
    {
        var amount = this.GetMarketRateAmount();
        if (this.Player.CurrentInvested[stockKey] >= amount)
        {
            this.Player.CurrentLiquid += amount;
            this.Player.CurrentInvested[stockKey] -= amount;
            if(!ass.isPlaying)
            {
                ass.PlayOneShot(audioclipsell, 0.5f);
            }
        }
        if (!this.currentlyAnimatingSellEffect)
        {
            this.TriggerAnimateSellEffect();
        }

    }

    public void TriggerAnimateSellEffect()
    {
        this.currentlyAnimatingSellEffect = true;
        this.SellAnimateNumFramesAggr = 0;
        this.InitialInvestedHeartPos = this.InvestedHeart.transform.position;

    }

    public void AnimateSellEffect()
    {
        if (this.currentlyAnimatingSellEffect)
        {
            this.SellAnimateNumFramesAggr++;
            if (this.SellAnimateNumFramesAggr > this.numFramesToApplyEffect)
            {
                this.currentlyAnimatingSellEffect = false;
                this.InvestedHeart.transform.position = this.InitialInvestedHeartPos;
            }
            else
            {
                var pos = this.InvestedHeart.transform.position;
                pos.y += 3;
                this.InvestedHeart.transform.position = pos;
            }

        }
        }

    public float getLiquid()
    {
        return Player.CurrentLiquid;
    }
}
