using System.Numerics;

namespace ShaderPrebuild
{
	public class BoidBehavior
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
		public Vector3 dampeneing; //Array

		public BoidBehavior(float LookRadius,
			float AvoidanceRadius,
			float Speed,
			float MaxTurningSpeed,
			float TurningTime,
			float DirectionStrength,
			float CohesionStrength,
			float AvoidanceStrenght,
			float ObjektAvoidanceStrength)
		{
			this.LookRadius = LookRadius;
			this.AvoidanceRadius = AvoidanceRadius;
			this.Speed = Speed;
			this.MaxTurningSpeed = MaxTurningSpeed;
			this.TurningTime = TurningTime;
			this.DirectionStrength = DirectionStrength;
			this.CohesionStrength = CohesionStrength;
			this.AvoidanceStrenght = AvoidanceStrenght;
			this.ObjektAvoidanceStrength = ObjektAvoidanceStrength;
			this.dampeneing = Vector3.Zero;
		}
		public void Update(Boid self, Boid[] Boids, Plane[] planes, Sphere[] spheres, float deltaTime)
		{
			Vector3 center = Vector3.Zero;
			Vector3 direction = self.forward;
			Vector3 avoidance = Vector3.Zero;
			Vector3 ColisionAvoidance = Vector3.Zero;
			int visible = 0;
			int tooClose = 0;
			foreach (Boid Boi in Boids)
			{
				if (Vector3.Distance(self.position, Boi.position) <= LookRadius)
				{
					if (RaycastHitsBoid(self.position, Boi.position - self.position, Boi, Boids))
					{
						float distance = (Boi.position - self.position).Length();
						if (distance == 0)//Catching for self
						{
							continue;
						}
						visible++;
						center += Boi.position;
						direction += Boi.forward;
						if (distance < AvoidanceRadius)
						{
							tooClose++;
							avoidance += -Vector3.Normalize(Boi.position - self.position) * (AvoidanceRadius / distance);
						}
					}
				}

			}
			foreach (Plane plane in planes)
			{
				if (distancePlane(self.position, plane) <= LookRadius)
				{
					Vector3 closestPoint = ClosestPointPlane(self.position, plane);
					Vector3 relPos = closestPoint - self.position; //From self to Closest point
					Vector3 target = Vector3.Normalize(Vector3.Cross(Vector3.Cross(relPos, self.forward), relPos));
					ColisionAvoidance += target * Math.Clamp((-1 / (LookRadius - AvoidanceRadius)) * (relPos.Length() - LookRadius), 0, 1);
					ColisionAvoidance += -relPos * Math.Max(((-1 / (float)Math.Pow(AvoidanceRadius, 2)) * relPos.LengthSquared() + 1) * ObjektAvoidanceStrength, 0);
				}
			}
			foreach (Sphere sphere in spheres)
			{
				if (Vector3.Distance(self.position, sphere.position) <= LookRadius + sphere.radius)
				{
					Vector3 closestPoint = ClosestPointSphere(self.position, sphere);
					Vector3 relPos = closestPoint - self.position; //From self to Closest point
					Vector3 target = Vector3.Normalize(Vector3.Cross(Vector3.Cross(relPos, self.forward), relPos));
					ColisionAvoidance += target * Math.Clamp((-1 / (LookRadius - AvoidanceRadius)) * (relPos.Length() - LookRadius), 0, 1);
					ColisionAvoidance += -relPos * Math.Max(((-1 / (float)Math.Pow(AvoidanceRadius, 2)) * relPos.LengthSquared() + 1) * ObjektAvoidanceStrength, 0);
				}
			}

			Vector3 ResDirection = Vector3.Zero;
			if (visible > 0)
			{
				center /= visible;
				ResDirection += Vector3.Normalize(center - self.position) * CohesionStrength;
			}
			if (tooClose > 0)
			{
				ResDirection += (avoidance / tooClose) * AvoidanceStrenght;
			}
			ResDirection += ColisionAvoidance;

			ResDirection += Vector3.Normalize(direction) * DirectionStrength;

			//self.forward = Vector3.SmoothDamp(self.forward, Vector3.Normalize(ResDirection), ref dampeneing, TurningTime, MaxTurningSpeed).normalized;
			self.forward = Vector3.Normalize(ResDirection);
			self.position += self.forward * Speed * deltaTime;
		}

		private bool RaycastBoid(Vector3 position, Vector3 direction, Boid boid)
		{
			return distance(position, direction, boid.position) <= boid.radius;
		}
		private bool[] RaycastBoids(Vector3 position, Vector3 direction, Boid[] boids)
		{
			bool[] result = new bool[boids.Length];
			for (int i = 0; i < boids.Length; i++)
			{
				result[i] = distance(position, direction, boids[i].position) <= boids[i].radius;
			}

			return result;
		}

		private bool RaycastShere(Vector3 position, Vector3 direction, Sphere sphere)
		{
			return distance(position, direction, sphere.position) <= sphere.radius;
		}

		private bool RaycastHitsBoid(Vector3 position, Vector3 direction, Boid target, Boid[] boids)
		{
			float distance = Vector3.Distance(position, target.position);
			bool[] hits = RaycastBoids(position, direction, boids);
			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i])
				{
					if (Vector3.Distance(boids[i].position, position) < distance) // catch behind target
					{
						if (Vector3.Distance(target.position, boids[i].position) <= distance) // Catch other side
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public float distance(Vector3 startPos, Vector3 direction, Vector3 point)
		{
			Vector3 relpos = point - startPos;
			Vector3 cross = Vector3.Cross(relpos, direction);
			return cross.Length() / direction.Length();
		}

		public float distancePlane(Vector3 point, Plane plane)
		{
			Vector3 cross = Vector3.Cross(plane.vertecies[1] - plane.vertecies[0], plane.vertecies[2] - plane.vertecies[0]);
			return Vector3.Dot(cross, point - plane.vertecies[0]) / cross.Length();
		}

		public Vector3 ClosestPointPlane(Vector3 point, Plane plane)
		{
			Vector3 cross = Vector3.Cross(plane.vertecies[1] - plane.vertecies[0], plane.vertecies[2] - plane.vertecies[0]);
			float distance = Vector3.Dot(cross, point - plane.vertecies[0]) / cross.Length();
			return point - distance * Vector3.Normalize(cross);
		}

		public Vector3 ClosestPointSphere(Vector3 point, Sphere sphere)
		{
			Vector3 relpos = point - sphere.position;
			relpos = Vector3.Normalize(relpos) * sphere.radius;
			return sphere.position + relpos;
		}
	}
}