
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using XNode;

public class Threshold : ImageProcessingNode
{
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats Values;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public float ThresholdValue;
    [Output] public EnumerableFloats Results;

    public override object GetValue(NodePort port)
    {
        return this.Results;
    }
    
    
    public override void OnNodeUpdated()
    {
        Debug.Log("Updating Threshold...");
        
        var values = this.GetInputValue("Values", this.Values);
        var threshold = this.GetInputValue("ThresholdValue", this.ThresholdValue);

        
        this.Results = new EnumerableFloats(this.DoThreshold(values.GetEnumerable() , threshold));
    }

    private IEnumerable<float> DoThreshold(IEnumerable<float> inputValues, float threshold)
    {
        foreach (var value in inputValues)
        {
            if (value > threshold)
            {
                yield return 1;
            }
            else
            {
                yield return 0;
            }
        }
    }
}
