using System;
using UnityEngine;
using XNode;

public class Kernel : ImageProcessingNode 
{
    [Output] public KernelValue kernel;

    public Kernel()
    {
        this.ManualUpdate = true;
    }
    
    public override object GetValue(NodePort port)
    {
        return this.kernel;
    }
}

[Serializable] 
public class KernelValue : ISerializationCallbackReceiver
{
    private readonly float[,] kernelValues5X5 = new float[,] {{1,1,1,1,1}, {1,1,1,1,1}, {1,1,1,1,1}, {1,1,1,1,1}, {1,1,1,1,1}};
    private readonly float[,] kernelValues3X3 = new float[,] {{1,1,1}, {1,1,1}, {1,1,1}};
    
    public KernelValue()
    {
        //this.Values = kernelValues3X3;
    }
    
    
    public void SetKernelSize(KernelSize size)
    {
        if (this.Size != size)
        {
            if (size == KernelSize.Five)
            {
                this.Values = kernelValues5X5;
            }
            else
            {
                this.Values = kernelValues3X3;
            }
            
            this.Size = size;
        }
    }

    [NodeEnum] 
    public KernelSize Size;
    
    public float[,] Values { get; set; }

    [HideInInspector] [SerializeField]
    private float[] _internalValues;
    
    [Serializable]
    public enum KernelSize
    {
        Three = 3,
        Five = 5
    }

    public void OnBeforeSerialize()
    {
        //Unity does not support multidimentional arrays
        //Need to convert the 2D array into a 1D array
        
        if (this.Values != null)
        {
            var kernelSize = (int)this.Size;

            _internalValues = new float[kernelSize * kernelSize];

            var indexer = 0;
            
            for (var x = 0; x < kernelSize; x++)
            {
                for (var y = 0; y < kernelSize; y++)
                {
                    _internalValues[indexer] = this.Values[x, y];
                    indexer++;
                }
            }                 
        }
    }

    public void OnAfterDeserialize()
    {
        //Unity does not support multidimentional arrays
        //Need to convert the 1D array into a 2D array
        
        if (_internalValues != null)
        {
            var kernelSize = (int)this.Size;
            
            this.Values = new float[kernelSize,kernelSize];
            
            var indexer = 0;
            
            for (var x = 0; x < kernelSize; x++)
            {
                for (var y = 0; y < kernelSize; y++)
                {
                    this.Values[x, y] = _internalValues[indexer];
                    indexer++;
                }
            }  
            
        }
    }
}