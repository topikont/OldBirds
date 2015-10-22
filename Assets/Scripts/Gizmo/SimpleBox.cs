using UnityEngine;
using System.Collections;

/// <summary>
/// Simple box gizmo that scales and translates itself to the 
/// position and scale of the attached transform.
/// </summary>

public class SimpleBox : MonoBehaviour {
	/// <summary>
	/// The color of the gizmo.
	/// </summary>
	public Color gizmoColor	= Color.red;
	
	/// <summary>
	/// Hide gizmo if necessary
	/// </summary>
	public bool hideGizmo = false;
	
	/// <summary>
	/// Draws the gizmo
	/// </summary>
	void OnDrawGizmos() {
		Gizmos.color = gizmoColor;
		
		// Make the matrix to use for the gizmo
		// Rotates, translates and scales the gizmo according to the transform
		Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = rotationMatrix;
		
		if(!hideGizmo)
			Gizmos.DrawCube (Vector3.zero, Vector3.one);
	}
}
