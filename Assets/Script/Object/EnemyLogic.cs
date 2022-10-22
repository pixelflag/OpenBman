using UnityEngine;

public class EnemyLogic :DIMonoBehaviour
{
    // 1マスごとにランダムにターンする確率 1/100
    [SerializeField]
    private int RandomTurnRate = 10;

    // ブロックにぶつかったときにターンする確率 1/100
    [SerializeField]
    private int blockHitRandomTurnRate = 10;
    
    // 十字路でランダムにターンする確率 1/100
    [SerializeField]
    private int crossRandomTurnRate = 10;
    
    // プレイヤーにじわじわ寄っていく確率 1/100
    [SerializeField]
    private int playerApproachRate = 50;
    
    // プレイヤーを見つけて方向転換する確率 1/100
    [SerializeField]
    private int playerChaseRate = 50;
    
    // 爆弾を見つけて逃げる確率 1/100
    [SerializeField]
    private int bomEscapeRate = 50;
    
    // プレイヤーや爆弾を見つけられる距離
    [SerializeField]
    private int eyeLength = 3;
    
    // 逃げモードに入ったときの持続フレーム数
    [SerializeField]
    private int escapeWait = 120;

    private int escapeCount = 0;
    private bool isEscape => 0 < escapeCount;

    private Vector3Int direction;
    private Switch gridLocationSwitch;

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        None,
    }

    public void Initialize()
    {
        direction = GetRandomDirection();
        gridLocationSwitch = new Switch();
    }

    public Vector3 Execute(Enemy enemy)
    {
        Vector3Int pLoc = objects.player.location;
        Vector3Int eLoc = enemy.location;
        Vector2Int dist = new Vector2Int(pLoc.x - eLoc.x, pLoc.y - eLoc.y);

        gridLocationSwitch.SetValue(CheckGridEnter(enemy));

        // マスの中心を通過
        if (gridLocationSwitch.isEnter)
        {
            // 十字路にいるか。
            if (LocationIsCross(eLoc))
            {
                Direction pResult = SeachTarget(eLoc, pLoc, eyeLength, enemy.brickPass);

                // 視野内にプレイヤーがいたら追う。
                if (pResult != Direction.None)
                {
                    if (dist.x == 0 || dist.y == 0)
                        if (Lottery(playerChaseRate))
                            direction = DirectionToVector3IntForEscape(pResult, isEscape);
                }
                // プレイヤー側に寄っていく。
                else if (Lottery(playerApproachRate))
                {
                    if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.y))
                    {
                        if (0 < dist.x) direction = DirectionToVector3IntForEscape(Direction.Right, isEscape);
                        if (dist.x < 0) direction = DirectionToVector3IntForEscape(Direction.Left, isEscape);
                    }
                    else
                    {
                        if (0 < dist.y) direction = DirectionToVector3IntForEscape(Direction.Up, isEscape);
                        if (dist.y < 0) direction = DirectionToVector3IntForEscape(Direction.Down, isEscape);
                    }
                }
                // 十字路でランダムにターン
                else if (Lottery(crossRandomTurnRate))
                {
                    direction = GetRandomDirection();
                }
            }
            else
            {
                // 十字路以外でランダムにターン
                if (Lottery(RandomTurnRate))
                    direction = GetRandomDirection();
            }
        }
        // 視野内に爆弾置かれたら逃げる。
        else if (objects.player.isSetBom)
        {
            Direction bResult = SeachTarget(eLoc, pLoc, eyeLength, false);
            if (bResult != Direction.None)
            {
                if (Lottery(bomEscapeRate))
                {
                    escapeCount = escapeWait;
                    direction = DirectionToVector3IntForEscape(bResult, isEscape);
                }
            }
        }

        escapeCount--;

        return direction;
    }

    private Direction SeachTarget(Vector3Int sLoc, Vector3Int tLoc, int eyeLength, bool isThrough)
    {
        if (sLoc.x == tLoc.x)
        {
            if(sLoc.y < tLoc.y)
            {
                if (Seach(sLoc, tLoc, Direction.Up))
                    return Direction.Up;
            }
            else if (tLoc.y < sLoc.y)
            {
                if (Seach(sLoc, tLoc, Direction.Down))
                    return Direction.Down;
            }
        }
        else if (sLoc.y == tLoc.y)
        {
            if (sLoc.x < tLoc.x)
            {
                if (Seach(sLoc, tLoc, Direction.Right))
                    return Direction.Right;
            }
            else if (tLoc.x < sLoc.x)
            {
                if (Seach(sLoc, tLoc, Direction.Left))
                    return Direction.Left;
            }
        }
        return Direction.None;

        bool Seach(Vector3Int sLoc, Vector3Int tLoc, Direction dir)
        {
            for (int i = 0; i < eyeLength; i++)
            {
                sLoc += DirectionToVector3Int(dir);
                if (tLoc == sLoc)
                    return true;
                else if (ExistsObstacle(sLoc, isThrough))
                    return false;
            }
            return false;
        }
    }

    private bool CheckGridEnter(Enemy enemy)
    {
        Vector3 pos = enemy.position;
        Vector3 locPos = Calculate.LocationToPosition(enemy.location);
        int range = 4;

        if (direction.y == 0 && direction.x != 0)
        {
            if (locPos.x - range < pos.x && pos.x < locPos.x + range)
                return true;
        }
        else if (direction.x == 0 && direction.y != 0)
        {
            if (locPos.y - range < pos.y && pos.y < locPos.y + range)
                return true;
        }
        return false;
    }

    public Vector3 BlockHit(Vector3Int location, bool brickPass)
    {
        if (ExistsObstacle(location + direction, brickPass) == true)
        {
            if (Lottery(blockHitRandomTurnRate))
                direction = GetRandomDirection();
            else
                direction = FlipDirction(direction);
        }
        return direction;
    }

    private bool ExistsObstacle(Vector3Int location, bool brickPass)
    {
        switch (field.ExistsType(location))
        {
            case FieldObjectType.Block:
            case FieldObjectType.Bomb:
                return true;
            case FieldObjectType.Brick:
            case FieldObjectType.FireBrick:
                if (brickPass)
                    return false;
                else
                    return true;
        }
        return false;
    }

    private bool Lottery(int rate)
    {
        return Random.Range(0, 100) < rate;
    }

    private bool LocationIsCross(Vector3Int location)
    {
        return location.x % 2 == 1 && location.y % 2 == 1;
    }

    private Vector3Int DirectionToVector3Int(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:    return new Vector3Int( 0, 1, 0);
            case Direction.Right: return new Vector3Int( 1, 0, 0);
            case Direction.Down:  return new Vector3Int( 0,-1, 0);
            case Direction.Left:  return new Vector3Int(-1, 0, 0);
        }
        return new Vector3Int(0, 0, 0);
    }

    private Vector3Int DirectionToVector3IntForEscape(Direction direction, bool isEscape)
    {
        Vector3Int vector = DirectionToVector3Int(direction);
        return isEscape? FlipDirction(vector) : vector;
    }

    private Vector3Int GetRandomDirection()
    {
        switch (Random.Range(0,4))
        {
            case 0: return new Vector3Int( 0, 1, 0);
            case 1: return new Vector3Int( 1, 0, 0);
            case 2: return new Vector3Int( 0,-1, 0);
            case 3: return new Vector3Int(-1, 0, 0);
        }

        throw new System.Exception("random out of range");
    }

    private Vector3Int FlipDirction(Vector3Int direction)
    {
        if (direction.x == 0)
            direction.y = -direction.y;
        if (direction.y == 0)
            direction.x = -direction.x;

        return direction;
    }
}