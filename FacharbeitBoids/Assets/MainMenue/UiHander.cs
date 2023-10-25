using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UiHander : MonoBehaviour
{
    public UIDocument ui;

    void Start()
    {
        var root = ui.rootVisualElement;
        Button ParallelExample = root.Query<Button>("ParallelExample");
        ParallelExample.RegisterCallback<ClickEvent>(LoadParallelExample);
        Button SimSingleThreaded = root.Query<Button>("SimSingleThreaded");
        SimSingleThreaded.RegisterCallback<ClickEvent>(LoadSimSingleThreaded);
		Button SimShader = root.Query<Button>("SimShader");
		SimShader.RegisterCallback<ClickEvent>(LoadSimShader);
		Button ExitButton = root.Query<Button>("ExitButton");
		ExitButton.RegisterCallback<ClickEvent>(ExitGame);
	}

	private void ExitGame(ClickEvent evt)
	{
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}

	private void LoadSimShader(ClickEvent evt)
	{
		SceneManager.LoadScene("BoidSimulationShader");
	}

	private void LoadSimSingleThreaded(ClickEvent evt)
	{
		SceneManager.LoadScene("BoidSimulation");
	}

	private void LoadParallelExample(ClickEvent evt)
	{
        SceneManager.LoadScene("Parallelisation Example");
	}
}