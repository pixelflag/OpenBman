using UnityEngine;

public class Item : SpriteBase, IFieldObject
{
    public FieldObjectType fieldType { get; private set; }
    public ItemName itemName { get; private set; }

    private int count;
    public bool isInvincible => count < 30;

    private int fireId;

    public void Initialize(ItemName name, Vector3Int location, int fireId)
    {
        base.Initialize();
        UpdateLocationAndPosition(location);

        this.itemName = name;
        fieldType = FieldObjectType.Item;

        this.fireId = fireId;

        switch (itemName)
        {
            case ItemName.None:
                render.sprite = sprites[0];
                break;
            case ItemName.Door:
                render.sprite = sprites[1];
                break;
            case ItemName.Fire:
                render.sprite = sprites[2];
                break;
            case ItemName.Bomb:
                render.sprite = sprites[3];
                break;
            case ItemName.Remocon:
                render.sprite = sprites[4];
                break;
            case ItemName.Speed:
                render.sprite = sprites[5];
                break;
            case ItemName.BombPass:
                render.sprite = sprites[6];
                break;
            case ItemName.BrickPass:
                render.sprite = sprites[7];
                break;
            case ItemName.FirePass:
                render.sprite = sprites[8];
                break;
            case ItemName.Invincible:
                render.sprite = sprites[9];
                break;
        }
    }

    public override void Execute()
    {
        count++;
    }

    public override Box GetHitBox()
    {
        if(itemName == ItemName.Door)
            return new Box(position, collisionSize / 8, collisionSize / 8);
        else
            return new Box(position, collisionSize / 2, collisionSize / 2);
    }

    public virtual void HitPlayer(SpriteBase obj)
    {
        if( itemName != ItemName.Door)
        {
            Discard();
        }
    }

    public void HitFire(int fireId)
    {
        if (this.fireId == fireId) return;

        switch (itemName)
        {
            case ItemName.None:
                break;
            case ItemName.Door:
                creater.SpawnPenaltyEnemy(position);
                break;
            case ItemName.Fire:
            case ItemName.Bomb:
            case ItemName.Remocon:
            case ItemName.Speed:
            case ItemName.BombPass:
            case ItemName.BrickPass:
            case ItemName.FirePass:
            case ItemName.Invincible:
                creater.SpawnPenaltyEnemy(position);
                Discard();
                break;
        }
    }
}
