using System.Collections.Generic;
using UnityEngine;

public class ObjectExecuter
{
    public static ObjectExecuter instance;

    public Player player;
    public List<Enemy> enemys { get; private set; }
    public List<SpriteBase> effects { get; private set; }

    public ObjectExecuter()
    {
        instance = this;
        enemys = new List<Enemy>();
        effects = new List<SpriteBase>();
    }

    public void AddEnemy(Enemy enemy)
    {
        enemys.Add(enemy);
        enemy.OnDead = () =>
        {
            foreach(Enemy enemy in enemys)
            {
                if (enemy.isDestroy == false)
                    return;
            }

            if (OnEnemyAllKill != null)
                OnEnemyAllKill();
        };
    }

    public delegate void ObjectExecuteDelegate();
    public ObjectExecuteDelegate OnEnemyAllKill;

    public void StageRestart()
    {
        foreach (Enemy enemy in enemys)
            enemy.ReStart();

        foreach (SpriteBase sb in effects)
            sb.Discard();
    }

    public void Flush()
    {
        foreach (SpriteBase sb in enemys)
            sb.Discard();

        foreach (SpriteBase sb in effects)
            sb.Discard();
    }

    public void Execute()
    {
        if(player != null)
            if(!player.isDestroy)
                player.Execute();

        foreach (SpriteBase sb in enemys)
        {
            if (!sb.isDestroy)
                sb.Execute();
        }
        foreach (SpriteBase sb in effects)
        {
            if (!sb.isDestroy)
                sb.Execute();
        }
    }

    public void CheckDestroy()
    {
        if (player != null && player.isDestroy)
            Object.Destroy(player.gameObject);

        for (int i = enemys.Count - 1; 0 <= i; i--)
        {
            if (enemys[i].isDestroy)
            {
                Object.Destroy(enemys[i].gameObject);
                enemys.RemoveAt(i);
            }
        }
        for (int i = effects.Count - 1; 0 <= i; i--)
        {
            if (effects[i].isDestroy)
            {
                Object.Destroy(effects[i].gameObject);
                effects.RemoveAt(i);
            }
        }
    }
}
