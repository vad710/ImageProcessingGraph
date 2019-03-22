using System.Security.Cryptography;
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

        
        //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Size"));
        
        var node = target as Kernel;
        if (node != null)
        {

            var selectedSize = (Kernel.KernelSize)EditorGUILayout.EnumPopup("Size", node.Size);
            if (selectedSize != node.Size)//user is changing the size of the kernel
            {
                Debug.Log("changing size of kernel");

                if (selectedSize == Kernel.KernelSize.Three)
                {
                    node.KernelValues = node.KernelValues3x3;
                }
                else
                {
                    node.KernelValues = node.KernelValues5x5;
                }
                
                node.Size = selectedSize;
            }
             
            EditorGUILayout.LabelField("Kernel Matrix Values");
    
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
            else
            {
                EditorGUILayout.LabelField("Kernel is Null");
            }
        }
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Results"));
    }
    
    public override int GetWidth()
    {
        return 300;
    }
}
