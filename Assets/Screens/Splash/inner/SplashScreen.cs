using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour, IScreen
{
	StateController StateController = null;
	public GameObject Player1;
	public GameObject Player2;

	private Player player1;
	private Player player2;


	// Update is called once per frame
	void Update()
	{

	}

	// Start is called before the first frame update
	public void ToShow(StateController st)
    {
		this.StateController = st;
		this.gameObject.SetActive(true);

		this.player1 = this.Player1.GetComponent<Player>();
		this.player2 = this.Player2.GetComponent<Player>();
	}
	public void ToHide()
	{
		this.gameObject.SetActive(false);
		this.StateController.OnHide(this);
	}



	// contorller callback for skipping
	void OnTriClick()
	{
		this.ToHide();
	}

	public string getName()
	{
		return "SplashScreen";
	}

	public void StartController()
	{
		this.player1.Controller.Splash.Enable();
		this.player2.Controller.Splash.Enable();

		this.player1.Controller.Splash.TriClick.started += context => OnTriClick();
		this.player2.Controller.Splash.TriClick.started += context => OnTriClick();
	}

	public void StopController()
	{
		this.player1.Controller.Splash.TriClick.started += null;
		this.player2.Controller.Splash.TriClick.started += null;

		this.player1.Controller.Splash.Disable();
		this.player2.Controller.Splash.Disable();
	}
}
