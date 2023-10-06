using Unity.VisualScripting;
using UnityEngine;

public class BoidHandler : MonoBehaviour
{
	public float Lookradius;
	public float AvoidanceRadius;
	public float ObjektAvoidanceStrength;
	private Vector3 dampeneing;
	void Update()
	{
		Collider[] Cols = Physics.OverlapSphere(transform.position, Lookradius);
		Vector3 center = Vector3.zero;
		Vector3 direction = transform.forward;
		Vector3 avoidance = Vector3.zero;
		Vector3 ColisionAvoidance = Vector3.zero;
		int visible = 0;
		int tooClose = 0;
		foreach (Collider col in Cols)
		{
			if (col.gameObject.tag == "Boid")
			{
				RaycastHit RayHit;
				Physics.Raycast(transform.position, col.transform.position - transform.position, out RayHit);
				if (RayHit.collider == col)
				{
					visible++;
					center += col.transform.position;
					direction += col.transform.forward;
					float distance = (col.transform.position - transform.position).magnitude;
					if (distance < AvoidanceRadius)
					{
						tooClose++;
						avoidance += -(col.transform.position - transform.position).normalized * (AvoidanceRadius / distance);
					}
				}
			}
			else
			{
				Vector3 closestPoint = col.ClosestPoint(transform.position);
				Vector3 relPos = closestPoint - transform.position; //From self to Closest point
				Vector3 target = Vector3.Cross(Vector3.Cross(relPos, transform.forward), relPos).normalized;
				ColisionAvoidance += target;
				ColisionAvoidance += -relPos * Mathf.Max(((-1 / Mathf.Pow(AvoidanceRadius,2)) * Mathf.Pow(relPos.magnitude, 2) + 1)*ObjektAvoidanceStrength, 0);
			}
		}
		Vector3 ResDirection = Vector3.zero;
		if (visible > 0)
		{
			center /= visible;
			ResDirection += (center - transform.position).normalized;
		}
		if (tooClose > 0)
		{
			ResDirection += avoidance / tooClose;
		}
		ResDirection += ColisionAvoidance;

		ResDirection += direction.normalized;

		transform.forward = Vector3.SmoothDamp(transform.forward, ResDirection.normalized, ref dampeneing, 1).normalized;
		transform.position += transform.forward * Time.deltaTime;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, Lookradius);
		Gizmos.color = Color.gray;
		Gizmos.DrawWireSphere(transform.position, AvoidanceRadius);
		Collider[] Cols = Physics.OverlapSphere(transform.position, Lookradius);
		Vector3 center = Vector3.zero;
		Vector3 direction = transform.forward;
		Vector3 avoidance = Vector3.zero;
		Vector3 ColisionAvoidance = Vector3.zero;
		int visible = 0;
		int tooClose = 0;
		foreach (Collider col in Cols)
		{
			if (col.gameObject.tag == "Boid")
			{
				RaycastHit RayHit;
				Physics.Raycast(transform.position, col.transform.position - transform.position, out RayHit);
				if (RayHit.collider == col)
				{
					Gizmos.color = Color.white;
					Gizmos.DrawSphere(col.transform.position, 1);
					visible++;
					center += col.transform.position;
					direction += col.transform.forward;
					float distance = (col.transform.position - transform.position).magnitude;
					if (distance < AvoidanceRadius)
					{
						Gizmos.color = Color.cyan;
						Gizmos.DrawSphere(col.transform.position, 1);
						avoidance += -(col.transform.position - transform.position).normalized * (AvoidanceRadius / distance);
					}
				}
			}
			else
			{
				Vector3 closestPoint = col.ClosestPoint(transform.position);
				Vector3 relPos = closestPoint - transform.position; //From self to Closest point
				Vector3 target = Vector3.Cross(Vector3.Cross(relPos, transform.forward), relPos).normalized;
				ColisionAvoidance += target;
				Gizmos.color = Color.red;
				Gizmos.DrawRay(transform.position, target);
				Gizmos.color = Color.black;
				Gizmos.DrawLine(transform.position, closestPoint);
			}
		}
		center /= visible;
		//Debug.Log($"Visible: {visible}");
		Vector3 ResDirection = Vector3.zero;
		if (visible > 0)
		{
			center /= visible;
			ResDirection += (center - transform.position).normalized;
		}
		if (tooClose > 0)
		{
			ResDirection += avoidance / tooClose;
		}
		ResDirection += ColisionAvoidance;

		ResDirection += direction.normalized;

		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, ResDirection);//Target direktion
		Gizmos.color = Color.gray;
		Gizmos.DrawRay(transform.position, avoidance);//avoidance direction
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, ColisionAvoidance);// Colision avoidance direction
	}
}