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
	}

	public struct Triangle
	{
		public Vector3[] vertecies; // länge 3
		public Triangle(Vector3 vert1, Vector3 vert2, Vector3 vert3)
		{
			vertecies = new Vector3[] {vert1, vert2, vert3};
		}
	}
}