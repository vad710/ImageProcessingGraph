using System;
using XNode;

public class Kernel : ImageProcessingNode 
{
    [Output] public KernelValue kernel;

    public override object GetValue(NodePort port)
    {
        return this.kernel;
    }
}

[Serializable]
public class KernelValue
{
    public static float[,] KernelValues5x5 = new float[5,5];
    public static float[,] KernelValues3x3 = new float[3, 3];

    public KernelValue()
    {
        this.Values = KernelValues3x3;
    }
    
    public KernelSize Size { get; set; }

    public float[,] Values { get; set; }
    
    public enum KernelSize
    {
        Three = 3,
        Five = 5
    }
    
}