// See https://aka.ms/new-console-template for more information
using ShaderPrebuild;
using System.Numerics;


BoidBehavior boidBehavior = new BoidBehavior(0, 0, 0, 0, 0, 0, 0, 0, 0);

var point = boidBehavior.ClosestPoint(new Vector3(0, 0, 0), 
	new Triangle(
		new Vector3(1, 4, 5), 
		new Vector3(0, 7, 8), 
		new Vector3(3, -6, 9)));
Console.WriteLine(point);