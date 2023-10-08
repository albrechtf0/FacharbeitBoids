using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidShaderHandler : MonoBehaviour
{
	public GameObject Boid;
	public int BoidCount;
	public Vector3 Space;

	private void Start()
	{
		for(int i = 0; i < BoidCount; i++)
		{
			GameObject.Instantiate(Boid, new Vector3(Random.Range(-Space.x / 2, Space.x / 2), Random.Range(-Space.y / 2, Space.y / 2), Random.Range(-Space.z / 2, Space.z / 2)), Random.rotation);
		}
	}
}