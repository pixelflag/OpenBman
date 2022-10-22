using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private void Awake()
    {
        instance = this;
    }

    // デバッグ時の無敵モード
    [SerializeField]
    private bool _playerUnDeadMode;
    public bool playerUnDeadMode => _playerUnDeadMode;

    // 残機初期値
    [SerializeField]
    private int startLeft = 2;

    // 残り時間初期値
    [SerializeField]
    private int limitTime = 200;

    // アイテムでの無敵時間
    [SerializeField]
    private int _invincibleTime = 30;
    public int invincibleTime => _invincibleTime * 60;

    // 爆弾の初期値
    [SerializeField]
    private int initBomb = 2;

    // 爆弾の最大数
    [SerializeField]
    private int maxBomb = 10;

    // 炎の初期値
    [SerializeField]
    private int initFire = 2;

    // 炎の最大数
    [SerializeField]
    private int maxFire = 10;

    // ステージに登場できる敵の最大数
    [SerializeField]
    private int _maxEnemyNum = 12;
    public int maxEnemyNum => _maxEnemyNum;

    // ステージ選択
    [SerializeField]
    private int startStage = 1;

    // ステージデータ
    [SerializeField]
    private StageData[] stages;
    public StageData GetStageData(int stageNum) { return stages[stageNum]; }

    // 残機
    public int left { get; private set; }
    // スコア
    public long score { get; private set; }
    // 残り時間
    public int time { get; private set; }

    // プレイヤーステータス
    public int currentBomb { get; set; }
    public int currentFire { get; set; }
    public bool speedUp { get; set; }
    public bool useRemocon { get; set; }
    public bool useBombPass { get; set; }
    public bool useBrickPass { get; set; }
    public bool useFirePass { get; set; }
    public bool useInvincible { get; set; }

    // 現在のステージ
    public int currentStage;
    // 現在の敵の数
    public int currentEnemyNum => ObjectExecuter.instance.enemys.Count;
    // 最後のステージである
    public bool isLastStage => currentStage == stages.Length - 1;

    public void Reset()
    {
        left = startLeft;
        if (OnLeftUpdate != null) OnLeftUpdate(left);

        time = limitTime * 60;
        if (OnTimeUpdate != null) OnTimeUpdate((int)Mathf.Ceil(time / 60));

        score = 0;
        if (OnScoreUpdate != null) OnScoreUpdate(score);

        currentStage = startStage;

        currentBomb = initBomb;
        currentFire = initFire;
        useRemocon = false;
        speedUp = false;
        useBombPass = false;
        useBrickPass = false;
        useFirePass = false;
        useInvincible = false;
    }

    public void PlusLeft()
    {
        left++;
        left = Mathf.Clamp(left, 0, 99);
        if (OnLeftUpdate != null) OnLeftUpdate(left);
    }

    public void MinusLeft()
    {
        left--;
        left = Mathf.Clamp(left, 0, 99);
        if (OnLeftUpdate != null) OnLeftUpdate(left);
    }

    public void TimeCountReset()
    {
        time = limitTime * 60;
    }

    public void TimeCountDown(int value)
    {
        time -= value;
        if (OnTimeUpdate != null) OnTimeUpdate((int)Mathf.Ceil(time / 60));
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        score = 9999999900 < score ? 9999999900 : score;
        if (OnScoreUpdate != null) OnScoreUpdate(score);
    }

    public void AddFire()
    {
        currentFire++;
        currentFire = Mathf.Min(currentFire , maxFire);
    }

    public void AddBomb()
    {
        currentBomb++;
        currentBomb = Mathf.Min(currentBomb, maxBomb);
    }

    public EnemyName GetPenaltyEnemyName()
    {
        return stages[currentStage].penaltyEnemy;
    }

    public delegate void ScoreUpdateDelegate(long score);
    public ScoreUpdateDelegate OnScoreUpdate;

    public delegate void LeftUpdateDelegate(int left);
    public LeftUpdateDelegate OnLeftUpdate;

    public delegate void TimeUpdateDelegate(int time);
    public TimeUpdateDelegate OnTimeUpdate;
}