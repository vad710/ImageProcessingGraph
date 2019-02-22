using System;
using System.Collections.Generic;
using System.Linq;
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
        

        var rect = UnityEditor.EditorGUILayout.GetControlRect(false, 170);
        var node = target as PreviewImage;

        if (node != null && node.Image != null)
        {
            UnityEditor.EditorGUI.DrawPreviewTexture(rect, node.Image );    
        }
    }
}