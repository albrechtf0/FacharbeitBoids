using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Handler : MonoBehaviour
{
	/// <summary>
	/// The count of <see cref="int"/>s to multiply
	/// </summary>
	public int amount;
	/// <summary>
	/// The number of Runs taken on benchmarking
	/// </summary>
	public int runs;
	/// <summary>
	/// Calculator used for calculations 
	/// </summary>
	public Calculator calc;

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log($"Singlethreading with {amount} values took {calc.calculateSingelThreaded(amount)} Miliseconds.");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Debug.Log($"Multiprocessing with {amount} values took {calc.calculateMultiprocessed(amount)} Miliseconds.");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Debug.Log($"Shader with {amount} values took {calc.calculateParallelShader(amount)} Miliseconds.");
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift) & Input.GetKeyUp(KeyCode.RightShift))
		{
			Debug.Log("Starting bechmark");
			StartCoroutine(benchmarkTask());
		}
	}

	IEnumerator benchmarkTask()
	{
		long[] timeSingle = new long[runs];
		long[] timeMulti = new long[runs];
		long[] timeShader = new long[runs];
		for (int i = 0; i < runs; i++)
		{
			timeSingle[i] = calc.calculateSingelThreaded(amount);
			timeMulti[i] = calc.calculateMultiprocessed(amount);
			timeShader[i] = calc.calculateParallelShader(amount);
			Debug.Log($"Benchmark at: {i/(float)runs*100}%");
			yield return null;
		}
		using (StreamWriter file = new StreamWriter(Path.Combine(Application.persistentDataPath, "BechmarkResults.csv")))
		{
			file.WriteLine("TimeSinglethreading;TimeMultiprocessing;TimeShader");
			for(int i = 0; i < runs; ++i)
			{
				file.WriteLine($"{timeSingle[i]};{timeMulti[i]};{timeShader[i]}");
			}
		}
		Debug.Log("Finished Benchmark");
		yield return null;
	}
}
