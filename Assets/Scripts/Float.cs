using XNode;

public class Float : ImageProcessingNode
{
    [Node.OutputAttribute(Node.ShowBackingValue.Always)] public float Value;
    
    public override object GetValue(NodePort port)
    {
        return this.Value;
    }
}
