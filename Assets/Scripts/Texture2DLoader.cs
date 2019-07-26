using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Texture2DLoader : ImageProcessingNode 
{
	public Texture2D texture;
	//[Output] public Color[] RGBPixels;
	[Output] public EnumerableColors RGBPixels;
	[Output] public int Width;
	[Output] public int Height;
	
	protected override void Init()
	{
		base.name = "Texture2D Loader";
		
		this.OnNodeUpdated();
		
		base.Init();
	}
	
	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		if (this.texture != null)
		{	
			if (port.fieldName == "RGBPixels")
			{
				//return new TextureToColors(this.texture);
				//return this.texture.GetPixels();
				return this.RGBPixels;
			}
			
			if (port.fieldName == "Width")
			{
				return this.texture.width;
			}

			if (port.fieldName == "Height")
			{
				return this.texture.height;
			}
		}

		return null;

	}

	public override void OnNodeUpdated()
	{
		Debug.Log("Reloading pixels in Texture2DLoader");

		if (this.texture != null && this.texture.width > 0)
		{
			this.RGBPixels = new EnumerableColors(this.texture.GetPixels());	
		}
	}
}

