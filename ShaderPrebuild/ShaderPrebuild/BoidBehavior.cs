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
		void Update(Boid self, Boid[] Boids, Triangle[] Triangles, float deltaTime)
		{
			Vector3 center = Vector3.Zero;
			Vector3 direction = self.forward;
			Vector3 avoidance = Vector3.Zero;
			Vector3 ColisionAvoidance = Vector3.Zero;
			int visible = 0;
			int tooClose = 0;
			foreach (Boid Boi in Boids)
			{

				if (RaycastHitsBoid(self.position,Boi.position-self.position,Boi,Boids))
				{
					visible++;
					center += Boi.position;
					direction += Boi.forward;
					float distance = (Boi.position - self.position).Length();
					if (distance < AvoidanceRadius)
					{
						tooClose++;
						avoidance += -Vector3.Normalize(Boi.position - self.position) * (AvoidanceRadius / distance);
					}
				}
			}
            foreach (Triangle tri in Triangles)
            {
				//if( distance <= LookRadius)
				{
					Vector3 closestPoint = ClosestPoint(self.position, tri); // projezieren in 2d ebene des dreiecks wen innen distanz zur ebene ansonsten ?
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

		private bool RaycastHitsBoid(Vector3 position, Vector3 direction, Boid target, Boid[] boids)
		{
			float distance = Vector3.Distance(position,target.position);
			bool[] hits = RaycastBoids(position,direction, boids);
			for (int i = 0;i < hits.Length;i++)
			{
				if (hits[i])
				{
					if (Vector3.Distance(boids[i].position,position) < distance)
					{
						if (Vector3.Distance(target.position, boids[i].position) > distance)
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

		public Vector3 ClosestPointPlane(Vector3 point, Triangle triangle)
		{
			Vector3 cross = Vector3.Cross(triangle.vertecies[1] - triangle.vertecies[0], triangle.vertecies[2] - triangle.vertecies[0]);

			float distance = Vector3.Dot(cross, point - triangle.vertecies[0]) / cross.Length();

			return point - distance * Vector3.Normalize(cross);
			
		}
	}
}