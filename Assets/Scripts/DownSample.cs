
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DownSample : ImageProcessingNode
{
    
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats Values;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Width;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Height;

    [Input] public int Scale = 2;
    
    [Output] public EnumerableFloats Results;
    [Output] public int NewWidth;
    [Output] public int NewHeight;
    
    public override object GetValue(NodePort port)
    {

        switch (port.fieldName)
        {
            case "Results":
                return this.Results;

            case "NewWidth":
                return this.NewWidth;

            case "NewHeight":
                return this.NewHeight;
        }
        
        return null;
    }

    public override void OnNodeUpdated()
    {
        var width = this.GetInputValue<int>("Width", this.Width);
        var height = this.GetInputValue<int>("Height", this.Height);
        var values = this.GetInputValue<EnumerableFloats>("Values", this.Values);
        var scale = this.GetInputValue<int>("Scale", this.Scale);

        if (!this.IsPowerOfTwo(scale))
        {
            Debug.LogWarning("Scale must be a power of 2!");
        }
        
        this.Results = new EnumerableFloats(SubSample(values.GetEnumerable(),height, width, scale));

        this.NewHeight = height / scale;
        this.NewWidth = width / scale;
    }

    private bool IsPowerOfTwo (int val) 
    {
        return val != 0 && ((val & (val - 1)) == 0); 
          
    } 
    
    private IEnumerable<float> SubSample(IEnumerable<float> source, int height, int width, int scale)
    {
        using (var values = source.GetEnumerator())
        {
            for (var y = 0; y < height; y++)
            {    
                var skipColumn = y % scale > 0;
                
                for (var x = 0; x < width; x++)
                {
                    values.MoveNext();
                    
                    if (!skipColumn && x % scale == 0)
                    {
                        yield return values.Current;
                    }
                }
            }
        }
    }
}
