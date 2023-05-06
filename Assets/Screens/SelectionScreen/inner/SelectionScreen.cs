using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScreen : MonoBehaviour, IScreen
{
	StateController StateController = null;
	public GameObject Player1;
	public GameObject Player2;

	public GameObject Unit1;
	public GameObject Unit2;
	public GameObject Unit3;
	public GameObject Unit4;

	private Player player1;
	private Player player2;


	// places
	// 0 , 1      1 <- 0 -> 1 -> 0
	// 2 , 3    

	int selectedFrameP1 = 0;
	int selectedFrameP2 = 1;

	private GameObject FramePlayer1;
	private GameObject FramePlayer2;

	bool isPlayer1Ready = false;
	bool isPlayer2Ready = false;
	Color defFrameCol1 = Color.white;
	Color defFrameCol2 = Color.white;

	bool isBlockInput = true;
	float timoutBeforeLoading = 1.0f;

	public void Start()
	{
		this.FramePlayer1 = this.transform.Find("frame_Player1").gameObject;
		this.FramePlayer2 = this.transform.Find("frame_Player2").gameObject;
		this.defFrameCol1 = this.FramePlayer1.GetComponent<SpriteRenderer>().color;
		this.defFrameCol2 = this.FramePlayer2.GetComponent<SpriteRenderer>().color;

	}

	// Update is called once per frame
	void Update()
	{
		if (!this.isBlockInput)
		{
			this.ChangeSelectedUser(this.player1, ref this.selectedFrameP1, ref this.isPlayer1Ready, this.defFrameCol1, this.FramePlayer1, true);
			this.ChangeSelectedUser(this.player2, ref this.selectedFrameP2, ref this.isPlayer2Ready, this.defFrameCol2, this.FramePlayer2, false);

			if (this.isPlayer1Ready && this.isPlayer2Ready)
			{
				isBlockInput = true;
				// start  tru music here
			}
		}


		if (this.isBlockInput && this.timoutBeforeLoading > 0.0f)
		{
			this.timoutBeforeLoading -= Time.deltaTime;
			if (this.timoutBeforeLoading <= 0.0f)
			{
				this.ToHide();
			}
		}
		

	}

	private void ChangeSelectedUser( Player player, ref int selectedFrame, ref bool isReady, Color defFrame, GameObject FramePlayer, bool isPlayer1 )
	{
		// selecting
		if (player.localGamepad.crossButton.wasReleasedThisFrame)
		{
			isReady = !isReady;
			if (isReady)
			{
				FramePlayer.GetComponent<SpriteRenderer>().color = Color.cyan;
			}
			else
			{
				FramePlayer.GetComponent<SpriteRenderer>().color = defFrame;
			}
		}

		// is confirmed frame, can't move
		if (isReady)
			return;

		//dont wanna make linked list... hello harcode
		Vector2[] places = new Vector2[4];
		places[0] = new Vector2(-1.4f, 1.65f );
		places[1] = new Vector2(+1.4f, 1.65f);
		places[2] = new Vector2(-1.4f, -2.1f);
		places[3] = new Vector2(+1.4f, -2.1f);

		// horizontal
		bool changed = false;
		if (player.localGamepad.dpad.left.wasReleasedThisFrame || player.localGamepad.dpad.right.wasReleasedThisFrame)
		{
			if (selectedFrame == 0)
				selectedFrame = 1;
			else if (selectedFrame == 1)
				selectedFrame = 0;
			else if (selectedFrame == 2)
				selectedFrame = 3;
			else if (selectedFrame == 3)
				selectedFrame = 2;
			changed = true;
		}

		// vertical
		if (player.localGamepad.dpad.up.wasReleasedThisFrame || player.localGamepad.dpad.down.wasReleasedThisFrame)
		{
			if (selectedFrame == 0)
				selectedFrame = 2;
			else if (selectedFrame == 1)
				selectedFrame = 3;
			else if (selectedFrame == 2)
				selectedFrame = 0;
			else if (selectedFrame == 3)
				selectedFrame = 1;
			changed = true;
		}

		if (changed)
		{
			FramePlayer.transform.localPosition = places[selectedFrame];

			SelectUnit(player, isPlayer1, selectedFrame);
		}



	}

	private void SelectUnit(Player player, bool isPlayer1, int selectedFrame)
	{
		Vector2 v = new Vector2(-7.0f, -1.7f);
		if (!isPlayer1)
			v.x = 7.0f;

		if (selectedFrame == 0)
		{
			player.InitUnit(Instantiate(this.Unit1, v, Quaternion.identity));
		}
		if (selectedFrame == 1)
		{
			player.InitUnit(Instantiate(this.Unit2, v, Quaternion.identity));
		}
		if (selectedFrame == 2)
		{
			player.InitUnit(Instantiate(this.Unit3, v, Quaternion.identity));
		}
		if (selectedFrame == 3)
		{
			player.InitUnit(Instantiate(this.Unit4, v, Quaternion.identity));
		}
	}


	// Start is called before the first frame update
	public void ToShow(StateController st)
    {
		this.StateController = st;
		this.gameObject.SetActive(true);

		this.player1 = this.Player1.GetComponent<Player>();
		this.player2 = this.Player2.GetComponent<Player>();


		SelectUnit(this.player1, true, this.selectedFrameP1);
		SelectUnit(this.player2, false, this.selectedFrameP2);

		this.isBlockInput = false;
		this.timoutBeforeLoading = 5.0f;

	}
	public void ToHide()
	{
		if (this.StateController != null)
		{
			this.gameObject.SetActive(false);
			this.StateController.OnHide(this, "LoadingScreen");
		}
		this.StateController = null;
	}


	public string getName()
	{
		return "SelectionScreen";
	}
}
