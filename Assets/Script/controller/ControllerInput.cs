using UnityEngine;

public class ControllerInput
{
    public float stickX { get; set; }
    public float stickY { get; set; }

    public ControllerButton up = new ControllerButton();
    public ControllerButton down = new ControllerButton();
    public ControllerButton left = new ControllerButton();
    public ControllerButton right = new ControllerButton();
    public ControllerButton A = new ControllerButton();
    public ControllerButton B = new ControllerButton();
    public ControllerButton start = new ControllerButton();
    public ControllerButton select = new ControllerButton();

    public void Update()
    {
        stickX = 0;
        stickY = 0;

        stickX = Input.GetAxis("Gamepad1H");
        stickY = Input.GetAxis("Gamepad1V");

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) stickY += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) stickY -= 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) stickX -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) stickX += 1;

        up.SetValue(stickY > 0);
        down.SetValue(stickY < 0);
        left.SetValue(stickX < 0);
        right.SetValue(stickX > 0);

        A.SetValue(Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.Joystick1Button0));
        B.SetValue(Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.Joystick1Button1));

        select.SetValue(Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.Joystick1Button6));
        start.SetValue(Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.Joystick1Button7));
    }

    public bool GetKeyDown(ControllerButtonType type)
    {
        switch (type)
        {
            case ControllerButtonType.Up: return up.GetKeyDown();
            case ControllerButtonType.Down: return down.GetKeyDown();
            case ControllerButtonType.Left: return left.GetKeyDown();
            case ControllerButtonType.Right: return right.GetKeyDown();
            case ControllerButtonType.A: return A.GetKeyDown();
            case ControllerButtonType.B: return B.GetKeyDown();
            case ControllerButtonType.Start: return start.GetKeyDown();
            case ControllerButtonType.Select: return select.GetKeyDown();
            default: return false;
        }
    }

    public bool GetKey(ControllerButtonType type)
    {
        switch (type)
        {
            case ControllerButtonType.Up: return up.GetKey();
            case ControllerButtonType.Down: return down.GetKey();
            case ControllerButtonType.Left: return left.GetKey();
            case ControllerButtonType.Right: return right.GetKey();
            case ControllerButtonType.A: return A.GetKey();
            case ControllerButtonType.B: return B.GetKey();
            case ControllerButtonType.Start: return start.GetKey();
            case ControllerButtonType.Select: return select.GetKey();
            default: return false;
        }
    }
}

public class ControllerButton
{
    private bool current;
    private bool prev;

    public void SetValue(bool b)
    {
        prev = current;
        current = b;
    }

    public bool GetKeyDown()
    {
        return current && !prev;
    }

    public bool GetKey()
    {
        return current;
    }

    public override string ToString()
    {
        return current.ToString() + "," + prev.ToString();
    }
}

public enum ControllerButtonType
{
    Up,
    Down,
    Left,
    Right,
    A,
    B,
    Start,
    Select,
}
