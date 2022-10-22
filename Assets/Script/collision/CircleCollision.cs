using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CircleCollision
{
    public static Vector2 PositionCorrection(Circle circle, Vector2 target)
    {
        if (PointHitCheck(circle, target))
        {
            float radian = Calculate.PositionToRadian(target, circle.position);
            return Calculate.RadianToVector2(radian, circle.radius) + target;
        }
        return circle.position;
    }

    public static Vector2 PositionCorrection(Circle circle, Circle target)
    {
        if (CircleHitCheck(circle, target))
        {
            float radian = Calculate.PositionToRadian(circle.position, target.position);
            return Calculate.RadianToVector2(radian, circle.radius + target.radius) + target.position;
        }
        return circle.position;
    }

    public static bool CircleHitCheck(Circle circle, Circle target)
    {
        float dist = (circle.position - target.position).SqrMagnitude();
        float radius = circle.sqrMagnitude + target.sqrMagnitude;
        return dist < radius;
    }

    public static bool PointHitCheck(Circle circle, Vector2 target)
    {
        float dist = (circle.position - target).SqrMagnitude();
        return dist < circle.sqrMagnitude;
    }
}