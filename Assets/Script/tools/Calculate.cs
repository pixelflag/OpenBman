using UnityEngine;

public class Calculate : MonoBehaviour
{
    public static float PositionToRadian(Vector3 position, Vector3 targetPosition)
    {
        float radian = Mathf.Atan2(targetPosition.y - position.y, targetPosition.x - position.x);
        if (radian < 0)
        {
            radian = radian + 2 * Mathf.PI;
        }
        return radian;
    }

    public static Vector3 RadianToVector3(float radian, float distance)
    {
        return new Vector3(Mathf.Cos(radian) * distance, Mathf.Sin(radian) * distance, 0);
    }

    public static Vector2 RadianToVector2(float radian, float distance)
    {
        return new Vector3(Mathf.Cos(radian) * distance, Mathf.Sin(radian) * distance, 0);
    }


    public static Vector3Int PositionToLocation(Vector3 position)
    {
        int x = (int)Mathf.Round(position.x / Global.gridSize);
        int y = (int)Mathf.Round(position.y / Global.gridSize);

        return new Vector3Int(x, y, 0);
    }

    public static Vector3 LocationToPosition(Vector3Int location)
    {
        int x = location.x * Global.gridSize;
        int y = location.y * Global.gridSize;

        return new Vector3(x, y, 0);
    }
}
