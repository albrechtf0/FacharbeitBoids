using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoidSimulationShaderUiHandler : MonoBehaviour
{
	public UIDocument ui;
	void Start()
    {
		var root = ui.rootVisualElement;
		Button ExitButton = root.Query<Button>("ExitButton");
		ExitButton.RegisterCallback<ClickEvent>(ExitGame);
	}

	private void ExitGame(ClickEvent evt)
	{
		SceneManager.LoadScene("StartScene");
	}

}