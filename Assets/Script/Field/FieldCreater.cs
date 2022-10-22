using System.Collections.Generic;
using UnityEngine;

public static class FieldCreater
{
	public static void CreateField(StageData stageData)
    {
		ObjectCreater creater = ObjectCreater.instance;
		Field field = Field.instance;
		Vector2Int fieldSize = field.fieldSize;

		// 敵の初期配置
		Dictionary<Vector3Int, Enemy> enemysLocation = new Dictionary<Vector3Int, Enemy>();

		// Blockの作成
		for (int yy = 0; yy < fieldSize.y; yy++)
		{
			for (int xx = 0; xx < fieldSize.x; xx++)
			{
				int fsx = fieldSize.x - 1;
				int fsy = fieldSize.y - 1;
				if (yy == 0 || yy == fsy ||	xx == 0 || xx == fsx || xx % 2 == 0 && yy % 2 == 0)
					creater.CreateBlock(new Vector3Int(xx, yy, 0));
			}
		}

		// 敵を配置する
		int count = 0;
		while (GameData.instance.currentEnemyNum < stageData.enemys.Length)
		{
			int xx;
			int yy;
			Vector3Int location;
			// ポンタンだけは近すぎると詰むので遠くに配置
			if (stageData.enemys[count] == EnemyName.Coin)
			{
				xx = Random.Range(28, fieldSize.x - 1);
				yy = Random.Range(1, fieldSize.y - 1);
				location = new Vector3Int(xx, yy, 0);
			}
			else
            {
				xx = Random.Range(1, fieldSize.x - 1);
				yy = Random.Range(1, fieldSize.y - 1);
				location = new Vector3Int(xx, yy, 0);
			}

			// プレイヤーから近すぎる場所には配置しない
			if (xx < 5 && yy > 7) continue;
			// ブロックがある位置には配置しない。
			if (field.Contains(location)) continue;
			// すでに配置されている場所には配置しない。
			if (enemysLocation.ContainsKey(location)) continue;

			Vector3 pos = Calculate.LocationToPosition(location);
			Enemy enemy = creater.CreateEnemy(stageData.enemys[count], pos);
			enemysLocation.Add(location, enemy);

			count++;
		}

		// Brickの散布
		bool isSetDoor = false;
		int itemIndex = 0;
		int blockCount = 0;

		while (blockCount < stageData.blockCount)
		{
			int xx = Random.Range(1, fieldSize.x - 1);
			int yy = Random.Range(1, fieldSize.y - 1);
			Vector3Int location = new Vector3Int(xx, yy, 0);

			// スタート地点にレンガを置かない。
			if (xx < 3 && yy > 9) continue;
			// ブロックがある位置には配置しない。
			if (field.Contains(location)) continue;
			// 敵がいる場所には配置しない。
			if (enemysLocation.ContainsKey(location)) continue;

			Brick brick = creater.CreateBrick(location);
			if (!isSetDoor)
			{
				brick.dropItem = ItemName.Door;
				isSetDoor = true;
			}
			else if (itemIndex < stageData.dropItems.Length )
			{
				brick.dropItem = stageData.dropItems[itemIndex];
				itemIndex++;
			}
			blockCount++;
		}
	}
}