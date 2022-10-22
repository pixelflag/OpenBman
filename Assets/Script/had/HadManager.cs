using UnityEngine;

public class HadManager : MonoBehaviour
{
    public static HadManager instance;

    [SerializeField]
    private TextObject LeftText = default;
    [SerializeField]
    private TextObject ScoreText = default;
    [SerializeField]
    private TextObject TimeText = default;
    [SerializeField]
    private TextObject CenterText = default;
    [SerializeField]
    private GameObject Mask = default;

    private void Awake()
    {
        instance = this;
        CenterText.Hide();
        Mask.SetActive(false);
    }

    public void UpdateLeft(int left)
    {
        left = 99 < left ? 99 : left;
        left = left < 0 ? 0 : left;

        LeftText.UpdateText(ZeroPlusString(2,left.ToString()));
    }

    public void UpdateScore(long score)
    {
        int s = (int)(score/100);
        string str = s == 0? "" : s.ToString();
        ScoreText.UpdateText(ZeroPlusString(8, str)+"00");
    }

    public void UpdateTime(int time)
    {
        TimeText.UpdateText(ZeroPlusString(2, time.ToString()));
    }

    public void ShowGameOver()
    {
        CenterText.UpdateText("GAME OVER");
        CenterText.Show();
        Mask.SetActive(true);
    }

    public void ShowStageNum(int num)
    {
        CenterText.UpdateText("STAGE " + num.ToString());
        CenterText.Show();
        Mask.SetActive(true);
    }

    public void HideCenter()
    {
        CenterText.Hide();
        Mask.SetActive(false);
    }

    private string ZeroPlusString(int digit, string str)
    {
        int dist = digit - str.Length;
        dist = dist < 0 ? 0 : dist;

        for(int i=0; i < dist; i++)
        {
            str = " " + str;
        }

        return str;
    }
}
