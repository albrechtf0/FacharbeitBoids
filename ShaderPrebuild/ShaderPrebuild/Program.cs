// See https://aka.ms/new-console-template for more information
using ShaderPrebuild;
using System.Numerics;

Random random = new Random();
Vector3 randVec()
{
	return new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
}

BoidBehavior boidBehavior = new BoidBehavior(10,5,1,0,0,1,1,1,5);

Boid self = new Boid(new Vector3(1,1,1), Vector3.UnitX, 1);

Boid[] boids =new Boid[11];
for (int i = 0; i < 10; i++)
{
	boids[i] = new Boid(randVec(), Vector3.Normalize(randVec()), 1);
}
boids[10] = self;

Sphere[] spheres = new Sphere[10];
for (int i = 0;i < 10; i++)
{
	spheres[i] = new Sphere(randVec(), (float)random.NextDouble());
}
ShaderPrebuild.Plane[] planes = new ShaderPrebuild.Plane[] {new ShaderPrebuild.Plane(new Vector3(50,10,20),new Vector3(0,0,0),new Vector3(9,4,6))};

boidBehavior.Update(self, boids, planes, spheres, 1);
