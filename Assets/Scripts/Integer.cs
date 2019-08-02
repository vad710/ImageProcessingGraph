using XNode;

public class Integer : ImageProcessingNode
{
    [Node.OutputAttribute(Node.ShowBackingValue.Always)] public int Value;
    
    protected override void Init()
    {
        base.name = "Input Integer";

        base.Init();
    }
    
    public override object GetValue(NodePort port)
    {
        return this.Value;
    }
}