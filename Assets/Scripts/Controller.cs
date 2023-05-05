using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
	CircleCollider2D CircleCollider2D;
	GameObject Inner;

	Vector2 Direction = Vector2.zero;
	float Angle = 0.0f;


	bool isCaptured = false;
	Vector2 CapturePos;
	// Start is called before the first frame update
	void Start()
	{
		this.CircleCollider2D = GetComponent<CircleCollider2D>();
		this.Inner = this.transform.Find("Inner").gameObject;

		this.SetVisible(true);
	}

	public void SetVisible(bool value)
	{
		this.GetComponent<Renderer>().enabled = value;
		this.Inner.GetComponent<Renderer>().enabled = value;
	}

	public Vector2 getDirection()
	{
		return this.Direction;
	}
	public void SetPosition(Vector2 pos)
	{
		this.transform.position = pos;
	}

	public float getAngle()
	{
		return this.Angle;
	}

	public void BeginTouch(Vector2 mousePos2D)
	{
		this.isCaptured = true;
		this.CapturePos = mousePos2D;
		this.Inner.transform.localPosition = new Vector3(0.0f, 0.0f);
		
	}

	public void MoveTouch(Vector2 mousePos2D)
	{
		if (!this.isCaptured)
			return;

		Vector2 Diff = (mousePos2D - this.CapturePos) / this.transform.localScale.x;

		// bound to distance
		Vector2 newPos = this.Inner.transform.localPosition + new Vector3(Diff.x, Diff.y);
		float Radius = newPos.magnitude;
		Radius = Math.Clamp(Radius, 0.0f, 0.4f);// hardcode!

		this.Direction = newPos.normalized * Radius;
		this.Inner.transform.localPosition = this.Direction ;
		this.CapturePos = mousePos2D;

		Debug.LogFormat( "{0}   {1}",this.Direction.x, this.Direction.y);

		this.Angle = Mathf.Atan2(this.Inner.transform.localPosition.y, this.Inner.transform.localPosition.x) * Mathf.Rad2Deg;
		
	}

	public void EndTouch()
	{
		this.Direction = Vector2.zero;
		if (!this.isCaptured)
			return;

		//return back inner
		// release capture
		this.isCaptured = false;
		this.Inner.transform.localPosition = new Vector3(0.0f, 0.0f);

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchCount != 0)
		{
			Touch touch = Input.GetTouch(0);
			Debug.Log(touch.azimuthAngle);
		}

		var gamepad = Gamepad.current;
		if (gamepad == null)
			return; // No gamepad connected.



		if (gamepad.rightTrigger.wasPressedThisFrame)
		{
			// 'Use' code here
		}

		Vector2 move = gamepad.leftStick.ReadValue();
		float Radius = move.magnitude;
		if (Radius < 0.2f)
		{
			// deadzone
			Direction = Vector2.zero;
			return;
		}
		Radius = Math.Clamp(Radius, 0.0f, 0.4f);// hardco
		Direction += move * Radius * 0.25f;
		Debug.Log(move.x);
		
	}
}
