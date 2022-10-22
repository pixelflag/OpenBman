using UnityEngine;

public struct Box
{
    public Vector2 position;
    public int extendsX;
    public int extendsY;

    public Box(Vector2 position, int extendsX, int extendsY)
    {
        this.position = position;
        this.extendsX = extendsX;
        this.extendsY = extendsY;
    }

    public float top    => position.y + extendsY;
    public float right  => position.x + extendsX;
    public float bottom => position.y - extendsY;
    public float left   => position.x - extendsX;

    public Vector2 topLeft => new Vector2(left, top);
    public Vector2 topRight => new Vector2(right, top);
    public Vector2 bottomLeft => new Vector2(left, bottom);
    public Vector2 bottomRight => new Vector2(right, bottom);

    public float x
    {
        get{ return position.x; }
        set{ position.x = value; }
    }
    public float y
    {
        get{ return position.y; }
        set{ position.y = value; }
    }
}
