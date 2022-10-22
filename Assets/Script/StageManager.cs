using UnityEngine;

public class StageManager : DIMonoBehaviour
{
    [SerializeField]
    private CameraObject mainCamera = default;

    private CollisionManager collision;

    private int waitCount;
    private bool isPause;
    private bool isGetItem;

    private Vector3Int playerInitLocation = new Vector3Int(1,11,0);

    private State state;
    private enum State
    {
        StageTitle,
        Play,
        Clear,
        GameOver,
    }

    public void Initialize()
    {
        collision = new CollisionManager();
    }

    public void StageStart(int stageNum)
    {
        FieldCreater.CreateField(data.GetStageData(stageNum));
        isGetItem = false;
        ReStart();
    }

    private void ReStart()
    {
        objects.StageRestart();
        objects.OnEnemyAllKill = () =>
        {
            sound.PlayJingle(BgmType.AllKill, 1.0f);
        };

        field.StageRestart();

        Player player = creater.CreatePlayer();
        player.position = Calculate.LocationToPosition(playerInitLocation);
        player.OnDoorHit = () =>
        {
            if (objects.enemys.Count == 0)
                state = State.Clear;
        };
        player.OnGetItem = () =>
        {
            isGetItem = true;
            ChangeBgm();
        };
        player.OnStartInvisible = () =>
        {
            sound.PlayBgm(BgmType.Special);
        };
        player.OnEndInvisible = () =>
        {
            ChangeBgm();
        };

        mainCamera.CameraUpdate(player.transform.position);

        data.useInvincible = false;

        waitCount = 0;
        HadManager.instance.ShowStageNum(data.currentStage);
        sound.PlayJingle(BgmType.StageStart,1.0f);
        state = State.StageTitle;
    }

    private void ChangeBgm()
    {
        if (isGetItem)
        {
            if (sound.currentBgm != BgmType.PowerUp)
            {
                sound.PlayBgm(BgmType.PowerUp);
            }
        }
        else
        {
            if (sound.currentBgm != BgmType.Normal)
            {
                sound.PlayBgm(BgmType.Normal);
            }
        }
    }

    public void Execute(ControllerInput input)
    {
        switch (state)
        {
            case State.StageTitle:
                waitCount++;
                if (waitCount > 180)
                {
                    HadManager.instance.HideCenter();
                    ChangeBgm();
                    waitCount = 0;
                    data.TimeCountReset();
                    state = State.Play;
                }
                break;
            case State.Play:
                if (input.GetKeyDown(ControllerButtonType.Start))
                {
                    isPause = !isPause;
                    Pause(isPause);
                }
                if (isPause) return;

                if (!objects.player.isDead)
                {
                    // Key Update
                    if (input.stickX != 0 || input.stickY != 0)
                        objects.player.Move(new Vector2(input.stickX, input.stickY));
                    if (input.GetKeyDown(ControllerButtonType.A))
                        objects.player.SetBombs();
                    if (input.GetKeyDown(ControllerButtonType.B))
                        objects.player.RemoteFire();

                    data.TimeCountDown(1);

                    if (data.time <= 0)
                        objects.player.PlayerDead();
                }
                else
                {
                    waitCount++;
                    if(1 == waitCount)
                    {
                        sound.StopBgm();
                    }
                    if (60 == waitCount)
                    {
                        sound.PlayJingle(BgmType.Dead, 1);
                    }
                    if (waitCount > 180)
                    {
                        waitCount = 0;
                        if (data.left <= 0)
                        {
                            state = State.GameOver;
                        }
                        else
                        {
                            data.MinusLeft();
                            ReStart();
                        }
                    }
                }
                objects.Execute();
                field.Execute();
                break;
            case State.Clear:
                waitCount++;
                if (waitCount == 1)
                {
                    sound.StopBgm();
                    sound.PlayJingle(BgmType.StageClear, 1.0f);
                }
                if (waitCount > 150)

                {
                    waitCount = 0;
                    objects.Flush();
                    field.Flush();

                    if (OnStageClear != null) OnStageClear();
                }
                break;
            case State.GameOver:
                waitCount++;
                if (waitCount == 1)
                {
                    HadManager.instance.ShowGameOver();
                    objects.Flush();
                    field.Flush();

                    sound.StopBgm();
                    sound.PlayJingle(BgmType.GameOver, 1.0f);
                }
                if (waitCount == 360)
                {
                    if (OnGameOver != null) OnGameOver();
                }
                break;
        }

        collision.ObjectHitCheck();

        objects.CheckDestroy();
        field.CheckDestroy();

        mainCamera.CameraUpdate(objects.player.position);
    }

    private void Pause(bool isPause)
    {
        if (isPause)
        {
            sound.PlayJingle(BgmType.AllKill, 1.0f);
            sound.PauseBgm();
        }
        else
        {
            sound.StopJingle();
            sound.ResumeBgm();
        }
    }

    public delegate void GameOverDelegate();
    public GameOverDelegate OnGameOver;

    public delegate void StageClearDelegate();
    public StageClearDelegate OnStageClear;
}
