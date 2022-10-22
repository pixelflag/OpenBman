using System.Collections.Generic;
using UnityEngine;

public class Player : SpriteBase
{
    private AnimCounter anim;
    public bool isDead { get; private set; }

    public bool isSetBom => 0 <= setBomWaitCount;
    private int setBomWaitCount = -1;
    public Vector2 vector { get; private set; }

    private List<Bomb> bombs;
    public int bombCount => bombs.Count;

    private int invincibleCount = 0;
    private int walkTapCount;

    public override void Initialize()
    {
        base.Initialize();
        anim = new AnimCounter(4, 8);
        bombs = new List<Bomb>();
    }

    public void SetBombs()
    {
        if (isDead) return;
        if (bombCount >= data.currentBomb) return;

        if (field.Contains(location))
        {
            IFieldObject obj = field.Get(location);
            if (obj.fieldType == FieldObjectType.Fire)
            {
                Fire fire = obj as Fire;
                creater.CreateExplosion(location, fire.fireID);
            }
        }
        else
        {
            Bomb bomb = creater.CreateBomb(Calculate.PositionToLocation(position));
            sound.PlayOneShotOnChannel((int)SoundChannel.SetBom, SeType.SetBom, 0.5f);

            setBomWaitCount = 3;
            bomb.OnDestroy = (SpriteBase target) => { bombs.Remove(target as Bomb); };
            bombs.Add(bomb);
        }
    }

    public void RemoteFire()
    {
        if (isDead) return;
        if (data.useRemocon)
        {
            foreach(Bomb bm in bombs)
            {
                bm.Explose();
                break;
            }
        }
    }

    public void Move(Vector2 inputVector)
    {
        if (isDead) return;
        if(inputVector == Vector2.zero)
        {
            walkTapCount = 0;
        }
        walkTapCount++;

        if (walkTapCount %16 == 0)
        {
            if (inputVector.x != 0)
                sound.PlayOneShotOnChannel((int)SoundChannel.Walk, SeType.WalkA, 1);
            else if (inputVector.y != 0)
                sound.PlayOneShotOnChannel((int)SoundChannel.Walk, SeType.WalkB, 1);
        }

        vector = CharacterExtendMove.ExtendMove(field, data.useBrickPass, data.useBombPass, inputVector, position, location);
        this.x += data.speedUp ? vector.x * 1.5f : vector.x;
        this.y += data.speedUp ? vector.y * 1.5f : vector.y;

        AnimationUpdate(vector.x, vector.y);
    }

    private void AnimationUpdate(float vx, float vy)
    {
        anim.Execute();

        int head = 0;
        if (vx > 0) head = 4;
        else if (vx < 0) head = 12;
        else if (vy > 0) head = 8;
        else if (vy < 0) head = 0;

        render.sprite = sprites[head + anim.frame];
    }

    public override void Execute()
    {
        if (isDead) return;
        UpdateLocationWithPosition();
        setBomWaitCount--;

        if(data.useInvincible)
        {
            invincibleCount--;
            if(invincibleCount <= 0)
            {
                data.useInvincible = false;
            }
        }
    }

    // collisions ----------

    public void HitEnemy(Enemy enemy)
    {
        if (enemy.isDestroy) return;
        if (data.useInvincible) return;
        if (data.playerUnDeadMode) return;
        PlayerDead();
    }

    public void HitFire()
    {
        if (data.useFirePass) return;
        if (data.useInvincible) return;
        if (data.playerUnDeadMode) return;
        PlayerDead();
    }

    public void PlayerDead()
    {
        if (isDead) return;

        isDead = true;
        sound.PlayOneShotOnChannel((int)SoundChannel.SetBom, SeType.Dead, 0.8f);
        creater.CreatePlayerDeadEffect(position);
        render.sprite = null;

        // パワーアップ状態のリセット
        data.useRemocon = false;
        data.useFirePass = false;
        data.useBrickPass = false;
        data.useBombPass = false;
        data.useInvincible = false;

        if (OnDead != null) OnDead();
    }

    public void HitItem(ItemName itemType)
    {
        if (isDestroy) return;

        switch (itemType)
        {
            case ItemName.None:
                break;
            case ItemName.Door:
                if (OnDoorHit != null) OnDoorHit();
                break;
            case ItemName.Fire:
                ItemGet();
                data.AddFire();
                break;
            case ItemName.Bomb:
                ItemGet();
                data.AddBomb();
                break;
            case ItemName.Remocon:
                ItemGet();
                data.useRemocon = true;
                break;
            case ItemName.Speed:
                ItemGet();
                data.speedUp = true;
                break;
            case ItemName.BombPass:
                ItemGet();
                data.useBombPass = true;
                break;
            case ItemName.BrickPass:
                ItemGet();
                data.useBrickPass = true;
                break;
            case ItemName.FirePass:
                ItemGet();
                data.useFirePass = true;
                break;
            case ItemName.Invincible:
                ItemGet();
                data.useInvincible = true;
                invincibleCount = data.invincibleTime;
                if (OnStartInvisible != null) OnStartInvisible();
                break;
        }

        void ItemGet()
        {
            sound.PlayOneShotOnChannel((int)SoundChannel.SetBom, SeType.ItemGet, 0.8f);
            data.AddScore(1000);
            if (OnGetItem != null) OnGetItem();
        }
    }

    public delegate void PlayerDelegate();
    public PlayerDelegate OnDead;
    public PlayerDelegate OnDoorHit;
    public PlayerDelegate OnGetItem;
    public PlayerDelegate OnStartInvisible;
    public PlayerDelegate OnEndInvisible;
}