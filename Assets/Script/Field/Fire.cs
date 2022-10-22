using UnityEngine;

public class Fire : SpriteBase, IFieldObject
{
    public int fireID { get; private set; }

    public FieldObjectType fieldType { get; private set; }

    public int life;
    private int spriteIndexHead = 0;

    private AnimCounter anim;

    public void Initialize(Vector3Int location, int fireID)
    {
        base.Initialize();
        UpdateLocationAndPosition(location);

        this.fireID = fireID;
        fieldType = FieldObjectType.Fire;

        int frame = 4;
        int wait = 5;
        anim = new AnimCounter(frame, wait);
        life = frame * wait;
    }

    public override void Execute()
    {
        anim.Execute();
        render.sprite = sprites[spriteIndexHead + anim.frame];

        life--;
        if (life <= 0)
            Discard();
    }

    public void SetAnimationPattern(bool L, bool R, bool T, bool B)
    {
        if (L & R & T & B) Set(1,false,false);
        else if (L & R & T) Set(2,false,false);
        else if (L & R & B) Set(2,false,true);
        else if (L & T & B) Set(3,false, false);
        else if (R & T & B) Set(3, true, false);
        else if (L & R) Set(5, false, false);
        else if (L & T) Set(4, true, false);
        else if (L & B) Set(4, true, true);
        else if (R & T) Set(4, false, false);
        else if (R & B) Set(4, false, true);
        else if (T & B) Set(7, false, false);
        else if (L) Set(6, true, false);
        else if (R) Set(6, false, false);
        else if (T) Set(8, false, false);
        else if (B) Set(8, false, true);

        void Set(int head, bool flipX, bool flipY )
        {
            this.spriteIndexHead = head * anim.totalframe;
            render.flipX = flipX;
            render.flipY = flipY;
        }
    }

    public void HitFire(int fireId)
    {
    }
}
