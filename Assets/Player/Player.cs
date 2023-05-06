using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static DualshockAdaptive.SCE;

public class Player : MonoBehaviour
{
	public int PlayerID = 0;
	public Gamepad localGamepad { get; private set; } = null;

	// addaptive trigger data
	private int userId => this.PlayerID + 1;// SCE.SCE_USER_SERVICE_STATIC_USER_ID_1;
	private int dualShotHandler = -1;
	DualshockAdaptive.SCE.ScePadTriggerEffectParam addaptiveTrigger;

	bool isShoot = false;
	float fTriggerValue = 0.0f;

	float fBoostCharge = 0.0f;

	public int activeSkill { get; private set; } = 0;
	public Vector2 Direction { get; private set; } = Vector2.zero;
	public Vector2 Offset { get; private set; } = Vector2.zero;
	public float TimeSpanUlt = 10.0f;
	public float AwaitTillUlt = 10.0f;
	
	public GameObject UnitGo { get; private set; }
	public GameObject UnitGoSpr { get; private set; }
	public Unit Unit { get; private set; }
	public PlayerActionDelegate Delegate = null;



	// Start is called before the first frame update
	void Awake()
	{
		this.InitDSContoller();
		this.localGamepad = Gamepad.all[this.PlayerID];


	}


	public void UpdatePlayer()
	{
		if (this.Unit.character.isDead)
		{
			return;
		}

		

		// we have just dieL(
		if (this.Unit.character.isDead)
		{
			// just dieded
			this.Delegate?.onDie(this);
			return;
		}

		// angle
		this.Direction = this.localGamepad.rightStick.ReadValue();

		float fLeftTriggerValue = this.localGamepad.leftTrigger.value;
		if (fLeftTriggerValue > 0.2f)
		{
			// start particles for changing here
			this.fBoostCharge += Time.deltaTime * 0.5f;
			return;
		}
		if (fLeftTriggerValue < 0.2f && this.fBoostCharge > 0.0f)
		{
			// Add boost HERE
			Debug.Log("BOOSTED!! " + this.fBoostCharge);

			// add modificator for power
			Modifier mod = new Modifier();
			mod.Timer = 0.01f;
			mod.Stat = new Stat();
			mod.Stat.Power *= this.fBoostCharge;

			this.fBoostCharge = 0.0f;
		}


		this.fTriggerValue = this.localGamepad.rightTrigger.value;
		if (!this.isShoot && this.fTriggerValue > 0.85f)
		{
			this.isShoot = true;
			this.Delegate?.onFire(this, this.Unit.ActiveSkill);
			this.fBoostCharge = 0.0f;
		}
		if (this.isShoot && this.fTriggerValue < 0.2f)
		{
			this.isShoot = false;
		}



		// do not move by default
		this.Offset = Vector2.zero;

		Vector2 move = this.localGamepad.leftStick.ReadValue();
		float Radius = move.magnitude;
		if (Radius < 0.2f)
		{
			// deadzone
			Offset = Vector2.zero;
		}
		else
		{
			Offset = move * Radius * 0.05f;
		}


		// switch weapons here

		HandleButtons();

		if (this.AwaitTillUlt >= 0.0f)
		{
			AwaitTillUlt -= Time.deltaTime;
			if (AwaitTillUlt < 0.0f)
				AwaitTillUlt = 0.0f;
		}
		if (this.localGamepad.rightStickButton.isPressed && this.localGamepad.rightStickButton.isPressed)
		{
			if (this.AwaitTillUlt <= 0.0f)
			{
				this.Delegate?.onUlt(this, this.Unit.UltSkill);
				this.AwaitTillUlt = this.TimeSpanUlt;
			}
		}



		int activeSkill = this.activeSkill;
		//switch (activeSkill)
		//{
		//	case 0:
		//		go.GetComponent<SpriteRenderer>().color = Color.white;
		//		break;
		//	case 1:
		//		go.GetComponent<SpriteRenderer>().color = Color.red;
		//		break;
		//	case 2:
		//		go.GetComponent<SpriteRenderer>().color = Color.green;
		//		break;
		//	case 3:
		//		go.GetComponent<SpriteRenderer>().color = Color.blue;
		//		break;
		//}

		if (this.Offset.magnitude > 0.0f)
		{
			this.UnitGo.transform.localPosition += new Vector3(this.Offset.x, this.Offset.y, 0.0f) * this.Unit.character.Current.Speed;
		}

		float Angle = Mathf.Atan2(this.Direction.y, this.Direction.x) * Mathf.Rad2Deg;
		bool isRight = (Angle >= -90.0f && Angle <= 90.0f);
		this.UnitGoSpr.GetComponent<SpriteRenderer>().flipX = !isRight;
	}


