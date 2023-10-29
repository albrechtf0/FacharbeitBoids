using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoidSimulationShaderUiHandler : MonoBehaviour
{
	public UIDocument ui;
	public BoidShaderHandler boidShaderHandler;
	public float BechmarkTime = 300f;

	private UnsignedIntegerField BoidCount;
	private FloatField LookRadius;
	private FloatField AvoidanceRadius;
	private FloatField Speed;
	private Vector3Field LerpFactor;
	private FloatField DirectionStrength;
	private FloatField CohesionStrength;
	private FloatField AvoidanceStrength;
	private FloatField ObjectAvoidanceStrength;
	private Button BechmarkButton;
	private FloatField BenchmarkTimeField;
	public FloatField FPSCounter;

	void Start()
	{
		VisualElement root = ui.rootVisualElement;
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
		LerpFactor = root.Query<Vector3Field>("LerpFactor");
		LerpFactor.RegisterValueChangedCallback(LerpFactorChanged);
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
		FPSCounter = root.Query<FloatField>("FPSCounter");
	}

	private void Update()
	{
		FPSCounter.value = Mathf.Round(1f / Time.deltaTime);
	}

	private void BenchmarkTimeChanged(ChangeEvent<float> evt)
	{
		BechmarkTime = evt.newValue;
	}

	private void StartBechmark(ClickEvent evt)
	{
		StartCoroutine(bechmark());
	}
	private void LerpFactorChanged(ChangeEvent<Vector3> evt)
	{
		boidShaderHandler.setBoidLerpFactor(evt.newValue);
	}

	private void ObjectAvoidanceStrengthChanged(ChangeEvent<float> evt)
	{
		boidShaderHandler.setBoidObjectAvoidanceStrength(evt.newValue);
	}

	private void AvoidnceStrengthChanged(ChangeEvent<float> evt)
	{
		boidShaderHandler.setBoidAvoidanceStrength(evt.newValue);
	}

	private void CohesionStrengthChanged(ChangeEvent<float> evt)
	{
		boidShaderHandler.setBoidCohesionStrength(evt.newValue);
	}

	private void DirectionStrengthChanged(ChangeEvent<float> evt)
	{
		boidShaderHandler.setBoidDirectionStrength(evt.newValue);
	}

	private void SpeedChanged(ChangeEvent<float> evt)
	{
		boidShaderHandler.setBoidSpeed(evt.newValue);
	}

	private void AvoidanceRadiusChanged(ChangeEvent<float> evt)
	{
		boidShaderHandler.setBoidAvoidanceRadius(evt.newValue);
	}

	private void LookRadiusChanged(ChangeEvent<float> evt)
	{
		boidShaderHandler.setBoidLookRadius(evt.newValue);
	}

	private void BoidCountChanged(ChangeEvent<uint> evt)
	{
		boidShaderHandler.setBoidCount((int)evt.newValue);
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
		using (StreamWriter file = new StreamWriter(Path.Combine(Application.persistentDataPath, "Shader.csv")))
		{
			file.WriteLine("BoidCount; LookRadius; AvoidanceRadius; Speed; DirectionStrength; CohesionStrength; AvoidanceStrength; ObjektAvoidanceStrength; lerpFactor");
			file.WriteLine($"{boidShaderHandler.count}; {boidShaderHandler.LookRadius}; {boidShaderHandler.AvoidanceRadius}; {boidShaderHandler.Speed}; {boidShaderHandler.DirectionStrength}; {boidShaderHandler.CohesionStrength}; {boidShaderHandler.AvoidanceStrength}; {boidShaderHandler.ObjektAvoidanceStrength}; {boidShaderHandler.lerpFactor}");
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