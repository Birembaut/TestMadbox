using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Virtual joystick for mobile joystick control
/// </summary>
public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	public RectTransform backgroundJoystick;
	public RectTransform innerStick;

	public float HandleLimit = 1f;

	public Vector2 input = Vector2.zero;

	Vector2 joyPosition = Vector2.zero;
	Vector2 initialPosition;

	private void Start()
	{
		initialPosition = backgroundJoystick.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 joyDirection = eventData.position - joyPosition;
		input = (joyDirection.magnitude > backgroundJoystick.sizeDelta.x / 2f) ? joyDirection.normalized : joyDirection / (backgroundJoystick.sizeDelta.x / 2f);
		
		innerStick.anchoredPosition = (input * backgroundJoystick.sizeDelta.x / 2f) * HandleLimit;

		GameManager.Instance.InputManager.OnInputMoved(input);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		input = Vector2.zero;
		innerStick.anchoredPosition = Vector2.zero;
		backgroundJoystick.position = initialPosition;

		GameManager.Instance.InputManager.OnInputMoved(input);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);
		joyPosition = eventData.position;
		backgroundJoystick.position = eventData.position;
		innerStick.anchoredPosition = Vector2.zero;
	}
}