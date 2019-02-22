using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Multiply : ImageProcessingNode
{
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats Values;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public float MultiplyBy;
    [Output] public EnumerableFloats Results;


    public override object GetValue(NodePort port)
    {
        return this.Results;
    }

    private IEnumerable<float> Mutliply(IEnumerable<float> values, float multiplyBy)
    {
        foreach (var value in values)
        {
            yield return value * multiplyBy;
        }
    }
    
    public override void OnNodeUpdated()
    {
        Debug.Log("Multiplying...");
        
        var multiplyBy = this.GetInputValue("MultiplyBy", this.MultiplyBy);
        var values = this.GetInputValue("Values", this.Values);
        
        this.Results = new EnumerableFloats(this.Mutliply(values.GetEnumerable() , multiplyBy));
    }
}
