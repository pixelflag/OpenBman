using UnityEngine;

public static class Global
{
    public const int screenWidth  = 256;
    public const int screenHeight = 224;
    public static int gridSize = 16;

    public static uint count { get; private set; }
    public static void CountUp() { count++; }
}
