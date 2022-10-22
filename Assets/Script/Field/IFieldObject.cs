using UnityEngine;

public interface IFieldObject
{
    FieldObjectType fieldType { get; }
    Vector3 position { get; }
    Vector3Int location { get; }
    GameObject gameObject { get; }
    bool isDestroy { get; }
    void Discard();
    void Execute();
    void HitFire(int fireId);
    Box GetHitBox();
}

public enum FieldObjectType
{
    None,
    Block,
    Brick,
    FireBrick,
    Bomb,
    Fire,
    Item,
}
