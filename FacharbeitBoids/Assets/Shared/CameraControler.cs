using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
	private InputControles input;
	public Camera cam;
	public Cinemachine.CinemachineVirtualCameraBase[] virtualCameras;
	private int currentCamera;

	private void Update()
	{
		if (input.Player.Click.WasPressedThisFrame())
		{
			Ray ray = cam.ScreenPointToRay(input.Player.Point.ReadValue<Vector2>());
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				virtualCameras[1].Follow = hit.transform;
				virtualCameras[1].LookAt = hit.transform;
				virtualCameras[1].Priority = 10;
				virtualCameras[currentCamera].Priority = 0;
				currentCamera = 1;
			}
		}
		if (input.Player.Camera.WasPressedThisFrame())
		{
			virtualCameras[currentCamera].Priority = 0;
			if (currentCamera >= virtualCameras.Length-1)
			{
				currentCamera = 0;
			}
			else
			{
				currentCamera++;
			}
			virtualCameras[currentCamera].Priority = 10;
		}
	}


	private void Awake()
	{
		input = new InputControles();
		input.Player.Enable();
	}
	private void OnDisable()
	{
		input.Player.Disable();
	}
}