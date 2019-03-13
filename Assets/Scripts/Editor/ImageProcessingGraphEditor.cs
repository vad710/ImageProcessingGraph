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

        //private Node nodeToUpdate;
        
        public ImageProcessingGraphEditor()
        {
            NodeEditor.onUpdateNode += OnUpdateNode;
            
        }

//        public override void OnGUI()
//        {
//
//            if (this.nodeToUpdate != null)
//            {
//                DoOnUpdateNode(this.nodeToUpdate);
//                this.nodeToUpdate = null;
//            }
//            
//            base.OnGUI();
//        }

        private void DoOnUpdateNode(Node node)
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

        private void OnUpdateNode(Node node)
        {
            //this.nodeToUpdate = node;
            DoOnUpdateNode(node);
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
        
//        private void DetermineNodeUpdateOrder(Dictionary<ImageProcessingNode, int> order, Node startNode,
//            int level)
//        {
//            var imageProcNode = startNode as ImageProcessingNode;
//
//            if (imageProcNode != null)
//            {
//                
//                //Has this node already been added to the order?
//                if (order.ContainsKey(imageProcNode))
//                {
//                    //node already added - let's see if we need to change the notification order
//                    if (level > order[imageProcNode])
//                    {
//                        order[imageProcNode] = level; //update to a higher level
//                    }
//                }
//                else
//                {
//                    //go ahead and add the node to the dictionary at the current level
//                    order.Add(imageProcNode, level);
//                }
//
//            
//                //get all of the output connections of this node
//                var allOutputConnections = imageProcNode.Outputs.SelectMany(output => output.GetConnections());
//
//                foreach (var outputConnection in allOutputConnections)
//                {
//                    //this.OnUpdateNode(outputConnection.node);
//                    DetermineNodeUpdateOrder(order, outputConnection.node, level + 1);
//                }
//            }
//        }
    }
}