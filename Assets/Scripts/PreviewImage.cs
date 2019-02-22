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

	public Texture2D Image;

	protected override void Init()
	{

		if (this.Image != null)
		{
			this.Image = Texture2D.blackTexture;	
		}
		this.name = "Preview Image";
		
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	public override void OnNodeUpdated()
	{
		//Debug.Log("Render Image Updated!");
		
		var width = this.GetInputValue<int>("Width", this.Width);
		var height = this.GetInputValue<int>("Height", this.Height);

		this.Image = new Texture2D(width, height);
		
		var colors = this.GetInputValue<EnumerableColors>("RGBPixels");
		
		if (colors != null && colors.GetEnumerable() != null)
        {
	        Debug.Log("Updating pixels in Render image...");
	        
            var pixels = colors.GetEnumerable().ToArray();

            if (pixels != null)
            {
	            Debug.Log("Pixel Count " + pixels.Length);
	            
                this.Image.SetPixels(pixels );
                this.Image.Apply();
            }    
        }
	}
}

