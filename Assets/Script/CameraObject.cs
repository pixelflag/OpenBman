using UnityEngine;

public class CameraObject : MonoBehaviour
{
    public int left = 480;
    public int offset = 8;

    public void CameraUpdate(Vector3 position)
    {
        float hw = Mathf.Floor(Global.screenWidth / 2);
        float hh = Mathf.Floor(Global.screenHeight / 2);

        float x = position.x;
        x = x < hw-offset ? hw-offset : x;
        x = left - hw + offset < x ? left - hw + offset : x;

        float y = transform.position.y;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
