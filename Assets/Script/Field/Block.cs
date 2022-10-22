using UnityEngine;

public class Block : SpriteBase, IFieldObject
{
    public FieldObjectType fieldType { get; private set; }

    public void Initialize( Vector3Int location)
    {
        base.Initialize();
        UpdateLocationAndPosition(location);
        fieldType = FieldObjectType.Block;
    }

    public void HitFire(int fireID)
    {
    }
}

