﻿namespace Liyanjie.EnglishPluralization.Internals;

internal class BidirectionalDictionary<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull
{
    public Dictionary<TFirst, TSecond> FirstToSecondDictionary { get; set; }

    public Dictionary<TSecond, TFirst> SecondToFirstDictionary { get; set; }

    public BidirectionalDictionary()
    {
        this.FirstToSecondDictionary = [];
        this.SecondToFirstDictionary = [];
    }

    public BidirectionalDictionary(Dictionary<TFirst, TSecond> firstToSecondDictionary) : this()
    {
        foreach (TFirst current in firstToSecondDictionary.Keys)
        {
            this.AddValue(current, firstToSecondDictionary[current]);
        }
    }

    public virtual bool ExistsInFirst(TFirst value)
    {
        return this.FirstToSecondDictionary.ContainsKey(value);
    }

    public virtual bool ExistsInSecond(TSecond value)
    {
        return this.SecondToFirstDictionary.ContainsKey(value);
    }

    public virtual TSecond? GetSecondValue(TFirst value)
    {
        if (this.ExistsInFirst(value))
        {
            return this.FirstToSecondDictionary[value];
        }
        return default;
    }

    public virtual TFirst? GetFirstValue(TSecond value)
    {
        if (this.ExistsInSecond(value))
        {
            return this.SecondToFirstDictionary[value];
        }
        return default;
    }

    public void AddValue(TFirst firstValue, TSecond secondValue)
    {
        this.FirstToSecondDictionary.Add(firstValue, secondValue);
        if (!this.SecondToFirstDictionary.ContainsKey(secondValue))
        {
            this.SecondToFirstDictionary.Add(secondValue, firstValue);
        }
    }
}
