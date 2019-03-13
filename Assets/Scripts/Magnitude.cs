using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class Magnitude : ImageProcessingNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableFloats XValues;
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableFloats YValues;

    
    [Output] public EnumerableFloats MagnitudeValues;

    public override void OnNodeUpdated()
    {
        var xValues = this.GetInputValue<EnumerableFloats>("XValues");
        var yValues = this.GetInputValue<EnumerableFloats>("YValues");


        if (xValues != null && yValues != null)
        {
            var magnitudes = CalculateMagnitudes(xValues.GetEnumerable(), yValues.GetEnumerable());
            
            this.MagnitudeValues = new EnumerableFloats(magnitudes);	
        }
    }

    private IEnumerable<float> CalculateMagnitudes(IEnumerable<float> xValues, IEnumerable<float> yValues)
    {
        var xValuesArray = xValues.ToArray();
        var yValuesArray = yValues.ToArray();
		
        if (xValuesArray.Length == yValuesArray.Length )
        {
            for (var i = 0; i < xValuesArray.Length ; i++)
            {
                yield return Mathf.Sqrt((xValuesArray[i] * xValuesArray[i]) + (yValuesArray[i] * yValuesArray[i]));
            }		
        }
    }
    
    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) 
    {
        return this.MagnitudeValues;
    }
}
