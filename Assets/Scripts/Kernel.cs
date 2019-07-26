using System;
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
public class KernelValue
{
    private readonly float[,] kernelValues5X5 = new float[,] {{1,1,1,1,1}, {1,1,1,1,1}, {1,1,1,1,1}, {1,1,1,1,1}, {1,1,1,1,1}};
    private readonly float[,] kernelValues3X3 = new float[,] {{1,1,1}, {1,1,1}, {1,1,1}};
    
    public KernelValue()
    {
        this.Values = kernelValues3X3;
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
    
    public KernelSize Size { get; private set; }
    
    public float[,] Values { get; set; }
    
    [Serializable]
    public enum KernelSize
    {
        Three = 3,
        Five = 5
    }
    
}