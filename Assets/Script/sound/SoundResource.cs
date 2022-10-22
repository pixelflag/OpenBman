using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create SoundResource", fileName = "SoundResource")]
public class SoundResource : ScriptableObject
{
    public AudioClip[] SE;
    public AudioClip[] BGM;

    public AudioClip GetEffect(SeType type)
    {
        switch (type)
        {
            case SeType.SetBom:return SE[0];
            case SeType.WalkA:return SE[1];
            case SeType.WalkB:return SE[2];
            case SeType.Explose: return SE[4];
            case SeType.ItemGet:return SE[5];
            case SeType.Dead: return SE[6];
        }
        return null;
    }

    public AudioClip GetBgm(BgmType type)
    {
        switch (type)
        {
            case BgmType.StageStart: return BGM[0];
            case BgmType.Normal: return BGM[1];
            case BgmType.PowerUp: return BGM[1];
            case BgmType.Special: return BGM[1];
            case BgmType.StageClear: return BGM[3];
            case BgmType.Dead: return BGM[4];
            case BgmType.GameOver: return BGM[5];
            case BgmType.AllKill: return BGM[6];
        }
        return null;
    }
}

public enum SeType
{
    SetBom,
    WalkA,
    WalkB,
    Explose,
    ItemGet,
    Dead,
}

public enum BgmType
{
    None,
    StageStart,
    Normal,
    PowerUp,
    Special,
    StageClear,
    Dead,
    GameOver,
    AllKill,
}

public enum SoundChannel
{
    Explose,
    SetBom,
    Walk,
}