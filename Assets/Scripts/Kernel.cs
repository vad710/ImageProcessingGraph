using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class Kernel : ImageProcessingNode
{
    //Vertical Kernel
    public float[,] kernel = {
        {-1, -2, -1},
        { 0,  0,  0},
        { 1,  2,  1}
    };
    
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats Values;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Width;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Height;
    [Output] public EnumerableFloats Results;
    
    
    
    private IEnumerable<float> RunKernel(IEnumerable<float> values, int width, int height, float[,] kernel)
    {
        var kernelWidth = kernel.GetLength(0);
        var kernelHeight = kernel.GetLength(1);
        var kernelCenter = Mathf.FloorToInt(kernelWidth / 2f);


        var xOffset = kernel.GetLength(0) - 1;
        var yOffset = kernel.GetLength(1) - 1;
        
        var valuesArray = values.ToArray();
        var temp = new float[width * height];
        
        for (var y = yOffset; y < height - yOffset; y++)
        {
            for (var x = xOffset; x < width - xOffset; x++)
            {
                var index = (y * width) + x;

                var kernelYDirection = 1;
                var kernelXDirection = 1;

                var kernelResult = 0f;

                //Apply the kernel...
                for (var kernelY = 0; kernelY < kernelWidth; kernelY++)
                {
                    if (kernelY < kernelCenter)
                    {
                        kernelYDirection = -1;
                    }

                    for (var kernelX = 0; kernelX < kernelHeight; kernelX++)
                    {
                        if (kernelX < kernelCenter)
                        {
                            kernelXDirection = -1;
                        }
                        
                        var kernelValue = kernel[kernelX, kernelY];
                        
                        var kernelIndex = index + (kernelY * width * kernelYDirection) + (kernelX * kernelXDirection);

                        kernelResult += valuesArray[kernelIndex] * kernelValue;

                    }
                }

                var pixel = Mathf.Clamp01(kernelResult);
                temp[index] = pixel;
                //yield return pixel;
            }    
        }

        //TODO: Go back to using yield return for performance, but need to make sure that we return the correct number of pixels
        // simply doing a yield return from within the loop will result in a smaller array than needed due to the fact that the 
        // kernel does not get applied to the borders of the image
        return temp;
    }

    public override object GetValue(NodePort port)
    {
        return this.Results;
    }

    public override void OnNodeUpdated()
    {
        var values = this.GetInputValue("Values", this.Values);
        var width = this.GetInputValue<int>("Width", this.Width);
        var height = this.GetInputValue<int>("Height", this.Height);

        if (values != null && values.GetEnumerable() != null)
        {
            this.Results = new EnumerableFloats(this.RunKernel(values.GetEnumerable(), width, height, kernel));    
        }
    }
}

