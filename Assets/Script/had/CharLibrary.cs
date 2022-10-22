using UnityEngine;

[CreateAssetMenu(menuName = "Create CharLibrary", fileName = "CharLibrary")]
public class CharLibrary : ScriptableObject
{
    [SerializeField]
    private Sprite[] sprites = default;

    public Sprite GetSprite(char c)
    {
        return sprites[GetIndex(c)];
    }

    private int GetIndex(char c)
    {
        switch (c)
        {
            case 'A': return 0;
            case 'B': return 1;
            case 'C': return 2;
            case 'D': return 3;
            case 'E': return 4;
            case 'F': return 5;
            case 'G': return 6;
            case 'H': return 7;
            case 'I': return 8;
            case 'J': return 9;
            case 'K': return 10;
            case 'L': return 11;
            case 'M': return 12;
            case 'N': return 13;
            case 'O': return 14;
            case 'P': return 15;
            case 'Q': return 16;
            case 'R': return 17;
            case 'S': return 18;
            case 'T': return 19;
            case 'U': return 20;
            case 'V': return 21;
            case 'W': return 22;
            case 'X': return 23;
            case 'Y': return 24;
            case 'Z': return 25;
            case '0': return 26;
            case '1': return 27;
            case '2': return 28;
            case '3': return 29;
            case '4': return 30;
            case '5': return 31;
            case '6': return 32;
            case '7': return 33;
            case '8': return 34;
            case '9': return 35;
            case ' ': return 63;
            default: return 0;
        }
    }
}
