using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Handout {
	public class CreateStairs : MonoBehaviour {
		public int numberOfSteps = 10;
		// The dimensions of a single step of the staircase:
		public float width=3;
		public float height=1;
		public float depth=1;

		MeshBuilder builder;

		void Start () {
			builder = new MeshBuilder ();
			CreateShape ();
			GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
		}

		/// <summary>
		/// Creates a stairway shape in [builder].
		/// </summary>
		void CreateShape() {
			builder.Clear ();

			/**/
			// V1: single step, hard coded:
			// bottom:
			// int v1 = builder.AddVertex (new Vector3 (2, 0, 0), new Vector2 (1, 0));	
			// int v2 = builder.AddVertex (new Vector3 (-2, 0, 0), new Vector2 (0, 0));
			// // top front:
			// int v3 = builder.AddVertex (new Vector3 (2, 1, 0), new Vector2 (1, 0.5f));	
			// int v4 = builder.AddVertex (new Vector3 (-2, 1, 0), new Vector2 (0, 0.5f));
			// // top back:
			// int v5 = builder.AddVertex (new Vector3 (2, 1, 1), new Vector2 (0, 1));	
			// int v6 = builder.AddVertex (new Vector3 (-2, 1, 1), new Vector2 (1, 1));
			//
			// builder.AddTriangle (v1, v2, v3);
			// builder.AddTriangle (v2, v3, v4);
			// builder.AddTriangle (v3, v4, v5);
			// builder.AddTriangle (v4, v6, v5);

			
			// V2, with for loop:
			for (int i = 0; i < numberOfSteps; i++) {
				Vector3 offset = new Vector3 (0, height * i, depth * i); 

				// 1: use the width and height parameters from the inspector to change the step width and height
				//Added them to the Vertex's vectors
				

				// 4: Fix the uvs:
				// bottom:
				// int v1 = builder.AddVertex (offset + new Vector3 (width, 0, 0), new Vector2 (1, 0));	
				// int v2 = builder.AddVertex (offset + new Vector3 (-width, 0, 0), new Vector2 (0, 0));
				// // top front:
				// int v3 = builder.AddVertex (offset + new Vector3 (width, height, 0), new Vector2 (1, 0.5f));	
				// int v4 = builder.AddVertex (offset + new Vector3 (-width, height, 0), new Vector2 (0, 0.5f));
				// // top back:
				// int v5 = builder.AddVertex (offset + new Vector3 (width, height, 1), new Vector2 (1, 1));	
				// int v6 = builder.AddVertex (offset + new Vector3 (-width, height, 1), new Vector2 (0, 1));

				// // 2: Fix the winding order (everything clockwise):
				// builder.AddTriangle (v1, v2, v3);
				// builder.AddTriangle (v2, v4, v3);
				// builder.AddTriangle (v3, v4, v5);
				// builder.AddTriangle (v4, v6, v5);
				//
				// // 3: make the mesh solid by adding left, right and back side.
				//
				// builder.AddTriangle(v1, v3, v5);
				// builder.AddTriangle(v2,v6,v4);
				// builder.AddTriangle(v2,v1,v6);
				// builder.AddTriangle(v1,v5,v6);

				// TODO 5: Fix the normals by *not* reusing a single vertex in multiple triangles with different normals
				// (solve it by creating more vertices at the same position)
				
				// Front face of the stair (Front Bottom Right etc)
				int vFBR = builder.AddVertex (offset + new Vector3 (width, 0, 0), new Vector2 (1, 0));	
				int vFBL = builder.AddVertex (offset + new Vector3 (-width, 0, 0), new Vector2 (0, 0));
				int vFTR = builder.AddVertex (offset + new Vector3 (width, height, 0), new Vector2 (1, 0.5f));	
				int vFTL = builder.AddVertex (offset + new Vector3 (-width, height, 0), new Vector2 (0, 0.5f));
				// Top face of the stair (Top Front Right etc)
				int vTFR = builder.AddVertex (offset + new Vector3 (width, height, 0), new Vector2 (1, 0.5f));	
				int vTFL = builder.AddVertex (offset + new Vector3 (-width, height, 0), new Vector2 (0, 0.5f));
				int vTBR = builder.AddVertex (offset + new Vector3 (width, height, 1), new Vector2 (1, 1));	
				int vTBL = builder.AddVertex (offset + new Vector3 (-width, height, 1), new Vector2 (0, 1));
				//Left face of the stair (Left bottom right etc)
				int vLBR = builder.AddVertex(offset + new Vector3(-width, 0, 0), new Vector2(1, 0));
				int vLTL = builder.AddVertex (offset + new Vector3 (-width, height, 1), new Vector2 (0, 1));
				int vLTR = builder.AddVertex (offset + new Vector3 (-width, height, 0), new Vector2 (1, 1));
				//Right face of the stair
				int vRBL = builder.AddVertex (offset + new Vector3 (width, 0, 0), new Vector2 (0, 0));
				int vRTL = builder.AddVertex (offset + new Vector3 (width, height, 0), new Vector2 (0, 1));
				int vRTR = builder.AddVertex (offset + new Vector3 (width, height, 1), new Vector2 (1, 1));
				//Back of the stair
				int vBBR = builder.AddVertex (offset + new Vector3 (-width, 0, 0), new Vector2 (0, 0));
				int vBBL = builder.AddVertex (offset + new Vector3 (width, 0, 0), new Vector2 (1, 0));	
				int vBTR = builder.AddVertex (offset + new Vector3 (-width, height, 1), new Vector2 (0, 1));
				int vBTL = builder.AddVertex (offset + new Vector3 (width, height, 1), new Vector2 (1, 1));	
				
				//front and top
				builder.AddTriangle (vFBR, vFBL, vFTR);
				builder.AddTriangle (vFBL, vFTL, vFTR);
				builder.AddTriangle (vTFR, vTFL, vTBR);
				builder.AddTriangle (vTFL, vTBL, vTBR);
				//sides and back
				builder.AddTriangle(vRBL, vRTL, vRTR);
				builder.AddTriangle(vLBR,vLTL,vLTR);
				builder.AddTriangle(vBBR,vBBL,vBTR);
				builder.AddTriangle(vBBL,vBTL,vBTR);
			}
			
		}
		
	}
}