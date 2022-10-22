using UnityEngine;

public struct Circle
{
    public Vector2 position;

    public float radius { get; set; }
    public float sqrMagnitude { get; private set; }

    public float x
    {
        get { return position.x; }
        set { position.x = value; }
    }
    public float y
    {
        get { return position.y; }
        set { position.y = value; }
    }

    public Circle(Vector2 position, float radius)
    {
        this.position = position;
        this.radius = radius;
        sqrMagnitude = new Vector2(radius, 0).sqrMagnitude;
    }

    public float top => position.y + radius;
    public float right => position.x + radius;
    public float bottom => position.y - radius;
    public float left => position.x - radius;
}
