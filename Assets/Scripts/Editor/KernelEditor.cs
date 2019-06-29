using UnityEditor;
using UnityEngine;
using XNodeEditor;


[CustomNodeEditor(typeof(Kernel))]
public class KernelEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        var node = target as Kernel;
        if (node != null)
        {
            var selectedSize = (KernelValue.KernelSize)EditorGUILayout.EnumPopup("Size", node.kernel.Size);
            if (selectedSize != node.kernel.Size)//user is changing the size of the kernel
            {
                Debug.Log("changing size of kernel");
                
                
                //TODO: can we push this logic into the KernelValue class?
                if (selectedSize == KernelValue.KernelSize.Three)
                {
                    node.kernel.Values = KernelValue.KernelValues3x3;
                }
                else
                {
                    node.kernel.Values = KernelValue.KernelValues5x5;
                }
                
                node.kernel.Size = selectedSize;
            }
            
            EditorGUILayout.LabelField("Kernel Matrix Values");
    
            var kernel = node.kernel.Values;
    
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
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("kernel"));
    }
    public override int GetWidth()
    {
        return 300;
    }
}
