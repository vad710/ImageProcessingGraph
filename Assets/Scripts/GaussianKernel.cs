using XNode;

public class GaussianKernel : ImageProcessingNode
{
    public float Sigma = 1;
    
    [Output] public KernelValue kernel;
    
    public override object GetValue(NodePort port)
    {
        return this.kernel;
    }
}
