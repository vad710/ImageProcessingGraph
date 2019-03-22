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
		
		var grayscale = this.GetInputValue<EnumerableFloats>("GrayscalePixels");
		var colors = this.GetInputValue<EnumerableColors>("RGBPixels");

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
			pixelBuffer = new Color[0];
		}

		if (pixelBuffer.Length > 0)
		{
			//Debug.Log("Pixel Count " + pixels.Length);
            
			this.Image.SetPixels(pixelBuffer );

			if (crop)
			{
				Debug.Log("Cropping...");
                
				var cropWidth = 250;
				var cropHeight = 250;
                
				var croppedImage = new Texture2D(cropWidth,cropHeight);

				var cropX = (this.Image.width) / 3;
				var cropY = ((this.Image.height) / 5) * 4;

				var croppedPixels = this.Image.GetPixels(cropX, cropY, cropWidth, cropHeight);

				croppedImage.SetPixels(croppedPixels );
				this.Image = croppedImage;
			}
            
            
			this.Image.Apply();
			   
		}

	}
}

