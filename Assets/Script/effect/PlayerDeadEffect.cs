using UnityEngine;

public class PlayerDeadEffect : SpriteBase
{
    [SerializeField]
    private int wait = 12;

    private AnimCounter anim;

    public override void Initialize()
    {
        base.Initialize();
        anim = new AnimCounter(sprites.Length, wait);
    }

    public override void Execute()
    {
        anim.Execute();
        render.sprite = sprites[anim.frame];
        if(anim.isEnd)
        {
            Discard();
        }
    }
}
