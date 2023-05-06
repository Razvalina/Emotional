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

	private Vector2	DefPos1 = Vector2.zero;
	private Vector2 DefPos2 = Vector2.zero;


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
		this.timeLoading = 5.0f;

		this.player1 = this.Player1.GetComponent<Player>();
		this.player2 = this.Player2.GetComponent<Player>();

		if (this.player1.UnitGo != null)
		{
			this.DefPos1 = this.player1.UnitGo.transform.localPosition;
			this.player1.UnitGo.transform.localPosition = new Vector2(0.46f, 3.57f) ;
		}
		if (this.player2.UnitGo != null)
		{
			this.DefPos2 = this.player2.UnitGo.transform.localPosition;
			this.player2.UnitGo.transform.localPosition = new Vector2(-4.5f, 2.2f);
		}
	}
	public void ToHide()
	{
		if (this.StateController != null)
		{
			string tomove = "";
			if (this.player1.UnitGo != null)
			{
				this.player1.UnitGo.transform.localPosition = this.DefPos1;
				tomove = "ActionPhaseScreen";
			}
			if (this.player2.UnitGo != null)
			{
				this.player2.UnitGo.transform.localPosition = this.DefPos2;
			}

			this.gameObject.SetActive(false);
			this.StateController.OnHide(this, tomove);
		}
		this.StateController = null;
	}


	public string getName()
	{
		return "LoadingScreen";
	}
}
