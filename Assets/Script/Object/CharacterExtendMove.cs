using UnityEngine;

// ブロックをかわすように動く。
public static class CharacterExtendMove
{
    public static Vector2 ExtendMove(Field field, bool brickPass, bool bombPass, Vector2 vector, Vector3 position, Vector3Int location)
    {
        Vector3 locPos = Calculate.LocationToPosition(location);
        if (vector.y == 0)
        {
            if (0 < vector.x)
            {
                if (ExistsBlock(1, 0))
                {
                    if (locPos.y + 4 < position.y && !ExistsBlock(1, 1))
                        vector.y = 1;
                    if (locPos.y - 4 > position.y && !ExistsBlock(1,-1))
                        vector.y = -1;
                }
                else
                {
                    if (locPos.y + 2 < position.y)
                        vector.y = -1;
                    if (locPos.y - 2 > position.y)
                        vector.y = 1;
                }
            }
            if (vector.x < 0)
            {
                if (ExistsBlock(-1, 0))
                {
                    if (locPos.y + 4 < position.y && !ExistsBlock(-1, 1))
                        vector.y = 1;
                    if (locPos.y - 4 > position.y && !ExistsBlock(-1,-1))
                        vector.y = -1;
                }
                else
                {
                    if (locPos.y + 2 < position.y)
                        vector.y = -1;
                    if (locPos.y - 2 > position.y)
                        vector.y = 1;
                }
            }
        }

        if (vector.x == 0)
        {
            if (0 < vector.y)
            {
                if (ExistsBlock(0, 1))
                {
                    if (locPos.x + 4 < position.x && !ExistsBlock(1, 1))
                        vector.x = 1;
                    if (locPos.x - 4 > position.x && !ExistsBlock(-1, 1))
                        vector.x = -1;
                }
                else
                {
                    if (locPos.x + 2 < position.x)
                        vector.x = -1;
                    if (locPos.x - 2 > position.x)
                        vector.x = 1;
                }
            }
            if (vector.y < 0)
            {
                if (ExistsBlock(0, -1))
                {
                    if (locPos.x + 4 < position.x && !ExistsBlock(1, -1))
                        vector.x = 1;
                    if (locPos.x - 4 > position.x && !ExistsBlock(-1, -1))
                        vector.x = -1;
                }
                else
                {
                    if (locPos.x + 2 < position.x)
                        vector.x = -1;
                    if (locPos.x - 2 > position.x)
                        vector.x = 1;
                }
            }
        }

        return vector;

        bool ExistsBlock(int ox, int oy)
        {
            Vector3Int loc = location + new Vector3Int(ox, oy, 0);

            switch (field.ExistsType(loc))
            {
                case FieldObjectType.Block:
                    return true;
                case FieldObjectType.Brick:
                case FieldObjectType.FireBrick:
                    if (brickPass)
                        return false;
                    else
                        return true;
                case FieldObjectType.Bomb:
                    if (bombPass)
                        return false;
                    else
                        return true;
            }
            return false;
        }
    }
}
