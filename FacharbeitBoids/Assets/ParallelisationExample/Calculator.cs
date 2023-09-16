using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// A Class for handeling calculation using multible aproaches.
/// </summary>
[System.Serializable]
public class Calculator
{
	/// <summary>
	/// How often to repeat the calculation
	/// </summary>
	public int loops;
    /// <summary>
    /// The shader used for multiplying
    /// </summary>
    public ComputeShader calculationShader;


	/// <summary>
	/// A Function calculating <paramref name="amount"/> <see cref="int"/>s./>
	/// </summary>
	/// <param name="amount">The amount of <see cref="int"/>s to calculate.</param>
	/// <returns>Returnes the time the calculation took. Does not include generating the values.</returns>
	public long calculateSingelThreaded(int amount)
    {
        int[] numbers = new int[amount];
        for(int i = 0; i < amount; i++)
        {
            numbers[i] = i;
        }
		System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
		for (int i = 0;i < numbers.Length; i++)
		{
			int value = 0;
			for( int x = 0 ; x < loops; x++)
			{
				value += (int)(Mathf.Sqrt(numbers[i]) * Mathf.Cos(numbers[i]) / Mathf.Log(numbers[i]));
			}
			numbers[i] = value;
		}
		sw.Stop();
		return sw.ElapsedMilliseconds;
	}

	/// <summary>
	/// A Function calculating <paramref name="amount"/> <see cref="int"/>s./>
	/// </summary>
	/// <param name="amount">The amount of <see cref="int"/>s to calculate.</param>
	/// <returns>Returnes the time the calculation took. Does not include generating the values.</returns>
	public long calculateMultiprocessed(int amount)
    {
		NativeArray<int> numbers = new NativeArray<int>(amount,Allocator.TempJob);
		for (int i = 0; i < amount; i++)
		{
			numbers[i] = i;
		}
		System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        calculationJob mJob = new calculationJob()
        {
            numbers = numbers,
			loops = loops
        };
        JobHandle handle = mJob.Schedule(numbers.Length, 32);
        handle.Complete();
        numbers.Dispose();
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }
    /// <summary>
    /// Job for calculationg values.
    /// </summary>
	public struct calculationJob : IJobParallelFor
	{
        public NativeArray<int> numbers;
		public int loops;
		public void Execute(int index)
		{
			int value = 0;
			for(int x = 0 ; x < loops; x++)
			{
				value += (int)(Mathf.Sqrt(numbers[index]) * Mathf.Cos(numbers[index]) / Mathf.Log(numbers[index]));
			}
			numbers[index] = value;
		}
	}

	/// <summary>
	/// A Function calculating <paramref name="amount"/> <see cref="int"/>s./>
	/// </summary>
	/// <param name="amount">The amount of <see cref="int"/>s to calculate.</param>
	/// <returns>Returnes the time the calculation took. Does not include generating the values.</returns>
	public long calculateParallelShader(int amount)
    {
        int[] numbers = new int[amount];
		for (int i = 0; i < amount; i++)
		{
			numbers[i] = i;
		}
		System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        ComputeBuffer numsBuffer = new ComputeBuffer(numbers.Length,sizeof(int));
        numsBuffer.SetData(numbers);
		calculationShader.SetInt("loops", loops);
		calculationShader.SetBuffer(0, "numbers", numsBuffer);
		calculationShader.Dispatch(0, numbers.Length / 64, 1, 1);
        numsBuffer.GetData(numbers);
		numsBuffer.Dispose();
        sw.Stop();
        return sw.ElapsedMilliseconds;
	}
}