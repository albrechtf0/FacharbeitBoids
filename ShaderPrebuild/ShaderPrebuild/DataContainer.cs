using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShaderPrebuild
{
	public struct Boid
	{
		public Vector3 position; 
		public Vector3 forward; //Normalized
		public float radius;
		public Boid(Vector3 position, Vector3 forward, float radius)
		{
			this.position = position;
			this.forward = forward;
			this.radius = radius;
		}
	}

	public struct Plane
	{
		public Vector3[] vertecies; // länge 3
		public Plane(Vector3 vert1, Vector3 vert2, Vector3 vert3)
		{
			vertecies = new Vector3[] {vert1, vert2, vert3};
		}
	}
	public struct Sphere
	{
		public Vector3 position;
		public float radius;
		public Sphere(Vector3 position, float radius)
		{
			this.position = position;
			this.radius = radius;
		}
	}
}