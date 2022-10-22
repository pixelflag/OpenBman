using UnityEngine;

public class FireBrick : SpriteBase, IFieldObject
{
    public FieldObjectType fieldType { get; private set; }
    public int fireID { get; private set; }

    private int life = 20;
    private AnimCounter anim;

    public void Initialize(Vector3Int location, int fireID)
    {
        base.Initialize();
        UpdateLocationAndPosition(location);

        this.fireID = fireID;
        fieldType = FieldObjectType.FireBrick;

        int animWait = 5;
        anim = new AnimCounter(sprites.Length, animWait);
        life = sprites.Length * animWait;
    }

    public override void Execute()
    {
        if (isDestroy) return;

        life--;
        anim.Execute();
        render.sprite = sprites[anim.frame];
        if (life <= 0)
            Discard();
    }

    public void HitFire(int fireID)
    {
    }
}

