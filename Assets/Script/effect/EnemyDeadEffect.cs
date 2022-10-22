using UnityEngine;

public class EnemyDeadEffect : SpriteBase
{
    [SerializeField]
    private Sprite[] scoreSprite = default;

    private ScoreType score;
    private int count = 0;

    public void Initialize(Sprite deadSprite, ScoreType score)
    {
        base.Initialize();
        render.sprite = deadSprite;
        this.score = score;
    }

    public override void Execute()
    {
        switch (count)
        {
            case 60:
                render.sprite = sprites[0];
                break;
            case 70:
                render.sprite = sprites[1];
                break;
            case 80:
                render.sprite = sprites[2];
                break;
            case 90:
                render.sprite = sprites[3];
                break;
            case 100:
                render.sprite = sprites[4];
                break;
            case 110:
                data.AddScore(GetScore(score));
                render.sprite = scoreSprite[(int)score];
                break;
            case 160:
                Discard();
                break;
        }
        count++;
        
        int GetScore(ScoreType score)
        {
            switch (score)
            {
                case ScoreType.Score100: return 100;
                case ScoreType.Score200: return 200;
                case ScoreType.Score400: return 400;
                case ScoreType.Score800: return 800;
                case ScoreType.Score1000: return 1000;
                case ScoreType.Score2000: return 2000;
                case ScoreType.Score4000: return 4000;
                case ScoreType.Score8000: return 8000;
                case ScoreType.Score10000: return 10000;
                case ScoreType.Score20000: return 20000;
                case ScoreType.Score40000: return 40000;
                case ScoreType.Score80000: return 80000;
                default:
                    throw new System.Exception("There is no score");
            }
        }
    }
}
