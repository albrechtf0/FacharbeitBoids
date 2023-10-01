using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.ShaderData;

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
        Button SimMultiProcessed = root.Query<Button>("SimMultiProcessed");
        SimMultiProcessed.RegisterCallback<ClickEvent>(LoadSimMultiProcessed);
		Button SimShader = root.Query<Button>("SimShader");
		SimShader.RegisterCallback<ClickEvent>(LoadSimShader);
	}

	private void LoadSimShader(ClickEvent evt)
	{
		SceneManager.LoadScene("BoidSimulation");
	}

	private void LoadSimMultiProcessed(ClickEvent evt)
	{
		SceneManager.LoadScene("BoidSimulation");
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