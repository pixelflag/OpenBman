using UnityEngine;

#if UNITY_EDITOR

[ExecuteAlways]
public class GridFitObject : MonoBehaviour
{
    public bool fitToGrid = true;
    public bool disableZ = true;
    public int grid = 16;

    void Update()
    {
        if (fitToGrid)
            GridFitting(grid);
        else
            GridFitting(1);
    }

    private void GridFitting(float grid)
    {
        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Round(pos.x / grid) * grid;
        pos.y = Mathf.Round(pos.y / grid) * grid;
        
        if(!disableZ)
            pos.z = Mathf.Round(pos.z / grid) * grid;
        
        transform.localPosition = pos;
    }
}
#endif