public class AnimCounter
{
    public int totalframe { get; private set; }
    public int wait { get; private set; }
    public int frame => (count / wait) % totalframe;
    public bool isEnd => count%(totalframe * wait) == (totalframe * wait)-1;
    private int count;

    public AnimCounter(int totalframe, int wait)
    {
        this.totalframe = totalframe <= 0? 1: totalframe;
        this.wait = wait <= 0 ? 1 : wait;
    }

    public void Reset()
    {
        count = 0;
    }

    public void Execute()
    {
        count++;
    }
}
