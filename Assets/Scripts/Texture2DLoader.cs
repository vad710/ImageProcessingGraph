using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Texture2DLoader : ImageProcessingNode 
{
	public Texture2D texture;
	[Output] public EnumerableColors RGBPixels;
	[Output] public int Width;
	[Output] public int Height;
	
	public bool Crop;
	public RectInt CropRect = new RectInt(0,0, 250, 250);

	[NonSerialized]
	public Texture2D PreviewTexture;
	
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
				return this.GetWidth();
			}

			if (port.fieldName == "Height")
			{
				return this.GetHeight();
			}
		}

		return null;

	}

	public override void OnNodeUpdated()
	{
		Debug.Log("Reloading pixels in Texture2DLoader");

		if (this.texture != null && this.texture.width > 0)
		{
			Color[] pixels;
			
			if (this.Crop)
			{
				pixels = this.texture.GetPixels(this.CropRect.x, this.CropRect.y, this.CropRect.width,
					this.CropRect.height);
			}
			else
			{
				pixels = this.texture.GetPixels();	
			}
			
			this.PreviewTexture = new Texture2D(this.GetWidth(), this.GetHeight());
			this.PreviewTexture.SetPixels(pixels);
			this.PreviewTexture.Apply();
			
			this.RGBPixels = new EnumerableColors(pixels);	
		}
	}

	private int GetHeight()
	{
		if (this.Crop)
		{
			return this.CropRect.height;
		}
		return this.texture.height;
	}

	private int GetWidth()
	{
		if (this.Crop)
		{
			return this.CropRect.width;
		}
				
		return this.texture.width;
	}
}

