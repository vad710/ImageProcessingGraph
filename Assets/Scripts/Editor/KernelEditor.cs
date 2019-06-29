using Editor;
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
                
                node.kernel.SetKernelSize(selectedSize);
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
        
        if (GUILayout.Button("Apply"))
        {
            this.NotifyNodes();
        }
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("kernel"));
    }

    private void NotifyNodes()
    {
        var imageProcGraphEditor = this.window.graphEditor as ImageProcessingGraphEditor;
        
        if (imageProcGraphEditor != null)
        {
            imageProcGraphEditor.ManualUpdateNotification(this.target as ImageProcessingNode);
        }
    }


    public override int GetWidth()
    {
        return 300;
    }
}
