using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoidSimulationShaderUiHandler : MonoBehaviour
{
	public UIDocument ui;
	public BoidShaderHandler boidShaderHandler;

	private UnsignedIntegerField BoidCount;
	private FloatField LookRadius;
	private FloatField AvoidanceRadius;
	private FloatField Speed;
	private Vector3Field LerpFactor; 
	private FloatField DirectionStrength;
	private FloatField CohesionStrength;
	private FloatField AvoidanceStrength;
	private FloatField ObjectAvoidanceStrength;

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

}