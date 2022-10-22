using UnityEngine;

public class DIMonoBehaviour : MonoBehaviour
{
    protected static ObjectCreater creater;
    protected static ObjectExecuter objects;
    protected static Field field;
    protected static SoundManager sound;
    protected static GameData data;

    public static void Injection()
    {
        creater = ObjectCreater.instance;
        objects = ObjectExecuter.instance;
        field = Field.instance;
        sound = SoundManager.instance;
        data = GameData.instance;
    }
}

public class DI
{
    protected static ObjectCreater creater;
    protected static ObjectExecuter objects;
    protected static Field field;
    protected static SoundManager sound;
    protected static GameData data;

    public static void Injection()
    {
        creater = ObjectCreater.instance;
        objects = ObjectExecuter.instance;
        field = Field.instance;
        sound = SoundManager.instance;
        data = GameData.instance;
    }
}
