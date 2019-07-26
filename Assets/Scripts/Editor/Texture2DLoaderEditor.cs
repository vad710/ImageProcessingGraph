
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
                
            }   
            
            
            var rect = UnityEditor.EditorGUILayout.GetControlRect(false, 170);
            
            UnityEditor.EditorGUI.DrawPreviewTexture(rect, node.PreviewTexture );
        }
        

    
        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
    
}
