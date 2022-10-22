using UnityEngine;

public class CharSprite : MonoBehaviour
{
    [SerializeField]
    private CharLibrary library = default;

    public void SetChar(char c)
    {
        GetComponent<SpriteRenderer>().sprite = library.GetSprite(c);
    }

    public void Brank()
    {
        GetComponent<SpriteRenderer>().sprite = null;
    }
}
