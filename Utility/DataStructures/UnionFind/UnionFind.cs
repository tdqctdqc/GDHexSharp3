using Godot;
using System;
using System.Collections.Generic;
using HexWargame;
using System.Linq;

public class UnionFind<T>
{
    private Dictionary<T, int> _keyDic;
    private Dictionary<int, T> _backKeyDic;
    private Dictionary<int, List<T>> _unions; 
	private Func<T,T,bool> _linkFunc; 
	private Func<T,List<T>> _neighborFunc; 
	private Func<T,int> _getKeyFunc; 

    public UnionFind(Func<T,T,bool> linkFunc, Func<T,List<T>> neighborFunc, Func<T,int> getKeyFunc)
    {
		_getKeyFunc = getKeyFunc;
		_linkFunc = linkFunc;
		_neighborFunc = neighborFunc;
        _keyDic = new Dictionary<T, int>();
		_backKeyDic = new Dictionary<int, T>();
        _unions = new Dictionary<int, List<T>>();
    }

    public void AddElement(T element)
    {
        if(_keyDic.ContainsKey(element))
        {
            throw new Exception("already have this edge");
        }

        var neighbors = _neighborFunc(element).Where(n => _keyDic.ContainsKey(n) && _linkFunc(element, n)).ToList();
        if(neighbors.Count == 0)
        {
            _keyDic.Add(element, _getKeyFunc(element));
            _backKeyDic.Add(_getKeyFunc(element), element);
            _unions.Add(_getKeyFunc(element), new List<T>(){element});
        }
        else if(neighbors.Count == 1)
        {
            T prime = neighbors[0];
            var primeUnion = _keyDic[prime];
            var primeElements = _unions[primeUnion];

            _keyDic.Add(element, primeUnion);
            _backKeyDic.Add(_getKeyFunc(element), element);

            primeElements.Add(element);
        }
        else
        {
            T prime = neighbors[0];
            var primeUnion = _keyDic[prime];
            var primeElements = _unions[primeUnion];

            _keyDic.Add(element, primeUnion);
            _backKeyDic.Add(_getKeyFunc(element), element);

            primeElements.Add(element);

            for (int i = 1; i < neighbors.Count; i++)
            {
                T n = neighbors[i];
                var nUnion = _keyDic[n];
                if(nUnion == primeUnion) continue; 
                var nElements = _unions[nUnion];

                foreach (var nElement in nElements)
                {
                    _keyDic[nElement] = primeUnion;
                }
                _unions.Remove(nUnion);
                primeElements.AddRange(nElements);
            }
        }
    }
    public Dictionary<T, List<T>> GetUnions()
    {
        return _unions.ToDictionary(k => _backKeyDic[k.Key], k => _unions[k.Key]); 
    }
}
