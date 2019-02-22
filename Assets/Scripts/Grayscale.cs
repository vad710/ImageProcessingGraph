using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class Grayscale : ImageProcessingNode {

	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableColors RGBPixels;
	[Output] public EnumerableFloats IntensityValues;
	

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return this.IntensityValues;
	}

	public override void OnNodeUpdated()
	{
		Debug.Log("Converting pixels to greyscale intensities...");

		var colors = this.GetInputValue<EnumerableColors>("RGBPixels");
		
		if (colors != null && colors.GetEnumerable() != null)
		{	        
			this.IntensityValues = new EnumerableFloats(this.GetGrayscaleValues(colors.GetEnumerable()));
		}
	}

	private IEnumerable<float> GetGrayscaleValues(IEnumerable<Color> pixels)
	{
		if (pixels != null)
		{
			foreach (var pixel in pixels)
			{
				yield return (pixel.r + pixel.g + pixel.b) / 3;
			}
		}   
	}
	

}

