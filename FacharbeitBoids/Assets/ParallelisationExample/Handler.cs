using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Handler : MonoBehaviour
{
	/// <summary>
	/// The count of <see cref="int"/>s to multiply
	/// </summary>
	public int amount;
	/// <summary>
	/// How often a single calculation is repeated
	/// </summary>
	public int loops;
	/// <summary>
	/// The number of Runs taken on benchmarking
	/// </summary>
	public int runs;
	/// <summary>
	/// Calculator used for calculations 
	/// </summary>
	public Calculator calc;
	private ProgressBar BenchmarkProgressBar;
	private LongField SinglethreadedRuntimeField;
	private LongField MultiprocessedRuntimeField;
	private LongField ShaderRuntimeField;
	private UnsignedIntegerField AmountInput;

	private void OnEnable()
	{
		var uiDocument = GetComponent<UIDocument>();
		var root = uiDocument.rootVisualElement;
		Button RunSinglethreadedButton = root.Query<Button>("RunSinglethreaded");
		RunSinglethreadedButton.RegisterCallback<ClickEvent>(runSinglethreaded);
		Button RunMultiProcessedButton = root.Query<Button>("RunMultiProcessed");
		RunMultiProcessedButton.RegisterCallback<ClickEvent>(runMultiprocessed);
		Button RunShaderButton = root.Query<Button>("RunShader");
		RunShaderButton.RegisterCallback<ClickEvent>(runShader);
		Button RunBenchmarkButton = root.Query<Button>("RunBechmarkButton");
		RunBenchmarkButton.RegisterCallback<ClickEvent>(runBenchmark);

		AmountInput = root.Query<UnsignedIntegerField>("AmountInput");
		AmountInput.RegisterValueChangedCallback(updateAmount);
		amount = (int)AmountInput.value;
		UnsignedIntegerField LoopsInput = root.Query<UnsignedIntegerField>("LoopsInput");
		LoopsInput.RegisterValueChangedCallback(updateLoops);
		loops = (int)LoopsInput.value;
		UnsignedIntegerField RunsInput = root.Query<UnsignedIntegerField>("RunsInput");
		RunsInput.RegisterValueChangedCallback(updateRuns);
		runs = (int)RunsInput.value;

		BenchmarkProgressBar = root.Query<ProgressBar>("BenchmarkProgress");
		SinglethreadedRuntimeField = root.Query<LongField>("SinglethreadedLastRuntime");
		MultiprocessedRuntimeField = root.Query<LongField>("MultiprocessedLastRuntime");
		ShaderRuntimeField = root.Query<LongField>("ShaderLastRuntime");

		Button ExitButton = root.Query<Button>("ExitButton");
		ExitButton.RegisterCallback<ClickEvent>(ExitClick);
	}

	private void ExitClick(ClickEvent evt)
	{
		SceneManager.LoadScene("StartScene");
	}

	void runSinglethreaded(ClickEvent click)
	{
		SinglethreadedRuntimeField.value = calc.calculateSingelThreaded(amount, loops);
	}
	void runMultiprocessed(ClickEvent click)
	{
		MultiprocessedRuntimeField.value = calc.calculateMultiprocessed(amount, loops);
	}
	void runShader(ClickEvent click)
	{
		ShaderRuntimeField.value = calc.calculateParallelShader(amount, loops);
	}
	void runBenchmark(ClickEvent click)
	{
		StartCoroutine(benchmarkTask());
	}
	void updateAmount(ChangeEvent<uint> change)
	{
		if(change.newValue <= 65535*64)
		{
			amount = (int)change.newValue;
		}
		else
		{
			amount = 65535 * 64;
			AmountInput.value = (uint)amount;
		}
	}
	void updateLoops(ChangeEvent<uint> change)
	{
		loops = (int)change.newValue;
	}
	void updateRuns(ChangeEvent<uint> change)
	{
		runs = (int)change.newValue;
	}

	IEnumerator benchmarkTask()
	{
		long[] timeSingle = new long[runs];
		long[] timeMulti = new long[runs];
		long[] timeShader = new long[runs];
		for (int i = 0; i < runs; i++)
		{
			timeSingle[i] = calc.calculateSingelThreaded(amount, loops);
			timeMulti[i] = calc.calculateMultiprocessed(amount, loops);
			timeShader[i] = calc.calculateParallelShader(amount, loops);
			BenchmarkProgressBar.value = i+1 / (float)runs * 100;
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
		BenchmarkProgressBar.value = 0;
		yield return null;
	}
}