	public void InitUnit(GameObject go)
	{
		if (this.UnitGo != null)
			Destroy(this.UnitGo);
		this.UnitGo = go;
		this.Unit = this.UnitGo.GetComponent<Unit>();
		this.UnitGoSpr = this.UnitGo.transform.Find("unit").gameObject;

		this.Unit.UpdateCharacter();
	}

	private void OnDestroy()
	{
		if (this.dualShotHandler != -1)
		{
			SCE.scePadClose(this.dualShotHandler);
			this.dualShotHandler = -1;
		}
	}


	void HandleButtons()
	{
		if (this.localGamepad.buttonSouth.wasReleasedThisFrame)
		{
			this.activeSkill = 0;
		}
		if (this.localGamepad.buttonWest.wasReleasedThisFrame)
		{
			this.activeSkill = 1;
		}
		if (this.localGamepad.buttonNorth.wasReleasedThisFrame)
		{
			this.activeSkill = 2;
		}
		if (this.localGamepad.buttonEast.wasReleasedThisFrame)
		{
			this.activeSkill = 3;
		}
	}


	public void SetAdaptiveType(bool isDD)
	{
		if (this.dualShotHandler == -1)
			return;

		this.addaptiveTrigger = new SCE.ScePadTriggerEffectParam();
		this.addaptiveTrigger.triggerMask = SCE.SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_L2 | SCE.SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_R2;
		this.addaptiveTrigger.command = new SCE.ScePadTriggerEffectCommand[SCE.SCE_PAD_TRIGGER_EFFECT_TRIGGER_NUM];
		this.addaptiveTrigger.padding = new byte[7];

		if (isDD)
		{
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].padding = new byte[4];
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].mode = (int)SCE.ScePadTriggerEffectMode.SCE_PAD_TRIGGER_EFFECT_MODE_FEEDBACK;

			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].commandData = new ScePadTriggerEffectCommandData();
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].commandData.feedbackParam.position = 3;
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].commandData.feedbackParam.strength = 7;



			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].padding = new byte[4];
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].mode = (int)SCE.ScePadTriggerEffectMode.SCE_PAD_TRIGGER_EFFECT_MODE_FEEDBACK;

			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].commandData = new ScePadTriggerEffectCommandData();
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].commandData.feedbackParam.position = 3;
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].commandData.feedbackParam.strength = 7;

		}
		else
		{
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].padding = new byte[4];
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].mode = (int)SCE.ScePadTriggerEffectMode.SCE_PAD_TRIGGER_EFFECT_MODE_WEAPON;

			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].commandData = new ScePadTriggerEffectCommandData();
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].commandData.weaponParam.endPosition = 5;
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].commandData.weaponParam.startPosition = 3;
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2].commandData.weaponParam.strength = 7;



			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].padding = new byte[4];
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].mode = (int)SCE.ScePadTriggerEffectMode.SCE_PAD_TRIGGER_EFFECT_MODE_WEAPON;

			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].commandData = new ScePadTriggerEffectCommandData();
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].commandData.weaponParam.endPosition = 5;
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].commandData.weaponParam.startPosition = 4;
			this.addaptiveTrigger.command[SCE.SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2].commandData.weaponParam.strength = 7;
		}
		int ret = SCE.scePadSetTriggerEffect(this.dualShotHandler, ref this.addaptiveTrigger);
		if (ret != 0)
		{
			Debug.Log("can't assign triggerEffect to Left");
		}
	}

	private void InitDSContoller()
	{
		int ret = SCE.scePadInit();
		if (ret != 0)
		{
			Debug.Log("Can't init libScePad");
			return;
		}

		this.dualShotHandler = SCE.scePadOpen(this.userId, 0, 0, IntPtr.Zero);
		if ((uint)this.dualShotHandler == SCE.SCE_PAD_ERROR_ALREADY_OPENED)
		{
			// need to get control
			this.dualShotHandler = SCE.scePadGetHandle(this.userId, 0, 0);
			if ((uint)this.dualShotHandler == SCE.SCE_PAD_ERROR_NO_HANDLE)
			{
				Debug.Log("Controller is inited somewhere, but tool couldn't attach to it. \nDo something with contoller and restart tool");
				dualShotHandler = -1;
				return;
			}
		}



	}
}
