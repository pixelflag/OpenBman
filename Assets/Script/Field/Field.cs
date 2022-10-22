using System.Collections.Generic;
using UnityEngine;

public class Field
{
    public static Field instance;

    public Vector2Int fieldSize = new Vector2Int(31, 13);
    public IFieldObject[] cells { get; private set; }

    public List<IFieldObject> fieldObjects { get; private set; }

    public Field()
    {
        instance = this;
        cells = new IFieldObject[fieldSize.x * fieldSize.y];
        fieldObjects = new List<IFieldObject>();
    }

    public void Set(Vector3Int location, IFieldObject cell)
    {
        int index = GetIndex(location);
        if(cells[index] != null)
        {
            cells[index].Discard();
            cells[index] = null;
        }
        cells[index] = cell;
        fieldObjects.Add(cell);
    }

    private int GetIndex(Vector3Int location)
    {
        if (location.x < 0 || location.y < 0)
            throw new System.Exception("minus location. : " + location);

        return location.y * fieldSize.x + location.x;
    }

    public IFieldObject Get(Vector3Int location)
    {
        int index = GetIndex(location);
        return cells[index];
    }

    public void Remove(Vector3Int location)
    {
        IFieldObject obj = Get(location);
        if (obj == null) return;
        obj.Discard();
    }

    public bool Contains(Vector3Int location)
    {
        int index = GetIndex(location);
        return cells[index] != null;
    }

    public FieldObjectType ExistsType(Vector3Int location)
    {
        if (location.x < 0 || location.y < 0)
            return FieldObjectType.None;

        IFieldObject obj = Get(location);
        if (obj == null) return FieldObjectType.None;
        return obj.fieldType;
    }

    public void StageRestart()
    {
        for(int i=0; i < cells.Length; i++)
        {
            if (cells[i] == null) continue;
            switch (cells[i].fieldType)
            {
                case FieldObjectType.Block:
                case FieldObjectType.Brick:
                case FieldObjectType.Item:
                    break;
                case FieldObjectType.Bomb:
                case FieldObjectType.FireBrick:
                case FieldObjectType.Fire:
                    cells[i].Discard();
                    cells[i] = null;
                    break;
            }
        }
    }

    public void Flush()
    {
        for (int i=0; i < cells.Length; i++)
        {
            if (cells[i] == null) continue;

            cells[i].Discard();
            cells[i] = null;
        }
    }

    public void Execute()
    {
        foreach (IFieldObject obj in cells)
        {
            if (obj == null) continue;
            if (obj.isDestroy) continue;
            obj.Execute();
        }
    }

    public void CheckDestroy()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == null) continue;
            if (cells[i].isDestroy)
            {
                cells[i] = null;
            }
        }

        for (int i = fieldObjects.Count - 1; 0 <= i; i--)
        {
            if (fieldObjects[i].isDestroy)
            {
                Object.Destroy(fieldObjects[i].gameObject);
                fieldObjects.RemoveAt(i);
            }
        }
    }
}