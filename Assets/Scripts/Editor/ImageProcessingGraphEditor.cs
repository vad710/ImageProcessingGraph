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
        

        private void DoOnUpdateNode(ImageProcessingNode node)
        {
            var updateOrder = new LinkedList<ImageProcessingNode>();
            
            DetermineNodeUpdateOrder(updateOrder,node);

            var notifications = 0;
            
            var currentNode = updateOrder.First;
            while (currentNode != null)
            {
                //Debug.Log("Notifying: " + currentNode.Value.name);
                currentNode.Value.OnNodeUpdated();
                currentNode = currentNode.Next;

                notifications++;
            }
            
            
            Debug.Log("Notified a total of " + notifications + " nodes");
        }

        public void ManualUpdateNotification(ImageProcessingNode node)
        {
            DoOnUpdateNode(node);
        }
        
        private void OnUpdateNode(Node node)
        {
            var imageProcNode = node as ImageProcessingNode;
            
            if (imageProcNode != null && !imageProcNode.ManualUpdate)
            {
                DoOnUpdateNode(imageProcNode);    
            }
        }

        private void DetermineNodeUpdateOrder(LinkedList<ImageProcessingNode> order, Node startNode)
        {
            var imageProcNode = startNode as ImageProcessingNode;

            if (imageProcNode != null)
            {
                //if this node is already in the list, let's move it to the back of the list
                if (order.Contains(imageProcNode))
                {
                    order.Remove(imageProcNode);
                }
                
                order.AddLast(imageProcNode);
                
                //get all of the output connections of this node
                var allOutputConnections = imageProcNode.Outputs.SelectMany(output => output.GetConnections());

                foreach (var outputConnection in allOutputConnections)
                {
                    //this.OnUpdateNode(outputConnection.node);
                    DetermineNodeUpdateOrder(order, outputConnection.node);
                }
            }
            
        }
        

    }
}