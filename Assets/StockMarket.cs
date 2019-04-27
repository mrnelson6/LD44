using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockMarket : MonoBehaviour
{

    public player Player;
    private float perfectEconomySteadyBullGrowth = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float GetMarketRateAmount()
    {
        return perfectEconomySteadyBullGrowth;
    }

    public TransactionResult Buy(string stockKey){
        var amount = this.GetMarketRateAmount();
        var newLiquid = this.Player.CurrentLiquid;
        newLiquid -= amount;
        var newInvested = this.Player.CurrentInvested.ToDictionary(entry => entry.Key,
                                                                   entry => entry.Value);
        newInvested[stockKey] += amount;
        return new TransactionResult(NewLiquid: newLiquid,
                                     NewInvested: newInvested);
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
