using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public partial class ActionPhaseScreen : MonoBehaviour, IScreen, PlayerActionDelegate
{
	StateController StateController = null;
	public GameObject Player1;
	public GameObject Player2;
	public GameObject Camera;

	private Player player1;
	private Player player2;

	public GameObject P1;
	public GameObject P2;

	float fTimeWave = 5000.0f;
	//List<float> Waves = new List<float>();

	// Update is called once per frame
	void Update()
	{
		this.fTimeWave -= Time.deltaTime;
		if (fTimeWave < 0.0f)
		{
			fTimeWave = 5.0f;
			this.Camera.GetComponent<Camera>().orthographicSize += 5.0f;

			Modifier mod = new Modifier();
			mod.Timer = 99999;
			mod.Stat.Speed = 1.5f;
			this.player1.Unit.character.Modifiers.Add(mod);

			Modifier mod2 = new Modifier();
			mod2.Timer = 99999;
			mod2.Stat.Speed = 1.5f;
			this.player2.Unit.character.Modifiers.Add(mod2);


			fTimeWave = 5.0f;
		}

		this.player1.UpdatePlayer();
		this.player2.UpdatePlayer();

	}


	public void onDie(Player pl)
	{
		throw new System.NotImplementedException();
	}

	public void onFire(Player pl, Skill skill)
	{
		if (skill == null || skill.Visual == null)
			return;

		float fpower = skill.Use();
		if (fpower < 0.0f)
		{
			// blocked
			return;
		}
		GameObject g = Instantiate(skill.Visual, pl.UnitGo.transform.localPosition, Quaternion.identity);
		if (g != null)
		{
			bullet_fly u = g.GetComponent<bullet_fly>();
			if (u != null)
			{
				u.skill = skill;
				u.player = pl;
				u.unit = pl.Unit;
				u.Power = fpower;
				u.Init();
			}
		}

	}

	public void onUlt(Player pl, Skill skill)
	{
		throw new System.NotImplementedException();
	}






	// Start is called before the first frame update
	public void ToShow(StateController st)
    {
		this.StateController = st;
		this.gameObject.SetActive(true);

		this.player1 = this.Player1.GetComponent<Player>();
		this.player2 = this.Player2.GetComponent<Player>();
		this.player1.SetAdaptiveType(this.player1.Unit.isDamageDriver);
		this.player2.SetAdaptiveType(this.player2.Unit.isDamageDriver);

		this.player1.Delegate = this;
		this.player2.Delegate = this;

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
