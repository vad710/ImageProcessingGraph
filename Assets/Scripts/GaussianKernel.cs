using System;
using System.Text;
using UnityEngine;
using XNode;

public class GaussianKernel : ImageProcessingNode
{
    public float Sigma = 1;
    
    [Output] public KernelValue kernel;
    
    public override object GetValue(NodePort port)
    {
        return this.kernel;
    }

    public override void OnNodeUpdated()
    {
        this.kernel = new KernelValue();
        this.kernel.SetKernelSize(KernelValue.KernelSize.Five);
        this.kernel.Values = CalculateGaussianKernel(5, this.Sigma);
        
        //Debug.Log(this.kernel);

    }

    public static float[,] CalculateGaussianKernel(int length, float sigma) 
    {
        //based on: https://softwarebydefault.com/2013/06/08/calculating-gaussian-kernels/
        
        var kernel = new float [length, length]; 
        var sumTotal = 0f;

        var radius = length / 2;
        
        var calculatedEuler = 1.0f / (2.0f * Mathf.PI * Mathf.Pow(sigma, 2));

        for (var filterY = -radius; 
            filterY <= radius; filterY++) 
        {
            for (var filterX = -radius; 
                filterX <= radius; filterX++) 
            {
                var distance = ((filterX * filterX) +  
                                   (filterY * filterY)) /  
                                  (2f * (sigma * sigma)); 

  
                kernel[filterY + radius,  
                        filterX + radius] =  
                    calculatedEuler * Mathf.Exp(-distance); 

  
                sumTotal += kernel[filterY + radius,  
                    filterX + radius]; 
            } 
        }
        
  
        for (var y = 0; y < length; y++) 
        { 
            for (var x = 0; x < length; x++) 
            { 
                kernel[y, x] = kernel[y, x] *  
                               (1.0f / sumTotal);
            }
        } 
        
        return kernel; 
    }
}
