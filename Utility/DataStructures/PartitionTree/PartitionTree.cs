using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class PartitionTree <T>
{
    public List<T>[,] Partitions {get; private set;} 
    private int _partitionsPerAxis;
    private float _partitionWidth, _partitionHeight;
    private Func<T, Vector2> _elementPos;
    public Vector2 Size { get; private set; }
    public PartitionTree(List<T> elements, Func<T, Vector2> elementPos, Vector2 size, int partitionsPerAxis)
    {
        Size = size; 
        _elementPos = elementPos;
        _partitionsPerAxis = partitionsPerAxis;
        _partitionWidth = Size.x / _partitionsPerAxis;
        _partitionHeight = Size.y / _partitionsPerAxis;
        Partitions = new List<T>[_partitionsPerAxis, _partitionsPerAxis];
        for (int i = 0; i < _partitionsPerAxis; i++)
        {
            for (int j = 0; j < _partitionsPerAxis; j++)
            {
                Partitions[i,j] = new List<T>();
            }
        }
        foreach (var t in elements)
        {
            AddElement(t);
        }
    }


    public T GetNearest(Vector2 pos)
    {
        bool go = true; 
        int searchRadius = 0;
        var elements = GetElementsAtPos(pos, searchRadius);
        while(go)
        {
            var elementsPlus = GetElementsAtPos(pos, searchRadius + 1);
            if(elements.Count > 0)
            {
                var elementsByDist = elements.OrderBy(e => _elementPos(e).DistanceTo(pos));
                var closeElement = elementsByDist.ElementAt(0);
                var closeDist = _elementPos(closeElement).DistanceTo(pos);
                var plusElementsDists = elementsPlus
                                        .Select(e => _elementPos(e).DistanceTo(pos))
                                        .OrderBy(f => f);
                if(plusElementsDists.ElementAt(0) >= closeDist) return closeElement;
                else
                {
                    searchRadius++;
                    elements = elementsPlus; 
                }
            }
            else
            {
                if(searchRadius >= _partitionsPerAxis)
                {
                    go = false;
                }
                else
                {
                    elements = elementsPlus;
                    searchRadius++;
                }
            }
        }
        return default(T);
    }

    private void AddElement(T t)
    {
        var pos = _elementPos(t);
        var coords = GetCoordsFromPos(pos);
        Partitions[(int)coords.x,(int)coords.y].Add(t);
    }
    private Vector2 GetCoordsFromPos(Vector2 pos)
    {
        int x = Mathf.FloorToInt(pos.x / _partitionWidth);
        int y = Mathf.FloorToInt(pos.y / _partitionHeight); 
        return new Vector2(x,y);
    }
    private List<T> GetElementsAtPos(Vector2 pos, int searchRadius)
    {
        var coords = GetCoordsFromPos(pos);
        var list = new List<T>();
        for (int i = -searchRadius; i <= searchRadius; i++)
        {
            for (int j = -searchRadius; j <= searchRadius; j++)
            {
                int x = (int)coords.x + i;
                int y = (int)coords.y + j;
                if( x < _partitionsPerAxis && x >= 0 && y < _partitionsPerAxis && y >= 0 )
                {
                    list.AddRange(Partitions[x,y]);
                }
            }
        }
        return list; 
    }
}
