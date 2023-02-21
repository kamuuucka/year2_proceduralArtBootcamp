using UnityEngine;
using UnityEditor;

namespace Demo {
	[CustomEditor(typeof(BuildingPainter))]
	public class BuildingPainterEditor : Editor {

        private BuildingPainter painter;

        private void OnEnable()
        {
            painter = (BuildingPainter)target;
        }

        public void OnSceneGUI()
        {
	        Event e = Event.current;
			if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
            {
				Debug.Log("TODO: Spawn a building, right here!");
				//  If this painter object is hit, create a new house and add it to 
				//   BuildingPainter's list.
				//  Optionally: if a previously generated house is hit, destroy it again.
				//  Don't forget to register these action for undo, and mark the scene as dirty.
				
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast(ray, out hit))
				{
					GameObject newBuilding = painter.CreateBuilding(hit.point);
					
						Undo.RegisterCreatedObjectUndo(newBuilding, "New building created");
						painter.createdBuildings.Add(newBuilding);
				}

				e.Use();
            }

            DrawBuildingTransforms();
		}


        private void DrawBuildingTransforms()
        {
            for (int i = 0; i < painter.createdBuildings.Count; i++)
            {
				// Take care of destroying buildings manually:
                GameObject building = painter.createdBuildings[i];
				if (building==null) {
					painter.createdBuildings.RemoveAt(i);
					i--;
					continue;
				}

				// TODO (Ex 2): Draw a handle at the position of this building.
				//  Try to draw a position, rotation and scale gizmo at the same time.
				//  Don't forget to record changes in the undo list, and mark the scene as dirty.
				Vector3 pos = building.transform.position;
				Quaternion quat = Quaternion.identity;
				Vector3 lScale = building.transform.localScale;
				Handles.TransformHandle(ref pos, ref quat, ref lScale);
				building.transform.position = pos;
				building.transform.rotation *= quat;
				building.transform.localScale = lScale;
            }
        }
	}
}