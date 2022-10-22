using System.Collections.Generic;
using UnityEngine;

public class CollisionManager:DI
{
	private Vector3Int U = new Vector3Int(0, 1, 0);
	private Vector3Int UR = new Vector3Int(1, 1, 0);
	private Vector3Int R = new Vector3Int(1, 0, 0);
	private Vector3Int DR = new Vector3Int(1, -1, 0);
	private Vector3Int D = new Vector3Int(0, -1, 0);
	private Vector3Int DL = new Vector3Int(-1, -1, 0);
	private Vector3Int L = new Vector3Int(-1, 0, 0);
	private Vector3Int UL = new Vector3Int(-1, 1, 0);

	private Vector3Int[] DY_DX;
	private Vector3Int[] DY_UX;
	private Vector3Int[] DY_EX;

	private Vector3Int[] UY_DX;
	private Vector3Int[] UY_UX;
	private Vector3Int[] UY_EX;

	private Vector3Int[] EY_DX;
	private Vector3Int[] EY_UX;
	private Vector3Int[] EY_EX;

	public CollisionManager()
    {
		DY_DX = new Vector3Int[] { L, D, DL, UL, DR };
		DY_UX = new Vector3Int[] { R, D, DR, UR, DL };
		DY_EX = new Vector3Int[] { DL, D, DR, L, R };

		UY_DX = new Vector3Int[] { L, U, UL, UR, DL };
		UY_UX = new Vector3Int[] { R, U, UR, UL, DR };
		UY_EX = new Vector3Int[] { UL, U, UR, L, R };

		EY_DX = new Vector3Int[] { DL, L, UL, U, D };
		EY_UX = new Vector3Int[] { DR, R, UR, U, D };
		EY_EX = new Vector3Int[0];
	}

	private Vector3Int[] GetTargetOffset(Vector2 vector)
	{
		if (vector.y < 0)
		{
			if (vector.x < 0)
				return DY_DX;
			else if (0 < vector.x)
				return DY_UX;
			else
				return DY_EX;
		}
		else if (0 < vector.y)
		{
			if (vector.x < 0)
				return UY_DX;
			else if (0 < vector.x)
				return UY_UX;
			else
				return UY_EX;
		}
		else
		{
			if (vector.x < 0)
				return EY_DX;
			else if (0 < vector.x)
				return EY_UX;
			else
				return EY_EX;
		}
	}

	public void ObjectHitCheck()
	{
		Player player = objects.player;
		if (player != null)
        {
			foreach (Enemy enemy in objects.enemys)
			{
				Circle circle = enemy.GetAttackHitCircle();
				if (CircleCollision.PointHitCheck(circle, player.position))
					player.HitEnemy(enemy);
			}
			PlayerFieldHitCheck(player, field);
		}
		EnemyFieldHitCheck(field);
	}

	private void PlayerFieldHitCheck(Player player, Field field)
    {
		CheckHit();
		CheckCollision();

		void CheckHit()
        {
			IFieldObject cell = field.Get(player.location);

			if (cell == null) return;
			if (cell.isDestroy) return;

			Box box = cell.GetHitBox();
			switch (cell.fieldType)
			{
				case FieldObjectType.FireBrick:
				case FieldObjectType.Fire:
					if (BoxCollision.PointHitCheck(player.position, box))
						player.HitFire();
					break;
				case FieldObjectType.Item:
					if (BoxCollision.PointHitCheck(player.position, box))
					{
						Item item = cell as Item;
						player.HitItem(item.itemName);
						item.HitPlayer(player);
					}
					break;
			}
		}

		void CheckCollision()
        {
			Circle circle = player.GetHitCircle();

			Vector3Int[] offset = GetTargetOffset(player.vector);
			foreach (Vector3Int of in offset)
			{
				IFieldObject cell = field.Get(player.location + new Vector3Int(of.x, of.y, 0));

				if (cell == null) continue;
				if (cell.isDestroy) continue;

				Box box = cell.GetHitBox();
				switch (cell.fieldType)
				{
					case FieldObjectType.Block:
						circle.position = BoxCollision.CirclePositionCorrection(circle, box);
						break;
					case FieldObjectType.Brick:
						if (!data.useBrickPass)
							circle.position = BoxCollision.CirclePositionCorrection(circle, box);
						break;
					case FieldObjectType.Bomb:
						if (!data.useBombPass)
							circle.position = BoxCollision.CirclePositionCorrection(circle, box);
						break;
					case FieldObjectType.FireBrick:
						if (!data.useBrickPass)
							circle.position = BoxCollision.CirclePositionCorrection(circle, box);
						break;
				}
				player.position2D = circle.position;
			}
		}
    }

    private void EnemyFieldHitCheck(Field field)
	{
		foreach (Enemy enemy in objects.enemys)
		{
			CheckHit(enemy);
			CheckCollision(enemy);
		}

		void CheckHit(Enemy enemy)
		{
			IFieldObject cell = field.Get(enemy.location);

			if (cell == null) return;
			if (cell.isDestroy) return;

			Box box = cell.GetHitBox();
			switch (cell.fieldType)
			{
				case FieldObjectType.FireBrick:
					if (BoxCollision.PointHitCheck(enemy.position, box) && enemy.brickPass)
						enemy.HitFire((cell as FireBrick).fireID);
					break;
				case FieldObjectType.Fire:
					if (BoxCollision.PointHitCheck(enemy.position, box))
						enemy.HitFire((cell as Fire).fireID);
					break;
			}
		}

		void CheckCollision(Enemy enemy)
		{
			Circle circle = enemy.GetHitCircle();

			Vector3Int[] offset = GetTargetOffset(enemy.vector);
			foreach (Vector3Int of in offset)
			{
				IFieldObject cell = field.Get(enemy.location + new Vector3Int(of.x, of.y, 0));

				if (cell == null) continue;
				if (cell.isDestroy) continue;

				Box box = cell.GetHitBox();
				switch (cell.fieldType)
				{
					case FieldObjectType.Bomb:
					case FieldObjectType.Block:
						CheckBlock();
						break;
					case FieldObjectType.Brick:
						if (!enemy.brickPass)
							CheckBlock();
						break;
					case FieldObjectType.FireBrick:
						if (!enemy.brickPass)
							CheckBlock();
						break;
				}
				enemy.position2D = circle.position;

				void CheckBlock()
				{
					if (BoxCollision.CircleHitCheck(circle, box))
						enemy.HitBlock();
					circle.position = BoxCollision.CirclePositionCorrection(circle, box);
				}
			}
		}
	}

}