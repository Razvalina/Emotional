using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplashScreen : MonoBehaviour, IScreen
{
	StateController StateController = null;

	protected GameObject Planetext;
	bool canProceed = false;

	public void Start()
	{
		this.Planetext = this.transform.Find("mathafaka").gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		if (Gamepad.all.Count != 2)
		{
			this.Planetext.GetComponent<Renderer>().enabled = true;
			this.canProceed = false;
		}
		else
		{
			this.Planetext.GetComponent<Renderer>().enabled = false;
			this.canProceed = true;
		}

		if (this.canProceed && Gamepad.all[1].buttonNorth.wasReleasedThisFrame || Gamepad.all[0].buttonNorth.wasReleasedThisFrame)
		{
			this.ToHide();
		}
	}

	// Start is called before the first frame update
	public void ToShow(StateController st)
    {
		this.StateController = st;
		this.gameObject.SetActive(true);

	}
	public void ToHide()
	{
		this.gameObject.SetActive(false);
		this.StateController.OnHide(this, "");
	}

	public string getName()
	{
		return "SplashScreen";
	}
}
