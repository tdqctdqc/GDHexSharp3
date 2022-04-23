using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class QuadTreeHolder<T> 
{
    private List<QuadTreeNode<T>> _list;
    private Func<T, Vector2> _elementPos;
    public QuadTreeNode<T> Root { get; private set; }
    public List<T> Elements { get; private set; }
    public List<QuadTreeNode<T>> ElementNodes { get; private set; }
    public List<QuadTreeNode<T>> Nodes { get; private set; }

    public QuadTreeHolder(int maxElementsInCell, List<T> elements, Rectangle bounds, Func<T, Vector2> elementPos)
    {
        _list = new List<QuadTreeNode<T>>();
        _elementPos = elementPos;
        Elements = new List<T>();
        ElementNodes = new List<QuadTreeNode<T>>();
        Nodes = new List<QuadTreeNode<T>>();
        Root = ConstructNode(maxElementsInCell, elements, bounds, 0);
        Nodes.Add(Root);
    }
    public void AddNode(QuadTreeNode<T> node)
    {
        node.ID = Nodes.Count; 
        Nodes.Add(node);
    }
    public QuadTreeNode<T> ConstructNode(int maxElementsInCell, List<T> elements, Rectangle bounds, int level, QuadTreeNode<T> parent = null)
    {
        var node = new QuadTreeNode<T>();
        node.Holder = this; 
        node.Bounds = bounds; 
        node.Parent = parent; 
        node.Level = level;
        //node.Holder.Nodes.Add(node);

        if(node.Parent == null){ node.HasParent = false; }
        else { node.HasParent = true; }
    
        elements = new List<T>(elements);

        if(elements.Count == 0)
        {
            node.HasElement = false; 
            node.IsLeaf = true; 
            node.ChildCount = 0;
        }
        else if(elements.Count <= maxElementsInCell)
        {
            //GD.Print("Child count in leaf: " + elements.Count);
            node.IsLeaf = true; 
            node.HasElement = true; 
            node.ChildCount = elements.Count;
            node.FirstChildID = Elements.Count;
            for (int i = 0; i < elements.Count; i++)
            {
                Elements.Add(elements[i]);
                ElementNodes.Add(node);
            }
        }
        else if(elements.Count > maxElementsInCell)
        {
            node.IsLeaf = false; 
            node.HasElement = true;

            var newBounds = bounds.Divide();

            var tlElements = elements.Where(e => newBounds[0].Contains(_elementPos(e))).ToList();
            elements = elements.Except(tlElements).ToList();

            var trElements = elements.Where(e => newBounds[1].Contains(_elementPos(e))).ToList();
            elements = elements.Except(trElements).ToList();

            var blElements = elements.Where(e => newBounds[2].Contains(_elementPos(e))).ToList();
            elements = elements.Except(blElements).ToList();

            var brElements = elements.Where(e => newBounds[3].Contains(_elementPos(e))).ToList();
            elements = elements.Except(brElements).ToList();

            if(elements.Count > 0)
            {
                throw new Exception("not all elements accounted for");
            }
            var tl = ConstructNode(maxElementsInCell, tlElements, newBounds[0], level + 1, node);
            var tr = ConstructNode(maxElementsInCell, trElements, newBounds[1], level + 1, node);
            var bl = ConstructNode(maxElementsInCell, blElements, newBounds[2], level + 1, node);
            var br = ConstructNode(maxElementsInCell, brElements, newBounds[3], level + 1, node);

            AddNode(tl);
            AddNode(tr);
            AddNode(bl);
            AddNode(br);
            node.ChildCount = 4;
            node.FirstChildID = tl.ID;
        }
        return node; 
    }

    public QuadTreeNode<T> GetNode(int id)
    {
        if(id < 0 || id >= Nodes.Count) return null;
        return Nodes[id];
    }

    public QuadTreeNode<T> GetElementHavingCellAtPoint(Vector2 point, int maxLevel = int.MaxValue)
    {
        var node = Root;
        return GetElementHavingCellAtPoint(node, point, maxLevel);
    }
    public QuadTreeNode<T> GetCellAtPoint(Vector2 point, int maxLevel = int.MaxValue)
    {
        var node = Root;
        return GetCellAtPoint(node, point, maxLevel);
    }
    public QuadTreeNode<T> GetCellAtPoint(QuadTreeNode<T> node, Vector2 point, int maxLevel = int.MaxValue)
    {
        if(node.Bounds.Contains(point) == false)
        {
            if(node.HasParent == false) return null;
            return GetCellAtPoint(node.Parent, point, maxLevel);
        }
        if(node.IsLeaf || node.Level == maxLevel)
        {
            return node;
        }

        for (int i = 0; i < node.ChildCount; i++)
        {
            var child = Nodes[node.FirstChildID + i];
            if(child.Bounds.Contains(point)) return GetCellAtPoint(child, point, maxLevel); 
        }
        return node;
    }
    public QuadTreeNode<T> GetElementHavingCellAtPoint(QuadTreeNode<T> node, Vector2 point, int maxLevel = int.MaxValue)
    {
        if(node.Bounds.Contains(point) == false)
        {
            //GD.Print("point out of bounds");
            if(node.HasParent == false) return null;
            return GetElementHavingCellAtPoint(node.Parent, point, maxLevel);
        }
        if(node.HasElement)
        {
            if(node.IsLeaf || node.Level == maxLevel)
            {
                //GD.Print("node is leaf with element, returning node");
                return node;
            }
            //GD.Print("node is not leaf with element, looking for child");

            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = Nodes[node.FirstChildID + i];
                if(child.Bounds.Contains(point) && child.HasElement)
                {    
                    //GD.Print("found child, returning child");
                    return GetElementHavingCellAtPoint(child, point, maxLevel);
                } 
            }
            
            //GD.Print("did not find child, returning node");
            
            return node; 
        }
        else 
        {
            //GD.Print("node has no element, returning parent");

            if(node.HasParent == false) return null;
            return GetElementHavingCellAtPoint(node.Parent, point, maxLevel);
        }
    }

    public T GetClosestElement(Vector2 point, int maxLevel = int.MaxValue)
    {
        return GetClosestElement(Root, point, maxLevel);
    }
    public T GetClosestElement(QuadTreeNode<T> node, Vector2 point, int maxLevel = int.MaxValue)
    {

        if(node.Bounds.Contains(point) == false)
        {
            //GD.Print("point is out of bounds, returning null");
            return default(T);
        }
        var current = GetElementHavingCellAtPoint(node, point);
        if(current == null) 
        {
            //GD.Print("could not find smallest element having cell, returning null");
            return default(T);
        }
        else if(current.HasElement == false)
        {
            //GD.Print("smallest element having cell did not have element, returning null");
            return default(T);
        }
        
        T e = default(T);
        float dist = Mathf.Inf;
        if(current.IsLeaf)
        {
            //GD.Print("is leaf");
            for (int i = 0; i < current.ChildCount; i++)
            {
                var cand = Elements[current.FirstChildID + i];
                if(_elementPos(cand).DistanceSquaredTo(point) < dist)
                {
                    e = cand; 
                }
            }
        }
        else
        {
            //GD.Print("no leaf");
        }
        



        var ePos = _elementPos(e);
        float maxDist = point.DistanceTo(ePos);
        
        var tl = new Vector2(point.x - maxDist, point.y - maxDist);
        var br = new Vector2(point.x + maxDist, point.y + maxDist);
        var box = new Rectangle(tl, br);
        
        var list = new List<T>();
        list = GetAllElementsInBounds(list, Root, box);
        if(list.Count == 0)
        {
            return e;
        }
        
        return list.OrderBy( f => _elementPos(f).DistanceSquaredTo(point)).First();
    }
    private List<T> GetAllElementsInBounds(List<T> elements, QuadTreeNode<T> node, Rectangle bounds)
    {
        if(node.HasElement)
        {
            if(node.IsLeaf)
            {
                for (int i = 0; i < node.ChildCount; i++)
                {
                    //GD.Print("child id: "+ node.FirstChildID + i);
                    var e = Elements[node.FirstChildID + i];
                    if(bounds.Contains(_elementPos(e))) elements.Add(e);
                }
            }
            else
            {
                var tl = Nodes[node.FirstChildID];
                elements.AddRange(GetAllElementsInBounds(elements, tl, bounds));
                var tr = Nodes[node.FirstChildID + 1];
                elements.AddRange(GetAllElementsInBounds(elements, tr, bounds));
                var bl = Nodes[node.FirstChildID + 2];
                elements.AddRange(GetAllElementsInBounds(elements, bl, bounds));
                var br = Nodes[node.FirstChildID + 3];
                elements.AddRange(GetAllElementsInBounds(elements, br, bounds));
            }
        }
        return elements;
    }
}
