using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ArrayCounter : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	//[Input] public float[] Array;
	//[Output] public int Length;
	
	// Return the correct value of an output port when requested
//	public override object GetValue(NodePort port)
//	{
//		return this.Length; //val.Length;
//	}
	
//	public override void OnRemoveConnection(NodePort port)
//	{
//		Debug.Log("Connection Removed...");
//		
//		base.OnRemoveConnection(port);
//		this.Length = 0;
//	}
//	
//	public override void OnCreateConnection(NodePort @from, NodePort to)
//	{
//		Debug.Log("Connection Created...");
//		
//		var val = GetInputValue<float[]>("Array");
//		this.Length = val.Length;
//		
//		base.OnCreateConnection(@from, to);
//		
//	}

}