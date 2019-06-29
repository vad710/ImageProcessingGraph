using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;


public abstract class ImageProcessingNode : Node
{

    public bool ManualUpdate { get; protected set; }
    
    public virtual void OnNodeUpdated()
    {
        
    }
    
}

[Serializable]
public class EnumerableFloats : Enumerable<float>
{
    public EnumerableFloats(IEnumerable<float> initializer) : base(initializer)
    {
    }
}

[Serializable]
public class EnumerableColors : Enumerable<Color>
{
    public EnumerableColors(IEnumerable<Color> initializer) : base(initializer)
    {
    }
}

[Serializable]
public abstract class Enumerable<T>
{
    private readonly IEnumerable<T> _internal;

    protected Enumerable(IEnumerable<T> initializer)
    {
        _internal = initializer;
    }

    public virtual IEnumerable<T> GetEnumerable()
    {
        return _internal;
    }
}