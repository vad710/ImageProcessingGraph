using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Editor
{
    [CustomNodeGraphEditor(typeof(ImageProcessingGraph))]
    public class ImageProcessingGraphEditor : NodeGraphEditor
    {
        public ImageProcessingGraphEditor()
        {
            NodeEditor.onUpdateNode += OnUpdateNode;
        }

        private void OnUpdateNode(Node node)
        {
            var imageProcNode = node as ImageProcessingNode;
            
            if (imageProcNode != null)
            {
                imageProcNode.OnNodeUpdated();

                HashSet<Node> notified = new HashSet<Node>();

                //Now notify all children that this node was updated...
                var allOutputConnections = imageProcNode.Outputs.SelectMany(output => output.GetConnections());

                if (allOutputConnections != null && allOutputConnections.Any())
                {
                    Debug.Log("Notifying outputs from " + imageProcNode.name);
                    
                    foreach (var outputConnection in allOutputConnections)
                    {
                        if (!notified.Contains(outputConnection.node))
                        {
                            this.OnUpdateNode(outputConnection.node);
                            notified.Add(outputConnection.node);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("This node was changed: " + node.name);
            }
        }
    }
}