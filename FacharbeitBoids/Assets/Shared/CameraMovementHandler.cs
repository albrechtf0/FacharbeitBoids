using UnityEngine;
using UnityEngine.InputSystem;

public class CameraVovementHandler : MonoBehaviour
{
	public Cinemachine.CinemachineVirtualCameraBase VCamera;
	public Camera mainCamera;
	public float moveSpeed;
	private InputControles input;

	private void Update()
	{
		if (Cinemachine.CinemachineCore.Instance.IsLive(VCamera))
		{
			transform.position += mainCamera.transform.forward * input.Player.Move.ReadValue<Vector2>().y * moveSpeed * Time.deltaTime;
			transform.position += mainCamera.transform.right * input.Player.Move.ReadValue<Vector2>().x * moveSpeed * Time.deltaTime;
			transform.position += Vector3.up * input.Player.Vertical.ReadValue<float>() * moveSpeed * Time.deltaTime;
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