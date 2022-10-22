using UnityEngine;

public class ObjectCreater : DIMonoBehaviour
{
    public static ObjectCreater instance;

    [SerializeField]
    private Player playerPrefab = default;
    [SerializeField]
    private GameObject[] EnemysPrefab = default;
    [SerializeField]
    private GameObject[] EffectPrefab = default;
    [SerializeField]
    private Item ItemPrefab = default;
    [SerializeField]
    private Bomb BombPrefab = default;
    [SerializeField]
    private Fire FirePrefab = default;
    [SerializeField]
    private Block BlockPrefab = default;
    [SerializeField]
    private Brick BrickPrefab = default;
    [SerializeField]
    private FireBrick FireBrickPrefab = default;

    private ScoreChain scoreChain;

    private void Awake()
    {
        instance = this;
        scoreChain = new ScoreChain();
    }

    public Player CreatePlayer()
    {
        if (objects.player != null) Destroy(objects.player.gameObject);

        objects.player = Instantiate(playerPrefab.gameObject).GetComponent<Player>();
        objects.player.Initialize();
        return objects.player;
    }

    public Enemy CreateEnemy(EnemyName name, Vector3 position)
    {
        Enemy en = Instantiate(EnemysPrefab[(int)name]).GetComponent<Enemy>();
        en.Initialize();
        en.position = position;
        en.initPosition = position;
        objects.AddEnemy(en);
        return en;
    }

    public void SpawnPenaltyEnemy(Vector3 position)
    {
        int me = data.maxEnemyNum;
        int ce = data.currentEnemyNum;

        EnemyName en = data.GetPenaltyEnemyName();
        for (int i = 0; i < me - ce; i++)
        {
            Enemy enemy = CreateEnemy(en, position);
            enemy.isExtra = true;
        }
    }

    public SpriteBase CreatePlayerDeadEffect(Vector3 position)
    {
        PlayerDeadEffect ef = Instantiate(EffectPrefab[0]).GetComponent<PlayerDeadEffect>();
        ef.Initialize();
        ef.position = position;
        objects.effects.Add(ef);
        return ef;
    }

    public SpriteBase CreateEnemyDeadEffect(Sprite deadSprite, ScoreType score, Vector3 position, int fireId)
    {
        ChainObject chain = scoreChain.Chain(fireId);
        ScoreType newScore = scoreChain.GetScore(score, chain.chainCount);

        EnemyDeadEffect ef = Instantiate(EffectPrefab[1]).GetComponent<EnemyDeadEffect>();
        ef.Initialize(deadSprite, newScore);
        ef.position = position;
        objects.effects.Add(ef);
        return ef;
    }

    public Item CreateItem(ItemName name, Vector3Int location, int fireId)
    {
        Item it = Instantiate(ItemPrefab.gameObject).GetComponent<Item>();
        it.Initialize(name, location, fireId);
        field.Set(location, it);
        return it;
    }

    public Bomb CreateBomb(Vector3Int location)
    {
        Bomb bm = Instantiate(BombPrefab.gameObject, transform).GetComponent<Bomb>();
        bm.Initialize(location);
        field.Set(location, bm);
        return bm;
    }

    public Block CreateBlock(Vector3Int location)
    {
        Block bl = Instantiate(BlockPrefab.gameObject, transform).GetComponent<Block>();
        bl.Initialize(location);
        field.Set(location, bl);
        return bl;
    }

    public Brick CreateBrick(Vector3Int location)
    {
        Brick br = Instantiate(BrickPrefab.gameObject, transform).GetComponent<Brick>();
        br.Initialize(location);
        field.Set(location, br);
        return br;
    }

    public FireBrick CreateFireBrick(Vector3Int location, int fireID)
    {
        FireBrick fb = Instantiate(FireBrickPrefab.gameObject, transform).GetComponent<FireBrick>();
        fb.Initialize(location, fireID);
        field.Set(location, fb);
        return fb;
    }

    public void CreateExplosion(Vector3Int location, int fireId)
    {
        sound.PlayOneShotOnChannel((int)SoundChannel.Explose, SeType.Explose, 1);

        Fire(location, new bool[] { true, true, true, true });
        ExtendFire(location, new Vector3Int(-1, 0, 0), new bool[] { true, true, false, false }, new bool[] { false, true, false, false });
        ExtendFire(location, new Vector3Int( 1, 0, 0), new bool[] { true, true, false, false }, new bool[] { true, false, false, false });
        ExtendFire(location, new Vector3Int( 0,-1, 0), new bool[] { false, false, true, true }, new bool[] { false, false, true, false });
        ExtendFire(location, new Vector3Int( 0, 1, 0), new bool[] { false, false, true, true }, new bool[] { false, false, false, true });

        void ExtendFire(Vector3Int targetLocation, Vector3Int offset, bool[] body, bool[] tail)
        {
            int cf = data.currentFire;
            for (int i = 0; i < cf; i++)
            {
                targetLocation += offset;
                if (field.Contains(targetLocation))
                {
                    IFieldObject obj = field.Get(targetLocation);
                    switch (obj.fieldType)
                    {
                        case FieldObjectType.Fire:
                            SetupFire(targetLocation, cf == i + 1);
                            break;
                        default:
                            obj.HitFire(fireId);
                            return;
                    }
                }
                else
                {
                    SetupFire(targetLocation, cf == i + 1);
                }
            }

            void SetupFire(Vector3Int newLocation, bool isTail)
            {
                if (isTail)
                    Fire(newLocation, tail);
                else
                    Fire(newLocation, body);


            }
        }

        void Fire(Vector3Int newLocation, bool[] p)
        {
            Fire fr = Instantiate(FirePrefab, transform).GetComponent<Fire>();
            fr.Initialize(newLocation, fireId);
            fr.SetAnimationPattern(p[0], p[1], p[2], p[3]);
            field.Set(newLocation, fr);
        }
    }
}
