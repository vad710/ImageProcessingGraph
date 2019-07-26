
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(Texture2DLoader))]
public class Texture2DLoaderEditor : NodeEditor
{
    public override void OnBodyGUI() 
    {
        // Update serialized object's representation
        serializedObject.Update();
        
        var options = new[]
        {

            GUILayout.ExpandWidth(true),
            GUILayout.MinWidth(30)
        };
        
        EditorGUIUtility.labelWidth = 84;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("texture"), null, false, options);
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("RGBPixels"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Height"));
        
        var node = target as Texture2DLoader;

        if (node != null)
        {

            node.Crop = (GUILayout.Toggle(node.Crop, "Crop Image"));

            var previewTexture = node.texture;
            
            if (node.Crop)
            {
                node.CropRect = EditorGUILayout.RectIntField("Crop Rectangle", node.CropRect);  
                
//                if (node.CropRect.x >= 0 && node.CropRect.y >= 0 && node.CropRect.width <= node.texture.width && node.CropRect.height <= node.CropRect.height)
//                {
//                    var croppedPixels = node.texture.GetPixels(node.CropRect.x, node.CropRect.y, node.CropRect.width,
//                        node.CropRect.height);
//                    //(cropX, cropY, cropWidth, cropHeight);
//                    
//                    var croppedImage = new Texture2D(node.CropRect.width, node.CropRect.height);
//                    
//                    croppedImage.SetPixels(croppedPixels );
//                    previewTexture = croppedImage;
//                }
//                else
//                {
//                    Debug.LogError("Invalid Crop Rect!");
//                }
                
            }   
            
            
            var rect = UnityEditor.EditorGUILayout.GetControlRect(false, 170);
            
            
            
            UnityEditor.EditorGUI.DrawPreviewTexture(rect, node.PreviewTexture );
        }
        

    
        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
    
}
