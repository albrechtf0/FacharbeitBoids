using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class BoidJobHandler : MonoBehaviour
{
	public GameObject Boid;
	public int BoidCount;
	public Vector3 Space;
	public float LookRadius;
	public float AvoidanceRadius;
	public float Speed;
	public float MaxTurningSpeed;
	public float TurningTime;
	public float DirectionStrength;
	public float CohesionStrength;
	public float AvoidanceStrenght;
	public float ObjektAvoidanceStrength;

	public int OverlapMaxHits;


	private TransformAccessArray Boids;
	private NativeArray<Vector3> dampenings;

	void Start()
	{
		Transform[] BoidTransforms = new Transform[BoidCount];
		Vector3[] dampVecs = new Vector3[BoidCount];
		for (int i = 0; i < BoidCount; i++)
		{
			BoidTransforms[i] = GameObject.Instantiate(Boid, new Vector3(Random.Range(-Space.x / 2, Space.x / 2), Random.Range(-Space.y / 2, Space.y / 2), Random.Range(-Space.z / 2, Space.z / 2)), Random.rotation).transform;
			dampVecs[i] = Vector3.zero;
		}
		dampenings = new NativeArray<Vector3>(dampVecs, Allocator.Persistent);
		Boids = new TransformAccessArray(BoidTransforms);
	}

	private void OnDisable()
	{
		dampenings.Dispose();
		Boids.Dispose();
	}

	private void Update()
	{
		NativeArray<OverlapSphereCommand> OverlapComands = new NativeArray<OverlapSphereCommand>(BoidCount, Allocator.TempJob);
		for (int i = 0; i < BoidCount; i++)
		{
			OverlapComands[i] = new OverlapSphereCommand(Boids[i].position, this.LookRadius, QueryParameters.Default);
		}
		NativeArray<ColliderHit> colliderHits = new NativeArray<ColliderHit>(BoidCount * OverlapMaxHits, Allocator.TempJob);
		JobHandle OverlapHandle = OverlapSphereCommand.ScheduleBatch(OverlapComands, colliderHits, 1, OverlapMaxHits);
		OverlapHandle.Complete();
		OverlapComands.Dispose();

		NativeArray<ClosestPointCommand> closestPointCommands = new NativeArray<ClosestPointCommand>(colliderHits.Length, Allocator.TempJob);
		for (int i = 0; i < closestPointCommands.Length; i++)
		{
			closestPointCommands[i] = 
				new ClosestPointCommand(
					Boids[Mathf.FloorToInt(i/OverlapMaxHits)].position, 
					colliderHits[i].collider, 
					colliderHits[i].collider.transform.position, 
					colliderHits[i].collider.transform.rotation, 
					colliderHits[i].collider.transform.lossyScale);
		}
		NativeArray<Vector3> closestPoints = new NativeArray<Vector3>(colliderHits.Length, Allocator.TempJob);
		JobHandle closestPointHandle = ClosestPointCommand.ScheduleBatch(closestPointCommands, closestPoints, 1);
		closestPointHandle.Complete();
		closestPointCommands.Dispose();

		NativeArray<ColliderData> colliders = new NativeArray<ColliderData>(BoidCount * OverlapMaxHits, Allocator.TempJob);
		for (int i = 0; i < colliderHits.Length; i++)
		{
			TransformAccess trans = new TransformAccess();
			trans.SetPositionAndRotation(colliderHits[i].collider.transform.position, colliderHits[i].collider.transform.rotation);
			colliders[i] = new ColliderData(trans, colliderHits[i].collider.tag == "Boid", closestPoints[i]);
		}
		colliderHits.Dispose();
		closestPoints.Dispose();

		BoidJob BoidMover = new BoidJob()
		{
			LookRadius = this.LookRadius,
			AvoidanceRadius = this.AvoidanceRadius,
			Speed = this.Speed,
			MaxTurningSpeed = this.MaxTurningSpeed,
			TurningTime = this.TurningTime,
			DirectionStrength = this.DirectionStrength,
			CohesionStrength = this.CohesionStrength,
			AvoidanceStrenght = this.AvoidanceStrenght,
			ObjektAvoidanceStrength = this.ObjektAvoidanceStrength,
			deltatime = Time.deltaTime,
			dampeneing = dampenings,
			Hits = colliders,
			MaxHits = OverlapMaxHits
		};
		JobHandle handle = BoidMover.Schedule(Boids, OverlapHandle);
		handle.Complete();
		colliders.Dispose();
	}

	private struct BoidJob : IJobParallelForTransform
	{
		public float LookRadius;
		public float AvoidanceRadius;
		public float Speed;
		public float MaxTurningSpeed;
		public float TurningTime;
		public float DirectionStrength;
		public float CohesionStrength;
		public float AvoidanceStrenght;
		public float ObjektAvoidanceStrength;
		public NativeArray<Vector3> dampeneing;
		public float deltatime;
		public NativeArray<ColliderData> Hits;
		public int MaxHits;
		public void Execute(int index, TransformAccess transform)
		{

			ColliderData[] Cols = Hits.GetSubArray(index * MaxHits, MaxHits).ToArray();
			Vector3 forward = transform.rotation * Vector3.forward;
			Vector3 center = Vector3.zero;
			Vector3 direction = forward;
			Vector3 avoidance = Vector3.zero;
			Vector3 ColisionAvoidance = Vector3.zero;
			int visible = 0;
			int tooClose = 0;
			foreach (ColliderData col in Cols)
			{
				if (col.IsBoid)
				{
					RaycastHit RayHit;
					Physics.Raycast(transform.position, col.transform.position - transform.position, out RayHit);
					if (RayHit.transform.position == col.transform.position)
					{
						visible++;
						center += col.transform.position;
						direction += col.transform.rotation * Vector3.forward;
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
					Vector3 closestPoint = col.closetPoint;
					Vector3 relPos = closestPoint - transform.position; //From self to Closest point
					Vector3 target = Vector3.Cross(Vector3.Cross(relPos, forward), relPos).normalized;
					ColisionAvoidance += target * Mathf.Clamp((-1 / (LookRadius - AvoidanceRadius)) * (relPos.magnitude - LookRadius), 0, 1);
					ColisionAvoidance += -relPos * Mathf.Max(((-1 / Mathf.Pow(AvoidanceRadius, 2)) * Mathf.Pow(relPos.magnitude, 2) + 1) * ObjektAvoidanceStrength, 0);
				}
			}
			Vector3 ResDirection = Vector3.zero;
			if (visible > 0)
			{
				center /= visible;
				ResDirection += (center - transform.position).normalized * CohesionStrength;
			}
			if (tooClose > 0)
			{
				ResDirection += (avoidance / tooClose) * AvoidanceStrenght;
			}
			ResDirection += ColisionAvoidance;

			ResDirection += direction.normalized * DirectionStrength;

			Vector3 currendVelocity = dampeneing[index];
			forward = Vector3.SmoothDamp(forward, ResDirection.normalized, ref currendVelocity, TurningTime, MaxTurningSpeed, deltatime).normalized;
			dampeneing[index] = currendVelocity;
			transform.position += forward * Speed * deltatime;
			transform.rotation.SetLookRotation(forward);
		}
	}

	struct ColliderData
	{
		public TransformAccess transform;
		public bool IsBoid;
		public Vector3 closetPoint;
		public ColliderData(TransformAccess transform, bool IsBoid, Vector3 closestPoint)
		{
			this.transform = transform;
			this.IsBoid = IsBoid;
			this.closetPoint = closestPoint;
		}
	}
}