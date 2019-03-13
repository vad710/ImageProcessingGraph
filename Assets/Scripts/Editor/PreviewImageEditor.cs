using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XNodeEditor;

[CustomNodeEditor(typeof(PreviewImage))]
public class PreviewImageEditor : NodeEditor
{    
    public override void OnBodyGUI() 
    {
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("RGBPixels"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Height"));
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Crop"));

        var rect = UnityEditor.EditorGUILayout.GetControlRect(false, 170);
        var node = target as PreviewImage;

        if (node != null && node.Image != null)
        {
            UnityEditor.EditorGUI.DrawPreviewTexture(rect, node.Image );
            
            
            if (GUILayout.Button("Save"))
            {
                this.OnSaveClick(node.Image);
            }
        }
    }

    public void OnSaveClick(Texture2D image)
    {
        Debug.Log("Saving Image");
        
        var jpg =  image.EncodeToJPG();
        var file = new FileInfo(@"preview.jpg");
        using (var writer = file.OpenWrite())
        {
            writer.Write(jpg, 0, jpg.Length);
            writer.Close();
        }

    }
}