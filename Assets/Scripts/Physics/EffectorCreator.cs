using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EffectorCreator : MonoBehaviour
{
	[SerializeField] Body effectorPrefab;
	[SerializeField] FloatData force;
	[SerializeField] EnumData effectorMode;

	bool action = false;
	bool pressed = false;

	void Update()
	{
		if (action && pressed)
		{
			pressed = false;

			Vector3 position = Simulator.Instance.GetScreenToWorldPosition(Input.mousePosition);
			Body body = Instantiate(effectorPrefab, position, Quaternion.identity);

			Simulator.Instance.bodies.Add(body);
		}
	}

	public void OnPointerDown()
	{
		if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
		{
			action = true;
			pressed = true;
		}
	}

	public void OnPointerExit()
	{
		action = false;
	}

	public void OnPointerUp()
	{
		action = false;
	}
}
