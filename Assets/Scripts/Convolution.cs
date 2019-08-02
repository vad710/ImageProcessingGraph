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


        var xOffset = kernelCenterIndex;
        var yOffset = kernelCenterIndex; 
        
        var valuesArray = values.ToArray(); //TODO: I wonder if we can add a caching system to EnumerableFloats 

        var kernelTotal = 0f;

        if (doAverage)
        {
            foreach (var kernelValue in kernel)
            {
                kernelTotal += kernelValue;
            }
        }
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                //the specific pixel value we are working on
                var index = (y * width) + x;
                
                //Make sure we are inside of the available area for this convolution
                if (x < xOffset || x >= (width - xOffset) || y < yOffset || y >= (height - yOffset))
                {
                    //Just keep the original value if we are outside of the bounds of the convolution
                    yield return valuesArray[index];
                }
                else
                {
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
                    
                    yield return kernelResult;
                }
            }
        }
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

