using Godot;
using System;
using System.Collections.Generic;

public class PairDictionary<T,V> 
{
    public V this[T t, T r] => Get(t,r);
    //private Func<T,T,T> _highKeyFunc;
    private Dictionary<int, V> _dic; 
    public PairDictionary()
    {
        //_highKeyFunc = highKeyFunc;
        _dic = new Dictionary<int, V>();
    }
    public void Add(T key1, T key2, V value)
    {
        var key = GetKey(key1, key2);
        _dic.Add(key, value);
    }
    public bool Contains(T t, T r)
    {
        var key = GetKey(t,r);
        if(_dic.ContainsKey(key)) return true;
        return false; 
    }
    public void Set(T t, T r, V val)
    {
        if(Contains(t,r))
        {
            var key = GetKey(t,r);
            _dic[key] = val;
        }
    }
    private V Get(T t, T r)
    {
        var key = GetKey(t,r);
        if(_dic.ContainsKey(key)) return _dic[key];
        return default(V);
    }
    private int GetKey(T t1, T t2)
    {
        int hash = 17;
        hash = hash * 31 + t1.GetHashCode();
        hash = hash * 31 + t2.GetHashCode();
        return hash;
    }
}

