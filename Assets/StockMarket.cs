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
    public Text PlusText;
    public Text MinusText;
    public Image Monitor;
    public Image GraphStartingPoint;
    private List<Image> onScreenGraphLines = new List<Image>();
    private Vector3 startingPositionGraphLines;

    private void Start()
    {
        this.onScreenGraphLines.Add(this.GraphStartingPoint);
        this.startingPositionGraphLines = new Vector3(this.GraphStartingPoint.transform.position.x,
                                                      this.GraphStartingPoint.transform.position.y);
    }

    void Update()
    {
        LiquidText.text = String.Format(  "Liquid Life:   {0} ♡", Player.CurrentLiquid);
        InvestedText.text = String.Format("Invested Life: {0} ♡", Player.CurrentInvested["stock1key"]);
        this.ApplyMoveGraphEffect();      
    }

    private void ApplyMoveGraphEffect()
    {
        var lastDrawnGraphLine = this.onScreenGraphLines.Last<Image>();
        //if(lastDrawnGraphLine.transform.position.x < Monitor.transform.position.x)
        if(Input.GetKey("0")){
            var clone = Instantiate(lastDrawnGraphLine);
            var pos = clone.transform.position;
            //pos.x += 10;
            clone.transform.position = pos;
            clone.transform.SetParent(lastDrawnGraphLine.transform.parent);
            //clone.transform.SetParent();
        
        //    this.onScreenGraphLines.Add(clone);
        }
        var LeftEdgeOfMonitor = (Monitor.transform.position.x - Monitor.rectTransform.sizeDelta.x / 2f);
        foreach (var image in this.onScreenGraphLines) {
            var pos = GraphStartingPoint.transform.position;
            pos.x -= 1;
            GraphStartingPoint.transform.position = pos;
            if (image.transform.position.x < LeftEdgeOfMonitor){
                Destroy(image);
                this.onScreenGraphLines.Remove(image);
            }
        }
        if (GraphStartingPoint.transform.position.x < LeftEdgeOfMonitor)
        {
            Destroy(GraphStartingPoint);
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
