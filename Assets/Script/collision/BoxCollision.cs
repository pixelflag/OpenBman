using UnityEngine;

public static class BoxCollision
{
    public static bool PointHitCheck(Vector2 point, Box box)
    {
        return box.bottom < point.y && point.y < box.top &&
               box.left   < point.x && point.x < box.right;
    }

    public static bool BoxHitCheck(Box box1, Box box2)
    {
        return box1.top > box2.bottom && box1.bottom < box2.top &&
               box1.right > box2.left && box1.left < box2.right;
    }

    public static Vector3 CirclePositionCorrection(Circle circle, Box box)
    {
        bool R = box.right < circle.position.x;
        bool L = circle.position.x < box.left;

        bool T = box.top < circle.position.y;
        bool B = circle.position.y < box.bottom;

        if (L && T)
        {
            return CircleCollision.PositionCorrection(circle, box.topLeft);
        }
        else if (R && T)
        {
            return CircleCollision.PositionCorrection(circle, box.topRight);
        }
        else if (L && B)
        {
            return CircleCollision.PositionCorrection(circle, box.bottomLeft);
        }
        else if (R && B)
        {
            return CircleCollision.PositionCorrection(circle, box.bottomRight);
        }
        else if (!L && !R && T)
        {
            if(circle.y < box.top+circle.radius)
                return new Vector2(circle.x, box.top + circle.radius);
        }
        else if (!L && !R && B)
        {
            if (circle.y > box.bottom - circle.radius)
                return new Vector2(circle.x, box.bottom - circle.radius);
        }
        else if (!T && !B && L)
        {
            if (circle.x > box.left - circle.radius)
                return new Vector2(box.left - circle.radius, circle.y);
        }
        else if (!T && !B && R)
        {
            if (circle.x < box.right + circle.radius)
                return new Vector2(box.right + circle.radius, circle.y);
        }

        return circle.position;
    }

    public static bool CircleHitCheck(Circle circle, Box box)
    {
        bool R = box.right < circle.position.x;
        bool L = circle.position.x < box.left;

        bool T = box.top < circle.position.y;
        bool B = circle.position.y < box.bottom;

        if (L && T)
        {
            return CircleCollision.PointHitCheck(circle, box.topLeft);
        }
        else if (R && T)
        {
            return CircleCollision.PointHitCheck(circle, box.topRight);
        }
        else if (L && B)
        {
            return CircleCollision.PointHitCheck(circle, box.bottomLeft);
        }
        else if (R && B)
        {
            return CircleCollision.PointHitCheck(circle, box.bottomRight);
        }
        else if (!L && !R && T)
        {
            return circle.bottom < box.top;
        }
        else if (!L && !R && B)
        {
            return circle.top > box.bottom;
        }
        else if (!T && !B && L)
        {
            return circle.right > box.left;
        }
        else if (!T && !B && R)
        {
            return circle.left < box.right;
        }
        else
        {
            return false;
        }
    }
}