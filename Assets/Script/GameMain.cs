using UnityEngine;

public class GameMain : DIMonoBehaviour
{
    [SerializeField]
    private StageManager stageManager = default;

    private ControllerInput input;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        new ObjectExecuter();
        new Field();
    }

    void Start()
    {
        Injection();
        DI.Injection();

        input = new ControllerInput();

        stageManager.Initialize();
        stageManager.OnStageClear = StageClear;
        stageManager.OnGameOver = GameOver;

        HadManager had = HadManager.instance;
        data.OnScoreUpdate = had.UpdateScore;
        data.OnLeftUpdate = had.UpdateLeft;
        data.OnTimeUpdate = had.UpdateTime;

        data.Reset();

        // タイトル無しでゲームスタート
        stageManager.StageStart(data.currentStage);

        // デバッグ用
        // data.playerUnDeadMode = true;
    }

    private void StageClear()
    {
        if(data.isLastStage)
        {
            // エンディングなしにステージ1に戻る
            data.currentStage = 1;
            stageManager.StageStart(data.currentStage);
        }
        else
        {
            data.currentStage += 1;
            data.PlusLeft();
            stageManager.StageStart(data.currentStage);
        }
    }

    private void GameOver()
    {
        data.Reset();
        stageManager.StageStart(data.currentStage);
    }

    private void FixedUpdate()
    {
        input.Update();

        stageManager.Execute(input);
        Global.CountUp();
    }
}