using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataContainer
{
	[System.Serializable]
	public struct Boid
	{
		public Vector3 position;
		public Vector3 forward; //Normalized
		public Boid(Vector3 position, Vector3 forward)
		{
			this.position = position;
			this.forward = forward;
		}
	}

	[System.Serializable]
	public struct Plane
	{
		public Vector3 vertecie1;
		public Vector3 vertecie2;
		public Vector3 vertecie3;
		public Plane(Vector3 vert1, Vector3 vert2, Vector3 vert3)
		{
			vertecie1 = vert1;
			vertecie2 = vert2;
			vertecie3 = vert3;
		}
	}

	[System.Serializable]
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