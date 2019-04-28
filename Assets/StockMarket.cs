using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StockMarket : MonoBehaviour
{
    public AudioSource audioclipbuy;
    public AudioSource audioclipsell;
    public player Player;
    private float perfectEconomySteadyBullGrowth = 1f;
    public Text LiquidText;
    public Text InvestedText;
    public Image GreenArrow;
    public Image RedArrow;
    public Image Monitor;
    public Image GraphLineImage;
    private List<Image> onScreenGraphLines = new List<Image>();
    private Vector3 startingPositionGraphLines;
    private Quaternion startingRotationGraphLines;
    private float currentDerivativeMarketChange = 0f;
    private float prevInvested = 50f;

    private void Start()
    {
        this.onScreenGraphLines.Add(this.GraphLineImage);
        this.startingPositionGraphLines = new Vector3(this.GraphLineImage.transform.position.x,
                                                      this.GraphLineImage.transform.position.y);
        this.startingRotationGraphLines = new Quaternion(this.GraphLineImage.transform.rotation.x,
            this.GraphLineImage.transform.rotation.y,
            this.GraphLineImage.transform.rotation.z,
            this.GraphLineImage.transform.rotation.w);

        this.UpdateLiquidAndInvestedTextAndImage();
        InvokeRepeating("UpdateLiquidAndInvestedTextAndImage", 1f, 1f);
    }

    void Update()
    {
        this.UpdateMarketChange();
        this.ApplyMoveGraphEffect();
        this.ApplyMarketChangeToPlayer();
    }

    void UpdateLiquidAndInvestedTextAndImage()
    {
        LiquidText.text = String.Format("Liquid Life:   {0:0} ♡", Player.CurrentLiquid);
        InvestedText.text = String.Format("Invested Life: {0:0} ♡", Player.CurrentInvested["stock1key"]);
        this.RedArrow.enabled = false;
        this.GreenArrow.enabled = false;
        if(this.Player.CurrentInvested["stock1key"] < this.prevInvested)
        {
            this.RedArrow.enabled = true;
        }
        if(this.Player.CurrentInvested["stock1key"] > this.prevInvested)
        {
            this.GreenArrow.enabled = true;
        }
        this.prevInvested = Player.CurrentInvested["stock1key"];
    }

    private void ApplyMarketChangeToPlayer(){
        this.Player.CurrentInvested["stock1key"] += this.currentDerivativeMarketChange;
    }

    private void UpdateMarketChange()
    {

        this.currentDerivativeMarketChange = (float)UnityEngine.Random.Range(-1f, 1f);
    }

    private void ApplyMoveGraphEffect()
        ///Super hacky but it workz
    {
        var lastDrawnGraphLine = this.onScreenGraphLines.Last<Image>();
        var imageLength = this.GraphLineImage.rectTransform.sizeDelta.x;
        if(lastDrawnGraphLine.transform.position.x < (this.startingPositionGraphLines.x - imageLength)){
            var clonePos = new Vector3(this.startingPositionGraphLines.x,
                                       lastDrawnGraphLine.transform.position.y);
            clonePos.y += this.currentDerivativeMarketChange * 3f;
            //var cloneRotation = lastDrawnGraphLine.transform.Rotate(new Vector3(0, 1, 1), 20f);
            var clone = Instantiate(lastDrawnGraphLine, clonePos, this.startingRotationGraphLines);
            clone.transform.Rotate(new Vector3(0, 1, 1), this.currentDerivativeMarketChange * 10f);
            var pos = clone.transform.position;
            //pos.x += 10;
            clone.transform.position = pos;
            clone.transform.SetParent(lastDrawnGraphLine.transform.parent);
            this.onScreenGraphLines.Add(clone);
            //clone.transform.SetParent();
        
        //    this.onScreenGraphLines.Add(clone);
        }
        var LeftEdgeOfMonitor = (Monitor.transform.position.x - Monitor.rectTransform.sizeDelta.x / 2f) + 20f;
        foreach (var image in this.onScreenGraphLines) {
            var pos = image.transform.position;
            pos.x -= 1;
            image.transform.position = pos;
            if (image.transform.position.x < LeftEdgeOfMonitor){
                Destroy(image);
                this.onScreenGraphLines.Remove(image);
            }
        }
    }

    private float GetMarketRateAmount(){
        return perfectEconomySteadyBullGrowth;
    }

    public void Buy(string stockKey)
    {
        var amount = this.GetMarketRateAmount();
        if(this.Player.CurrentLiquid > amount)
        {
            audioclipbuy.PlayScheduled(0.5);
            this.Player.CurrentLiquid -= amount;
            this.Player.CurrentInvested[stockKey] += amount;
        }
    }

    public void Sell(string stockKey)
    {
        var amount = this.GetMarketRateAmount();
        if (this.Player.CurrentInvested[stockKey] >= amount)
        {
            this.Player.CurrentLiquid += amount;
            this.Player.CurrentInvested[stockKey] -= amount;
            audioclipsell.PlayScheduled(0.5);
        }

    }

    public float getLiquid()
    {
        return Player.CurrentLiquid;
    }
}
