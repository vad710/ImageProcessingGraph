using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class Subtract : ImageProcessingNode
{
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats FirstSet;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats SecondSet;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public bool Rescale;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public bool AbsoluteValues;
    
    [Output] public EnumerableFloats Results;


    public override object GetValue(NodePort port)
    {
        return this.Results;
    }

    private IEnumerable<float> DoSubtraction(IEnumerable<float> firstValues, IEnumerable<float> secondValues, bool rescale, bool absoluteValues)
    {

        var firstValuesArray = firstValues.ToArray();
        var secondValuesArray = secondValues.ToArray();

//        var max = float.MinValue;
//        var min = float.MaxValue;
        
        const float from_min = -1f;
//        const float from_max = 1f;
//        const float to_min = 0;
//        const float to_max = 1;
        
        
        if (firstValuesArray.Length == secondValuesArray.Length)
        {
            for (var i = 0; i < firstValuesArray.Length; i++)
            {
                var val = firstValuesArray[i] - secondValuesArray[i];


                if (absoluteValues)
                {
                    val = Mathf.Abs(val);
                }
                
                if (rescale)
                {
                    //yield return (val - from_min) * (to_max - to_min) / (from_max - from_min) + to_min;
                    val =  (val - from_min) / 2;
                }
                

                yield return val;    
            }
        }
        
    }
    
    public override void OnNodeUpdated()
    {
        Debug.Log("Subtracting...");
        
        var firstSet = this.GetInputValue("FirstSet", this.FirstSet);
        var secondSet = this.GetInputValue("SecondSet", this.SecondSet);
        var rescale = this.GetInputValue("Rescale", this.Rescale);
        var absoluteValues = this.GetInputValue("AbsoluteValues", this.AbsoluteValues);
        
        this.Results = new EnumerableFloats(this.DoSubtraction(firstSet.GetEnumerable() , secondSet.GetEnumerable(), rescale, absoluteValues));
    }
}
