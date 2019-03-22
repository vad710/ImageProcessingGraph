using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class Kernel : ImageProcessingNode
{
    //Vertical Kernel
    public float[,] KernelValues;
    
    public float[,] KernelValues5x5 = new float[5,5];
    public float[,] KernelValues3x3 = new float[3, 3];
    
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats Values;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Width;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Height;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public bool Average;

    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public KernelSize Size;
    
    [Output] public EnumerableFloats Results;


    private IEnumerable<float> RunKernel(IEnumerable<float> values, int width, int height, float[,] kernel, bool doAverage)
    {
        var kernelWidth = kernel.GetLength(0);
        var kernelHeight = kernel.GetLength(1);
        var kernelCenter = Mathf.FloorToInt(kernelWidth / 2f);


        var xOffset = kernel.GetLength(0) - 1;
        var yOffset = kernel.GetLength(1) - 1;
        
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

                if (doAverage)
                {
                    kernelResult = kernelResult / kernelTotal;
                }
                
                //if we have negative data, we might be loosing some resolution here...
                var pixel = Mathf.Clamp01(Mathf.Abs(kernelResult));
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
        var doAverage = this.GetInputValue<bool>("Average", this.Average);
        
        if (values != null && values.GetEnumerable() != null)
        {
            Debug.Log("About to run kernel with doAverage: " + doAverage);
            
            this.Results = new EnumerableFloats(this.RunKernel(values.GetEnumerable(), width, height, this.KernelValues, doAverage));    
        }
    }

    public enum KernelSize
    {
        Three = 3,
        Five = 5
    }
}

