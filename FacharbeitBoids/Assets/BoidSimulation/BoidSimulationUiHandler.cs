using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoidSimulationUiHandler : MonoBehaviour
{
	public UIDocument ui;
	public BoidGroupHandler boidGroupHandler;

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
	
}