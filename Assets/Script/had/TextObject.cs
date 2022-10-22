using UnityEngine;

public class TextObject : MonoBehaviour
{
    public void UpdateText(string text)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var cs = transform.GetChild(i).GetComponent<CharSprite>();

            if (i < text.Length )
                cs.SetChar(text[i]);
            else
                cs.Brank();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
