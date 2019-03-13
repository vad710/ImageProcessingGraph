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

	[Input(ShowBackingValue.Unconnected, ConnectionType.Override)] public bool Crop;
	
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
		Debug.Log("Preview image updating...!");
		
		var width = this.GetInputValue<int>("Width", this.Width);
		var height = this.GetInputValue<int>("Height", this.Height);
		var crop = this.GetInputValue<bool>("Crop", this.Crop);

		this.Image = new Texture2D(width, height);
		
		var colors = this.GetInputValue<EnumerableColors>("RGBPixels");
		
		if (colors != null && colors.GetEnumerable() != null)
        {
	        Debug.Log("Updating pixels in Render image...");
	        
            var pixels = colors.GetEnumerable().ToArray();

            if (pixels != null)
            {
	            //Debug.Log("Pixel Count " + pixels.Length);
	            
                this.Image.SetPixels(pixels );

                if (crop)
                {
	                Debug.Log("Cropping...");
	                
	                var cropWidth = 250;
	                var cropHeight = 250;
	                
	                var croppedImage = new Texture2D(cropWidth,cropHeight);

	                var cropX = (this.Image.width) / 2;
	                var cropY = (this.Image.height) / 2;

	                var croppedPixels = this.Image.GetPixels(cropX, cropY, cropWidth, cropHeight);

	                croppedImage.SetPixels(croppedPixels );
	                this.Image = croppedImage;
                }
                
                
                this.Image.Apply();
                
                
            }    
        }
	}
}

