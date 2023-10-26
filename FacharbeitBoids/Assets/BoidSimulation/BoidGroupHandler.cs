using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidGroupHandler : MonoBehaviour
{
	public GameObject Boid;
	public int BoidCount;
	public Vector3 Space;

	private void Start()
	{
		CreateBoids(BoidCount);
	}

	public void CreateBoids(int count)
	{
		for (int i = 0; i < count; i++)
		{
			GameObject.Instantiate(Boid, new Vector3(Random.Range(-Space.x / 2, Space.x / 2), Random.Range(-Space.y / 2, Space.y / 2), Random.Range(-Space.z / 2, Space.z / 2)), Random.rotation);
		}
	}
	public void ClearBoids()
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			Destroy(Boid);
		}
	}

	public void setBoidLookRadius(float radius)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if( Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().LookRadius = radius;
			}
		}
	}
	public void setBoidAvoidanceRadius(float radius)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().AvoidanceRadius = radius;
			}
		}
	}
	public void setBoidSpeed(float speed)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().Speed = speed;
			}
		}
	}
	public void setBoidMaxTurningSpeed(float turningSpeed)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().MaxTurningSpeed = turningSpeed;
			}
		}
	}
	public void setBoidTurningTime(float turningTime)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().TurningTime = turningTime;
			}
		}
	}
	public void setBoidDirectionStrength(float strength)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().DirectionStrength = strength;
			}
		}
	}
	public void setBoidCohesionStrength(float strength)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().CohesionStrength = strength;
			}
		}
	}
	public void setBoidAvoidanceStrength(float strength)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().AvoidanceStrength = strength;
			}
		}
	}
	public void setBoidObjectAvoidanceStrength(float strength)
	{
		var Boids = GameObject.FindGameObjectsWithTag("Boid");
		foreach (var Boid in Boids)
		{
			if (Boid.GetComponent<BoidHandler>() != null)
			{
				Boid.GetComponent<BoidHandler>().ObjektAvoidanceStrength = strength;
			}
		}
	}
}