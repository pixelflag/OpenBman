using UnityEngine;

public class Enemy : SpriteBase
{
    [SerializeField]
    private EnemyName enemeyName;
    [SerializeField]
    private Sprite deadSprite = default;
    [SerializeField]
    private ScoreType _score;
    public ScoreType score => _score;
    [SerializeField]
    private float _moveSpeed = 1;
    public float moveSpeed => _moveSpeed;
    [SerializeField]
    private bool _brickPass = false;
    public bool brickPass => _brickPass;

    // プレイヤーとのヒットサイズ
    [SerializeField]
    protected int AttackCollisionSize = 20;
    public Circle GetAttackHitCircle()
    {
        return new Circle(position, AttackCollisionSize / 2);
    }

    // スタート位置
    public Vector3 initPosition { get; set; }
    // 後で増えた
    public bool isExtra { get; set; }

    public Vector2 vector { get; private set; }

    private EnemyLogic logic;
    private AnimCounter anim;

    public override void Initialize()
    {
        base.Initialize();

        vector = new Vector2();
        anim = new AnimCounter(4, 8);

        logic = GetComponent<EnemyLogic>();
        logic.Initialize();
    }

    public void ReStart()
    {
        if (isExtra)
        {
            Discard();
        }
        else
        {
            position = initPosition;
            anim.Reset();
            render.sprite = sprites[0];
            logic.Initialize();
        }
    }

    public override void Execute()
    {
        vector = logic.Execute(this);
        vector = CharacterExtendMove.ExtendMove(field, brickPass, false, vector, position, location);

        x += vector.x * moveSpeed;
        y += vector.y * moveSpeed;

        if (vector.x == 1 || vector.y == 1)
            render.flipX = true;
        else
            render.flipX = false;

        anim.Execute();
        render.sprite = sprites[anim.frame];

        UpdateLocationWithPosition();
    }

    // collisions ----------
    public void HitFire(int fireId)
    {
        Discard();
        creater.CreateEnemyDeadEffect(deadSprite, score, position, fireId);
        if (OnDead != null) OnDead();
    }

    public void HitBlock()
    {
        vector = logic.BlockHit(location, brickPass);
    }

    public delegate void DeadDelegate();
    public DeadDelegate OnDead;
}