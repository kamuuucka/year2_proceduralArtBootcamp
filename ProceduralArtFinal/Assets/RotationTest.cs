using UnityEngine;

// Transform.rotation example.

// Rotate a GameObject using a Quaternion.
// Tilt the cube using the arrow keys. When the arrow keys are released
// the cube will be rotated back to the center using Slerp.

public class RotationTest : MonoBehaviour
{
    
        public float rotationSpeed = 10f;
        public Vector3 rotationAxis = Vector3.up;

        void Update() {
            // Rotate the object around its center
            transform.RotateAround(Vector3.up, 10*Time.deltaTime);
        }
    

}