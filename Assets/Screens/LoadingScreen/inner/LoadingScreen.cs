using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour, IScreen
{
	StateController StateController = null;
	public GameObject Player1;
	public GameObject Player2;

	private Player player1;
	private Player player2;

	public float timeLoading = 5.0f;


	// Update is called once per frame
	void Update()
	{
		if (this.timeLoading < 0.0f)
		{
			this.ToHide();
		}
		this.timeLoading -= Time.deltaTime;
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
		if (this.StateController != null)
		{
			this.gameObject.SetActive(false);
			this.StateController.OnHide(this);
		}
		this.StateController = null;
	}


	public string getName()
	{
		return "LoadingScreen";
	}
}
