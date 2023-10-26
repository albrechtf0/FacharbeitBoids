using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;


public class BoidShaderHandler : MonoBehaviour
{
	public int count;
	public ComputeShader BoidShader;
	public Mesh boidMesh;
	public Material boidMaterial;
	public Bounds SpownRange;
	public float LookRadius;
	public float AvoidanceRadius;
	public float Speed;
	public float DirectionStrength;
	public float CohesionStrength;
	public float AvoidanceStrength;
	public float ObjektAvoidanceStrength;
	public Vector3 lerpFactor;
	public DataContainer.Plane[] planes;
	public Sphere[] spheres;




	private ComputeBuffer BoidBuffer;
	private ComputeBuffer InstanceDataBuffer;
	private ComputeBuffer planesBuffer;
	private ComputeBuffer spheresBuffer;
	private RenderParams renderParams;
	private Matrix4x4[] InstanceData;

	private void Start()
	{
		renderParams = new RenderParams(boidMaterial);

		BoidBuffer = new ComputeBuffer(count,sizeof(float)*6);
		InstanceDataBuffer = new ComputeBuffer(count,sizeof(float)*16);

		setBoidCount(count);

		planesBuffer = new ComputeBuffer(planes.Length, sizeof(float) * 9);
		planesBuffer.SetData(planes);
		BoidShader.SetBuffer(0, "Planes", planesBuffer);
		BoidShader.SetInt("planeCount", planes.Length);

		spheresBuffer = new ComputeBuffer(spheres.Length, sizeof(float) * 4);
		spheresBuffer.SetData(spheres);
		BoidShader.SetBuffer(0, "Spheres", spheresBuffer);
		BoidShader.SetInt("sphereCount", spheres.Length);

		BoidShader.SetFloat("LookRadius",LookRadius);
		BoidShader.SetFloat("AvoidanceRadius",AvoidanceRadius);
		BoidShader.SetFloat("Speed",Speed);
		BoidShader.SetFloat("DirectionStrength",DirectionStrength);
		BoidShader.SetFloat("CohesionStrength",CohesionStrength);
		BoidShader.SetFloat("AvoidanceStrenght",AvoidanceStrength);
		BoidShader.SetFloat("ObjektAvoidanceStrength",ObjektAvoidanceStrength);
		BoidShader.SetVector("lerpFactor",lerpFactor);
}
	private void Update()
	{
		BoidShader.SetFloat("deltaTime", Time.deltaTime);
		BoidShader.Dispatch(0, count / 8, 1, 1);
		Boid[] boiarr = new Boid[count];
		BoidBuffer.GetData(boiarr);

		
		InstanceDataBuffer.GetData(InstanceData);
		Graphics.RenderMeshInstanced(renderParams, boidMesh, 0, InstanceData);
	}

	private void OnDisable()
	{
		BoidBuffer.Dispose();
		InstanceDataBuffer.Dispose();
		planesBuffer.Dispose();
		spheresBuffer.Dispose();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		foreach(DataContainer.Plane plane in planes)
		{
			Gizmos.DrawLine(plane.vertecie1, plane.vertecie2);
			Gizmos.DrawLine(plane.vertecie1, plane.vertecie3);
			Gizmos.DrawLine(plane.vertecie2, plane.vertecie3);
		}
		foreach(Sphere sphere in spheres)
		{
			Gizmos.DrawSphere(sphere.position, sphere.radius);
		}
	}

	public void setBoidCount(int count)
	{
		Boid[] boids = new Boid[count];
		for (int i = 0; i < count; i++)
		{
			boids[i] = new Boid(
				new Vector3(
					Random.Range(SpownRange.min.x, SpownRange.max.x),
					Random.Range(SpownRange.min.y, SpownRange.max.y),
					Random.Range(SpownRange.min.z, SpownRange.max.z)),
				new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f).normalized);
		}
		BoidBuffer.SetData(boids);
		BoidShader.SetBuffer(0, "Boids", BoidBuffer);
		BoidShader.SetFloat("BoidCount", boids.Length);

		BoidShader.SetBuffer(0, "InstanceData", InstanceDataBuffer);
		InstanceData = new Matrix4x4[count];
	}
	public void setBoidLookRadius(float radius)
	{
		LookRadius = radius;
		BoidShader.SetFloat("LookRadius",radius);
	}
	public void setBoidAvoidanceRadius(float radius)
	{
		AvoidanceRadius = radius;
		BoidShader.SetFloat("AvoidanceRadius",radius);
	}
	public void setBoidSpeed(float speed)
	{
		this.Speed = speed;
		BoidShader.SetFloat("Speed",speed);
	}
	public void setBoidDirectionStrength(float strength)
	{
		DirectionStrength = strength;
		BoidShader.SetFloat("DirectionStrength",strength);
	}
	public void setBoidCohesionStrength(float strength)
	{
		CohesionStrength = strength;
		BoidShader.SetFloat("CohesionStrength",strength);
	}
	public void setBoidAvoidanceStrength(float strength)
	{
		AvoidanceStrength = strength;
		BoidShader.SetFloat("AvoidanceStrenght",strength);
	}
	public void setBoidObjectAvoidanceStrength(float strength)
	{
		ObjektAvoidanceStrength = strength;
		BoidShader.SetFloat("ObjektAvoidanceStrength",strength);
	}
	public void setBoidLerpFactor(Vector3 factor)
	{
		lerpFactor = factor;
		BoidShader.SetVector("lerpFactor",factor);
	}
}