﻿using System;
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
    private float derMarketChangeAggregator = 0f;
    private float currentDerivativeMarketChange = 0f;

    private void Awake()
    {
        this.SMLineGreenGreat = Resources.Load<Sprite>("SMLine1");
        this.SMLineGreenGood = Resources.Load<Sprite>("SMLine2");
        this.SMLineYellowOK = Resources.Load<Sprite>("SMLine3");
        this.SMLineOrangeBad = Resources.Load<Sprite>("SMLine4");
        this.SMLineRedYikes = Resources.Load<Sprite>("SMLine5");
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
        var imageLength = this.StartingGraphLineImage.rectTransform.sizeDelta.x;
        var dxToApply = -1f;
        var dyToApply = 0f;
        var LeftEdgeOfMonitor = (Monitor.transform.position.x - Monitor.rectTransform.sizeDelta.x / 2f) + 20f;
        var TopEdgeOfMonitor = (Monitor.transform.position.y + Monitor.rectTransform.sizeDelta.y / 2f) - 20f;
        var BottomEdgeOfMonitor = (Monitor.transform.position.y - Monitor.rectTransform.sizeDelta.y / 2f) + 20f; 
        if(lastDrawnGraphLine.transform.position.x < (this.startingPositionGraphLines.x - imageLength)){
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
            DestroyImmediate(image);
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
