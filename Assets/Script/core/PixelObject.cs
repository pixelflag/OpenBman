using UnityEngine;

public class PixelObject : DIMonoBehaviour
{
    private Transform _transform;

    public Vector3 position
    {
        get{ return _transform.position; }
        set { _transform.position = value; }
    }

    public Vector3 localPosition
    {
        get { return _transform.localPosition; }
        set { _transform.localPosition = value; }
    }

    public Vector2 position2D
    {
        get{ return _transform.position; }
        set
        {
            Vector3 pos = _transform.position;
            pos.x = value.x;
            pos.y = value.y;
            _transform.position = pos;
        }
    }

    public Vector2 localPosition2D
    {
        get{ return _transform.localPosition; }
        set
        {
            Vector3 pos = _transform.localPosition;
            pos.x = value.x;
            pos.y = value.y;
            _transform.localPosition = pos;
        }
    }

    public float x
    {
        get { return _transform.position.x; }
        set
        {
            Vector3 pos = _transform.position;
            pos.x = value;
            _transform.position = pos;
        }
    }

    public float y
    {
        get { return _transform.position.y; }
        set
        {
            Vector3 pos = _transform.position;
            pos.y = value;
            _transform.position = pos;
        }
    }

    public float z
    {
        get { return _transform.position.z; }
        set
        {
            Vector3 pos = _transform.position;
            pos.z = value;
            _transform.position = pos;
        }
    }

    public float localX
    {
        get { return _transform.localPosition.x; }
        set
        {
            Vector3 pos = _transform.localPosition;
            pos.x = value;
            _transform.localPosition = pos;
        }
    }

    public float localY
    {
        get { return _transform.localPosition.y; }
        set
        {
            Vector3 pos = _transform.localPosition;
            pos.y = value;
            _transform.localPosition = pos;
        }
    }

    public float localZ
    {
        get { return _transform.localPosition.z; }
        set
        {
            Vector3 pos = _transform.localPosition;
            pos.z = value;
            _transform.localPosition = pos;
        }
    }

    public virtual void Initialize()
    {
        this._transform = transform;
    }
}