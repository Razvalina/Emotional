using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ActionPhaseScreen : MonoBehaviour, IScreen
{
	StateController StateController = null;
	public GameObject Player1;
	public GameObject Player2;

	private Player player1;
	private Player player2;

	public GameObject P1;
	public GameObject P2;

	// Update is called once per frame
	void Update()
	{
		this.Place(player1, P1);
		this.Place(player2, P2);
	}

	void Place(Player pl, GameObject go)
	{
		int activeSkill = pl.activeSkill;
		switch (activeSkill)
		{
			case 0:
				go.GetComponent<SpriteRenderer>().color = Color.white;
				break;
			case 1:
				go.GetComponent<SpriteRenderer>().color = Color.red;
				break;
			case 2:
				go.GetComponent<SpriteRenderer>().color = Color.green;
				break;
			case 3:
				go.GetComponent<SpriteRenderer>().color = Color.blue;
				break;
		}
			
		if (pl.Offset.magnitude > 0.0f)
		{
			go.transform.localPosition += new Vector3(pl.Offset.x, pl.Offset.y, 0.0f );
		}

		float Angle = Mathf.Atan2(pl.Direction.y, pl.Direction.x) * Mathf.Rad2Deg;
		bool isRight = (Angle >= -90.0f && Angle <= 90.0f);
		go.GetComponent<SpriteRenderer>().flipX = !isRight;
	}





	// Start is called before the first frame update
	public void ToShow(StateController st)
    {
		this.StateController = st;
		this.gameObject.SetActive(true);

		this.player1 = this.Player1.GetComponent<Player>();
		this.player2 = this.Player2.GetComponent<Player>();
		this.player1.SetAdaptiveType(true);
		this.player2.SetAdaptiveType(false);

		this.P1 = this.transform.Find("temp1").gameObject;
		this.P2 = this.transform.Find("temp2").gameObject;
	}
	public void ToHide()
	{
		if (this.StateController != null)
		{
			this.gameObject.SetActive(false);
			this.StateController.OnHide(this,"");
		}
		this.StateController = null;
	}


	public string getName()
	{
		return "ActionPhaseScreen";
	}




}
