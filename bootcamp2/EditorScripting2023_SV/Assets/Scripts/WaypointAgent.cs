using UnityEngine;

public class WaypointAgent : MonoBehaviour
{
	[SerializeField]
	Curve curve;

	public float speed = 3f;
	public bool adjustAlways = false;

	private int targetWaypoint = 0;
    private Vector3 direction;


    private void Start()
    {
		curve.OnApply += UpdateDirection;
		UpdateDirection();
    }

	void UpdateDirection() {
		direction = CalculateDirection(transform.position, curve.GetPoint(targetWaypoint));
	}

	private void Update()
    {
		if (adjustAlways) {
			direction = CalculateDirection(transform.position, curve.GetPoint(targetWaypoint));
		}

		if ((transform.position - curve.GetPoint(targetWaypoint)).magnitude < speed * Time.deltaTime) {
			UpdateTargetWaypoint();
		}
		MoveToWaypoint(targetWaypoint);
    }

    private void MoveToWaypoint(int index)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void UpdateTargetWaypoint()
    {
        targetWaypoint++;
        if (targetWaypoint == curve.NumPoints())
        {
            targetWaypoint = 0;
        }
		UpdateDirection();
    }

    private Vector3 CalculateDirection(Vector3 from, Vector3 to)
    {
        Vector3 result = to - from;
        result.Normalize();
        return result;
    }
}
