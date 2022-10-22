using UnityEngine;

public class Brick : SpriteBase, IFieldObject
{
    public FieldObjectType fieldType { get; private set; }
    public ItemName dropItem;

    public void Initialize(Vector3Int location)
    {
        base.Initialize();
        UpdateLocationAndPosition(location);
        fieldType = FieldObjectType.Brick;
    }

    public void HitFire(int fireID)
    {
        Discard();
        if (dropItem == ItemName.None)
            creater.CreateFireBrick(location, fireID);
        else
            creater.CreateItem(dropItem, location, fireID);
    }
}