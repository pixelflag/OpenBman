using System;

[Serializable]
public struct StageData
{
    // ステージ数
    public int stageNum;

    // ステージのブロックの密度
    public int blockCount;

    // 出現アイテム
    public ItemName[] dropItems;

    // 初期配置の敵
    public EnemyName[] enemys;

    // ペナルティー時に出現する敵
    public EnemyName penaltyEnemy;

    public StageData(int stageNum, int blockCount, ItemName[] dropItems, EnemyName[] enemys, EnemyName penaltyEnemy)
    {
        this.stageNum = stageNum;
        this.blockCount = blockCount;
        this.dropItems = dropItems;
        this.enemys = enemys;
        this.penaltyEnemy = penaltyEnemy;
    }
}
