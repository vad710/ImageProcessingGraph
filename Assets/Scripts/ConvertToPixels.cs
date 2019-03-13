using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class ConvertToPixels : ImageProcessingNode {

	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableFloats RedValues;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableFloats BlueValues;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableFloats GreenValues;

	[Output] public EnumerableColors RGBPixels;

	public override void OnNodeUpdated()
	{
		//Converting to Pixels
		Debug.Log("Converting to Pixels...");
		
		var redValues = this.GetInputValue<EnumerableFloats>("RedValues");
		var blueValues = this.GetInputValue<EnumerableFloats>("BlueValues");
		var greenValues = this.GetInputValue<EnumerableFloats>("GreenValues");

		if (redValues != null && blueValues != null && greenValues != null)
		{
			this.RGBPixels = new EnumerableColors(this.IntensityToPixels(redValues.GetEnumerable(), greenValues.GetEnumerable(), blueValues.GetEnumerable()));	
		}
	}

	private IEnumerable<Color> IntensityToPixels(IEnumerable<float> redValues, IEnumerable<float> blueValues, IEnumerable<float> greenValues)
	{
		var redValueArray = redValues.ToArray();
		var blueValueArray = blueValues.ToArray();
		var greenValueArray = greenValues.ToArray();
		
		if (redValueArray.Length == blueValueArray.Length && blueValueArray.Length == greenValueArray.Length)
		{
			for (var i = 0; i < redValueArray.Length ; i++)
			{
				yield return new Color(redValueArray[i], greenValueArray[i], blueValueArray[i]);
			}		
		}
	}
	
	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) 
	{
		return this.RGBPixels;
	}
}