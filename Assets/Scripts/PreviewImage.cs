﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.Networking.PlayerConnection;
using UnityEngine;
using XNode;

public class PreviewImage : ImageProcessingNode
{

	[Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Width;
	[Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public int Height;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableColors RGBPixels;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public EnumerableFloats GrayscalePixels;

	public string FileName;
	
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
		var colorEnumerable = this.GetInputValue<EnumerableColors>("RGBPixels");
		
		if (height == 0 || width == 0 || (grayscale == null && colorEnumerable == null))
		{
			Debug.Log("Input data is invalid");

			this.IsValid = false;
			this.Image = null;
			return;
		}

		IEnumerable<Color> colors;
		if (grayscale != null && grayscale.GetEnumerable() != null)
		{
			colors = grayscale.GetEnumerable().Select(v => new Color(v,v,v));
		}
		else if (colorEnumerable != null && colorEnumerable.GetEnumerable() != null)
		{
			colors = colorEnumerable.GetEnumerable();
		}

		else
		{
			//ERROR CONDITION
			colors = Enumerable.Repeat(Color.cyan, height * width);
		}
		
		this.Image = new Texture2D(width, height);

		using (var colorEnum = colors.GetEnumerator())
		{
			for (var y = 0; y < height; y++)
			{
				for (var x = 0; x < width; x++)
				{
					colorEnum.MoveNext();
					var pixel = colorEnum.Current;
					
					this.Image.SetPixel(x,y,pixel);
				}		
			}
		}
		
		this.Image.Apply();
		this.IsValid = true;

		return;
	}
	
}

