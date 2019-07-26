using System.IO;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(PreviewImage))]
public class PreviewImageEditor : NodeEditor
{    
    public override void OnBodyGUI() 
    {
        // Update serialized object's representation
        serializedObject.Update();
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("GrayscalePixels"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("RGBPixels"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Height"));

        var rect = UnityEditor.EditorGUILayout.GetControlRect(false, 170);
        var node = target as PreviewImage;


        var enabled = false;
        var previewTexture = Texture2D.blackTexture;
        
        if (node != null && node.IsValid)
        {
            previewTexture = node.Image;
            enabled = true;
        }
         

        GUI.enabled = enabled;
        
        UnityEditor.EditorGUI.DrawPreviewTexture(rect, previewTexture );

        if (GUILayout.Button("Save"))
        {
            this.OnSaveClick(node.Image);
        }

        GUI.enabled = true;
        
        
        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
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