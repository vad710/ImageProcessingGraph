using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(Kernel))]
public class KernelEditor : NodeEditor
{
    private float firstValue;
    
    public override void OnBodyGUI() 
    {
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Values"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Height"));
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Average"));

//        var rect = UnityEditor.EditorGUILayout.GetControlRect(false, 170);
//        var node = target as PreviewImage;
//
//        if (node != null && node.Image != null)
//        {
//            UnityEditor.EditorGUI.DrawPreviewTexture(rect, node.Image );    
//        }
        
        EditorGUILayout.LabelField("Kernel Matrix Values");
        var node = target as Kernel;
        if (node != null)
        {
           var kernel = node.KernelValues;

           if (kernel != null)
           {
               var kernelWidth = kernel.GetLength(0);
               var kernelHeight = kernel.GetLength(1);

               for (var x = 0; x < kernelWidth; x++)
               {
                   GUILayout.BeginHorizontal();
               
                   for (var y = 0; y < kernelHeight; y++)
                   {
                       GUILayout.BeginVertical();
                       kernel[y,x] = EditorGUILayout.FloatField( kernel[y,x]);
                       GUILayout.EndVertical();
                   }
               
                   GUILayout.EndHorizontal();
               }                 
           }
        }
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Results"));
    }
}
