using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class Convolution : ImageProcessingNode
{

    
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats Values;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Width;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Height;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public bool Average;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public KernelValue Kernel;
    
    
    [Output] public EnumerableFloats Results;


    private IEnumerable<float> RunKernel(IEnumerable<float> values, int width, int height, float[,] kernel, bool doAverage)
    {
        var kernelWidth = kernel.GetLength(0);
        var kernelHeight = kernel.GetLength(1);
        
        var kernelCenterIndex = Mathf.FloorToInt(kernelWidth / 2f);
        //kernelCenterindex should be 1 for a 3x3 and a 2 for 5x5 
        
        
        var xOffset = kernelCenterIndex;
        var yOffset = kernelCenterIndex; 
        
        //TODO: Remove this for optimization
        var valuesArray = values.ToArray();
        
        var temp = new float[width * height];

        var kernelTotal = 0f;

        if (doAverage)
        {
            foreach (var kernelValue in kernel)
            {
                kernelTotal += kernelValue;
            }
        }
        
        for (var y = yOffset; y < height - yOffset; y++)
        {
            for (var x = xOffset; x < width - xOffset; x++)
            {
                //the specific pixel value we are working on
                var index = (y * width) + x;
                

                var kernelResult = 0f;

                //Apply the kernel...
                for (var kernelY = 0; kernelY < kernelWidth; kernelY++)
                {
                    
                    var kernelYOffset = kernelY - kernelCenterIndex;
                    
                    for (var kernelX = 0; kernelX < kernelHeight; kernelX++)
                    {
                        var kernelXOffset = kernelX - kernelCenterIndex;
                        
                        var kernelIndex = index + (width * kernelYOffset) + (kernelXOffset);

                        kernelResult += valuesArray[kernelIndex] * kernel[kernelX, kernelY];

                    }
                }

                if (doAverage)
                {
                    kernelResult = kernelResult / kernelTotal;
                }
                
                //if we have negative data, we might be loosing some resolution here...
                //var kernelResult = Mathf.Clamp01(Mathf.Abs(kernelResult));
                temp[index] = kernelResult;
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
        var doAverage = this.GetInputValue<bool>("Average", this.Average);
        var kernel = this.GetInputValue<KernelValue>("Kernel", this.Kernel);
        
        if (values != null && values.GetEnumerable() != null)
        {
            Debug.Log("About to run kernel with doAverage: " + doAverage);
            
            this.Results = new EnumerableFloats(this.RunKernel(values.GetEnumerable(), width, height, kernel.Values, doAverage));    
        }
    }

    public enum KernelSize
    {
        Three = 3,
        Five = 5
    }
}

