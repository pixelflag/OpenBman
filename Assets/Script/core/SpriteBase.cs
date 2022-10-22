using UnityEngine;

public class SpriteBase : PixelObject
{
    [SerializeField]
    protected Sprite[] sprites;
    [SerializeField]
    protected int collisionSize = 16;

    protected SpriteRenderer render;

    public bool isDestroy { get; private set; }
    public Vector3Int location { get; private set; }

    public void UpdateLocationAndPosition(Vector3Int location)
    {
        this.location = location;
        position = Calculate.LocationToPosition(location);
    }

    public void UpdateLocationWithPosition()
    {
        location = Calculate.PositionToLocation(position);
    }

    public virtual Box GetHitBox()
    {
        return new Box(position, collisionSize / 2, collisionSize / 2);
    }

    public virtual Circle GetHitCircle()
    {
        return new Circle(position, collisionSize / 2);
    }

    public virtual void Execute()
    {
        // please override.
    }

    public virtual void Discard()
    {
        isDestroy = true;
        if (OnDestroy != null) OnDestroy(this);
    }

    public override void Initialize()
    {
        base.Initialize();

        render = GetComponent<SpriteRenderer>();
    }

    public delegate void DestroyDelegate(SpriteBase target);
    public DestroyDelegate OnDestroy;
}
