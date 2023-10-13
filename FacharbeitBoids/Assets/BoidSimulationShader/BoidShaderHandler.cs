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
	public float BoidRadius;
	public float LookRadius;
	public float AvoidanceRadius;
	public float Speed;
	public float DirectionStrength;
	public float CohesionStrength;
	public float AvoidanceStrenght;
	public float ObjektAvoidanceStrength;
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


		Boid[] boids = new Boid[count];
		for (int i = 0; i < count; i++)
		{
			boids[i] = new Boid(
				new Vector3(
					Random.Range(SpownRange.min.x, SpownRange.max.x),
					Random.Range(SpownRange.min.y, SpownRange.max.y),
					Random.Range(SpownRange.min.z, SpownRange.max.z)),
				new Vector3(Random.value,Random.value,Random.value).normalized);
		}

		BoidBuffer = new ComputeBuffer(count,sizeof(float)*6);
		BoidBuffer.SetData(boids);
		BoidShader.SetBuffer(0, "Boids", BoidBuffer);
		BoidShader.SetFloat("BoidCount", boids.Length);

		InstanceDataBuffer = new ComputeBuffer(count,sizeof(float)*16);
		BoidShader.SetBuffer(0, "InstanceData", InstanceDataBuffer);
		InstanceData = new Matrix4x4[count];

		planesBuffer = new ComputeBuffer(planes.Length, sizeof(float) * 9);
		planesBuffer.SetData(planes);
		BoidShader.SetBuffer(0, "Planes", planesBuffer);
		BoidShader.SetInt("planeCount", planes.Length);

		spheresBuffer = new ComputeBuffer(spheres.Length, sizeof(float) * 4);
		spheresBuffer.SetData(spheres);
		BoidShader.SetBuffer(0, "Spheres", spheresBuffer);
		BoidShader.SetInt("sphereCount", spheres.Length);

		BoidShader.SetFloat("BoidRadius", BoidRadius);
		BoidShader.SetFloat("LookRadius",LookRadius);
		BoidShader.SetFloat("AvoidanceRadius",AvoidanceRadius);
		BoidShader.SetFloat("Speed",Speed);
		BoidShader.SetFloat("DirectionStrength",DirectionStrength);
		BoidShader.SetFloat("CohesionStrength",CohesionStrength);
		BoidShader.SetFloat("AvoidanceStrenght",AvoidanceStrenght);
		BoidShader.SetFloat("ObjektAvoidanceStrength",ObjektAvoidanceStrength);
}
	private void Update()
	{
		BoidShader.SetFloat("deltaTime", Time.deltaTime);
		BoidShader.Dispatch(0, count / 8, 1, 1);
		
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
}