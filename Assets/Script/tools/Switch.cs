public struct Switch
{
    private bool current;
    private bool prev;

    public bool isEnter { get { return current == true && prev == false; } }
    public bool isExit { get { return current == false && prev == true; } }
    public bool isStay { get { return current; } }

    public void SetValue(bool b)
    {
        prev = current;
        current = b;
    }
}