using XNode;

public class Float : ImageProcessingNode
{
    [Node.OutputAttribute(Node.ShowBackingValue.Always)] public float Value;
    
    protected override void Init()
    {
        base.name = "Input Float";

        base.Init();
    }
    
    public override object GetValue(NodePort port)
    {
        return this.Value;
    }
}
