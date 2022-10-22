using System;
using System.Collections.Generic;

public class ScoreChain
{
    private List<ChainObject> chain;

    private ScoreType[] scoreList =
    {
        ScoreType.Score100,
        ScoreType.Score200,
        ScoreType.Score400,
        ScoreType.Score800,
        ScoreType.Score1000,
        ScoreType.Score2000,
        ScoreType.Score4000,
        ScoreType.Score8000,
        ScoreType.Score10000,
        ScoreType.Score20000,
        ScoreType.Score40000,
        ScoreType.Score80000,
    };

    public ScoreChain()
    {
        chain = new List<ChainObject>();
    }

    public ChainObject Chain(int fireId)
    {
        for (int i = chain.Count - 1; 0 <= i; i--)
        {
            if (chain[i].CheckEnd())
                chain.RemoveAt(i);
        }

        foreach (ChainObject cs in chain)
        {
            if (cs.fireId == fireId)
            {
                cs.Chain();
                return cs;
            }
        }
        ChainObject newChain = new ChainObject(fireId);
        chain.Add(newChain);
        return newChain;
    }

    public ScoreType GetScore(ScoreType score, int chainCount)
    {
        int startIndex = (int)score;
        startIndex += chainCount;
        startIndex = scoreList.Length <= startIndex ? scoreList.Length - 1 : startIndex;

        return scoreList[startIndex];
    }
}

public struct ChainObject
{
    private uint life;
    private uint lifeCount;
    public int chainCount { get; private set; }
    public int fireId { get; private set; }

    public ChainObject(int fireId)
    {
        this.fireId = fireId;
        lifeCount = Global.count;
        life = 30;
        chainCount = 1;
    }

    public bool CheckEnd()
    {
        return life < Global.count - lifeCount;
    }

    public void Chain()
    {
        chainCount++;
        lifeCount = Global.count;
    }
}