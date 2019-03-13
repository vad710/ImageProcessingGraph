using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class Extrema : ImageProcessingNode
{
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats Values;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Width;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Height;
    [Output] public EnumerableFloats Results;


    public override object GetValue(NodePort port)
    {
        return this.Results;
    }

    private IEnumerable<float> FindExtremas(IEnumerable<float> values, int width, int height)
    {

        var xOffset = 3 - 1;
        var yOffset = 3 - 1;
        
        var valuesArray = values.ToArray();
        var temp = new float[width * height];

       
        //the 8 pixels immediately surrounding specific pixel will have a constant
        var indexOffsets = new[]
        {
            (width + 1) * -1,
            width * -1,
            (width - 1) * -1,
            -1,
            1,
            (width-1),
            (width),
            (width +1)
        };
        
        
        for (var y = yOffset; y < height - yOffset; y++)
        {
            for (var x = xOffset; x < width - xOffset; x++)
            {
                var index = (y * width) + x;

                var currentPixelValue = valuesArray[index];

                var isMinima = true;
                var isMaxima = true;
                
                foreach (var offsetValue in indexOffsets)
                {
                    var indexOffset = index + offsetValue;

                    if (isMinima) //we've not ruled out minima yet
                    {
                        if (currentPixelValue > valuesArray[indexOffset])
                        {
                            isMinima = false;
                        }
                    }

                    if (isMaxima) //we've not ruled out maxima yet
                    {
                        if (currentPixelValue < valuesArray[indexOffset])
                        {
                            isMaxima = false;
                        }
                    }

                    //if we've already proven that this is not a maxima or a minima, let's stop comparing
                    if (!isMaxima && !isMinima)
                    {
                        break;
                    }
                }

                if (isMaxima || isMinima)
                {
                    temp[index] = 1;
                }
                else
                {
                    temp[index] = 0;
                }
            }    
        }

        //TODO: Go back to using yield return for performance, but need to make sure that we return the correct number of pixels
        // simply doing a yield return from within the loop will result in a smaller array than needed due to the fact that the 
        // kernel does not get applied to the borders of the image
        return temp;
    }
    
    public override void OnNodeUpdated()
    {
        Debug.Log("Finding Extremas...");
        
        
        var values = this.GetInputValue("Values", this.Values);
        var width = this.GetInputValue("Width", this.Width);
        var height = this.GetInputValue("Height", this.Height);
        
        this.Results = new EnumerableFloats(this.FindExtremas(values.GetEnumerable(), width, height ));
    }
}
