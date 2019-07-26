using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class PreviewImage : ImageProcessingNode
{

	[Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Width;
	[Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Height;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableColors RGBPixels;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableFloats GrayscalePixels;
	
	
	[NonSerialized]
	public Texture2D Image;

	[NonSerialized] 
	public bool IsValid;
	

	protected override void Init()
	{

		if (this.Image == null)
		{
			this.RefreshPreviewImage();	
		}
		this.name = "Preview Image";
		
		base.Init();
	}

	public override void OnNodeUpdated()
	{
		this.RefreshPreviewImage();
	}

	private void RefreshPreviewImage()
	{
		Debug.Log("Preview image updating...!");
		
		var width = this.GetInputValue<int>("Width", this.Width);
		var height = this.GetInputValue<int>("Height", this.Height);

		var grayscale = this.GetInputValue<EnumerableFloats>("GrayscalePixels");
		var colors = this.GetInputValue<EnumerableColors>("RGBPixels");
		
		if (height == 0 || width == 0 || (grayscale == null && colors == null))
		{
			Debug.Log("Input data is invalid");

			this.IsValid = false;
			this.Image = null;
			return;
		}

		
		this.Image = new Texture2D(width, height);	
		
		Color[] pixelBuffer;
		
		if (grayscale != null && grayscale.GetEnumerable() != null)
		{
			Debug.Log("converting grayscale to rgb");
	        
			var pixels = grayscale.GetEnumerable().ToArray();

			pixelBuffer = new Color[pixels.Length];
			
			//TODO: We can probably convert to rgb and use SetPixel right into the image instead of doing an extra loop
			for (int i = 0; i < pixels.Length; i++)
			{
				pixelBuffer[i] = new Color(pixels[i],pixels[i],pixels[i]);	
			}                
		}
		else if (colors != null && colors.GetEnumerable() != null)
		{
			Debug.Log("Getting rgb pixels...");
	        
			pixelBuffer = colors.GetEnumerable().ToArray();
		}
		else
		{
			Debug.Log("No pixel data available from source...");
			pixelBuffer = new Color[0];
		}

		if (pixelBuffer.Length > 0)
		{
			
            
			this.Image.SetPixels(pixelBuffer );

			this.Image.Apply();
			this.IsValid = true;
		}
	}
}

