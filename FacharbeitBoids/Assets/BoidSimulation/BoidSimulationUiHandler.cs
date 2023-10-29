using DataContainer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoidSimulationUiHandler : MonoBehaviour
{
	public UIDocument ui;
	public BoidGroupHandler boidGroupHandler;
	public float BechmarkTime = 300f;

	private UnsignedIntegerField BoidCount;
	private FloatField LookRadius;
	private FloatField AvoidanceRadius;
	private FloatField Speed;
	private FloatField MaxTurningSpeed;
	private FloatField TurningTime;
	private FloatField DirectionStrength;
	private FloatField CohesionStrength;
	private FloatField AvoidanceStrength;
	private FloatField ObjectAvoidanceStrength;
	private Button BechmarkButton;
	private FloatField BenchmarkTimeField;

	void Start()
	{
		var root = ui.rootVisualElement;
		Button ExitButton = root.Query<Button>("ExitButton");
		ExitButton.RegisterCallback<ClickEvent>(ExitGame);
		BoidCount = root.Query<UnsignedIntegerField>("BoidCount");
		BoidCount.RegisterValueChangedCallback(BoidCountChanged);
		LookRadius = root.Query<FloatField>("LookRadius");
		LookRadius.RegisterValueChangedCallback(LookRadiusChanged);
		AvoidanceRadius = root.Query<FloatField>("AvoidanceRadius");
		AvoidanceRadius.RegisterValueChangedCallback(AvoidanceRadiusChanged);
		Speed = root.Query<FloatField>("Speed");
		Speed.RegisterValueChangedCallback(SpeedChanged);
		MaxTurningSpeed = root.Query<FloatField>("MaxTurningSpeed");
		MaxTurningSpeed.RegisterValueChangedCallback(MaxTurningSpeedChanged);
		TurningTime = root.Query<FloatField>("TurningTime");
		TurningTime.RegisterValueChangedCallback(TurningTimeChanged);
		DirectionStrength = root.Query<FloatField>("DirectionStrength");
		DirectionStrength.RegisterValueChangedCallback(DirectionStrengthChanged);
		CohesionStrength = root.Query<FloatField>("CohesionStrength");
		CohesionStrength.RegisterValueChangedCallback(CohesionStrengthChanged);
		AvoidanceStrength = root.Query<FloatField>("AvoidanceStrength");
		AvoidanceStrength.RegisterValueChangedCallback(AvoidnceStrengthChanged);
		ObjectAvoidanceStrength = root.Query<FloatField>("ObjectAvoidanceStrength");
		ObjectAvoidanceStrength.RegisterValueChangedCallback(ObjectAvoidanceStrengthChanged);
		BechmarkButton = root.Query<Button>("BechmarkButton");
		BechmarkButton.RegisterCallback<ClickEvent>(StartBechmark);
		BenchmarkTimeField = root.Query<FloatField>("BenchmarkTime");
		BenchmarkTimeField.RegisterValueChangedCallback(BenchmarkTimeChanged);
	}

	private void BenchmarkTimeChanged(ChangeEvent<float> evt)
	{
		BechmarkTime = evt.newValue;
	}

	private void StartBechmark(ClickEvent evt)
	{
		StartCoroutine(bechmark());
	}

	private void ObjectAvoidanceStrengthChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidObjectAvoidanceStrength(evt.newValue);
	}

	private void AvoidnceStrengthChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidAvoidanceStrength(evt.newValue);
	}

	private void CohesionStrengthChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidCohesionStrength(evt.newValue);
	}

	private void DirectionStrengthChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidDirectionStrength(evt.newValue);
	}

	private void TurningTimeChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidTurningTime(evt.newValue);
	}

	private void MaxTurningSpeedChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidMaxTurningSpeed(evt.newValue);
	}

	private void SpeedChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidSpeed(evt.newValue);
	}

	private void AvoidanceRadiusChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidAvoidanceRadius(evt.newValue);
	}

	private void LookRadiusChanged(ChangeEvent<float> evt)
	{
		boidGroupHandler.setBoidLookRadius(evt.newValue);
	}

	private void BoidCountChanged(ChangeEvent<uint> evt)
	{
		boidGroupHandler.ClearBoids();
		boidGroupHandler.CreateBoids((int)evt.newValue);
		boidGroupHandler.setBoidLookRadius(LookRadius.value);
		boidGroupHandler.setBoidAvoidanceRadius(AvoidanceRadius.value);
		boidGroupHandler.setBoidSpeed(Speed.value);
		boidGroupHandler.setBoidMaxTurningSpeed(MaxTurningSpeed.value);
		boidGroupHandler.setBoidTurningTime(TurningTime.value);
		boidGroupHandler.setBoidDirectionStrength(DirectionStrength.value);
		boidGroupHandler.setBoidCohesionStrength(CohesionStrength.value);
		boidGroupHandler.setBoidAvoidanceStrength(AvoidanceStrength.value);
		boidGroupHandler.setBoidObjectAvoidanceStrength(ObjectAvoidanceStrength.value);
	}

	private void ExitGame(ClickEvent evt)
	{
		SceneManager.LoadScene("StartScene");
	}

	IEnumerator bechmark()
	{
		BechmarkButton.SetEnabled(false);
		Debug.Log("StartingBenchmark");
		float Runtime = 0;
		int Frame = 0; 
		using (StreamWriter file = new StreamWriter(Path.Combine(Application.persistentDataPath, "SingleThreaded.csv")))
		{
			file.WriteLine("BoidCount; LookRadius; AvoidanceRadius; Speed; DirectionStrength; CohesionStrength; AvoidanceStrength; ObjektAvoidanceStrength; MaxTurningSpeed; TurningTime");
			BoidHandler boid = GameObject.FindGameObjectWithTag("Boid").GetComponent<BoidHandler>();
			file.WriteLine($"{boidGroupHandler.BoidCount}; {boid.LookRadius}; {boid.AvoidanceRadius}; {boid.Speed}; {boid.DirectionStrength}; {boid.CohesionStrength}; {boid.AvoidanceStrength}; {boid.ObjektAvoidanceStrength}; {boid.MaxTurningSpeed}; {boid.TurningTime}");
			file.WriteLine("PassedTime; Frame; FPS; FrameTimes");
			while (Runtime < BechmarkTime)
			{
				Runtime += Time.deltaTime;
				Frame++;
				file.WriteLine($"{Runtime}; {Frame}; {1f / Time.deltaTime}; {Time.deltaTime}");
				yield return null;
			}
		}
		yield return null;
		Debug.Log("Ending Benchmark");
		BechmarkButton.SetEnabled(true);
	}
}