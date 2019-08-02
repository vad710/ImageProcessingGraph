using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using XNode;

public class Harris : ImageProcessingNode
{
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats VerticalEdges;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public EnumerableFloats HorizontalEdges;
    
    [Output] public EnumerableFloats Results;
    
    
    public override object GetValue(NodePort port)
    {
        return this.Results;
    }

    private IEnumerable<float> DetectCorners(IEnumerable<float> horizontalEdges,IEnumerable<float> verticalEdges)
    {
        const float k = 0.04f;
        
        //the vertical and the horizontal need to be the same size!
        var iX = horizontalEdges.ToArray();
        var iY = verticalEdges.ToArray();

        var corners = new float[iX.Length];
        
        //TODO: There Should be a step where we can smooth out the window...
        
        for (var i = 0; i < iX.Length; i++)
        {
            //need to calculate the following sets of data:
            // Ix² = Ix . Ix
            // Iy² = Iy . Iy
            // Ixy = Ix . Iy

            if ((iY[i] > 1 || iY[i] < 0) || iX[i] > 1 || iX[i] < 0)
            {
                //Debug.Log("Large value");
            }
            
            
            var iXSquare = iX[i] * iX[i];
            var iYSquare = iY[i] * iY[i];
            var iXY = iY[i] * iX[i];
            
            // applying the formula det(M)-k*(trace(M)^2)
            
            
            // det(M) = (ksumpx2*ksumpy2 - ksumpxy*ksumpxy)
            var determinant = (iXSquare * iYSquare - iXY * iXY);
            
            // trace(M) = ksumpx2 + ksumpy2
            var trace = (iXSquare + iYSquare);

            // trace(M)^2
            var traceSquared = trace * trace;
            
            // k*(trace(M)^2)
            var kTraceSquared = k * traceSquared;
            
            // harris = det(M)-k*(trace(M)^2)
            corners[i] = determinant - kTraceSquared;
            
            
            //TODO: Threshold Here!
            
        }
        
        return corners;
    }
    
    public override void OnNodeUpdated()
    {
        Debug.LogError("Harris Node not finished!");
        
        var verticalEdges = this.GetInputValue("VerticalEdges", this.VerticalEdges);
        var horizontalEdges = this.GetInputValue("HorizontalEdges", this.HorizontalEdges);
        
        
        if (verticalEdges != null && verticalEdges.GetEnumerable() != null && horizontalEdges != null && horizontalEdges.GetEnumerable() != null)
        {
            Debug.Log("About to run Harris Corner Detector!");
            
            this.Results = new EnumerableFloats(DetectCorners(horizontalEdges.GetEnumerable(), verticalEdges.GetEnumerable()));    
        }
    }
}

