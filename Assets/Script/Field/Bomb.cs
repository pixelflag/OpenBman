using UnityEngine;

public class Bomb : SpriteBase, IFieldObject
{
    private static int fireIdCount;

    public FieldObjectType fieldType { get; private set; }

    [SerializeField]
    private int life = 128;
    private int count = 0;

    private AnimCounter anim;

    private bool isIgnition ;
    private int ignitionWait = 4;
    private int ignitionCount = 0;

    private int fireId;

    public void Initialize(Vector3Int location)
    {
        base.Initialize();
        UpdateLocationAndPosition(location);

        fireIdCount++;
        fireId = fireIdCount;
        fieldType = FieldObjectType.Bomb;

        anim = new AnimCounter(4, 12);

        isIgnition = false;
    }

    public override void Execute()
    {
        anim.Execute();
        render.sprite = sprites[anim.frame];

        if (isIgnition)
            ignitionCount++;

        if(!data.useRemocon)
            count++;

        if (life <= count || ignitionWait < ignitionCount)
            Explose();
    }

    public void Explose()
    {
        if (isDestroy) return;
        creater.CreateExplosion(location, fireId);
        Discard();
    }

    public void HitFire(int fireId)
    {
        this.fireId = fireId;
        isIgnition = true;
    }
}
