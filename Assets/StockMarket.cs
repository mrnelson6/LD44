using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockMarket : MonoBehaviour
{
    public AudioSource audioclipbuy;
    public AudioSource audioclipsell;
    public player Player;
    private float perfectEconomySteadyBullGrowth = 1f;

    private float GetMarketRateAmount()
    {
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
        Debug.Log("liquid " + Player.CurrentLiquid);
        Debug.Log("invested " + this.Player.CurrentInvested[stockKey]);
        //return new TransactionResult(NewLiquid: newLiquid,
        //                            NewInvested: newInvested);
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
        Debug.Log("liquid " + Player.CurrentLiquid);
        Debug.Log("invested " + this.Player.CurrentInvested[stockKey]);
        // return new TransactionResult(NewLiquid: newLiquid,
        //                             NewInvested: newInvested);

    }

    public float getLiquid()
    {
        return Player.CurrentLiquid;
    }
}

public class TransactionResult{
    public float NewLiquid;
    public Dictionary<string, float> NewInvested;

    public TransactionResult(float NewLiquid, Dictionary<string, float> NewInvested){
        this.NewLiquid = NewLiquid;
        this.NewInvested = NewInvested;
    }
}
