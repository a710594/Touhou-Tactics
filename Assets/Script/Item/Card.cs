using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Card
{
    public CardModel Data;
    public Effect Effect;

    public Card() { }

    public Card(CardModel data)
    {
        Data = data;
        Effect = EffectFactory.GetEffect(data.EffectID);
    }
}
