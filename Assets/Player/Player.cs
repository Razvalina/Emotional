using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DualshockAdaptive.SCE;

public class Player : MonoBehaviour
{
	public int PlayerID = 0;
	public MyControls Controller { get; private set; }

	// addaptive trigger data
	private int userId => this.PlayerID + 1;// SCE.SCE_USER_SERVICE_STATIC_USER_ID_1;
	private int dualShotHandler = -1;
	DualshockAdaptive.SCE.ScePadTriggerEffectParam addaptiveTrigger;
	

	// Start is called before the first frame update
	void Awake()
	{
		this.InitDSContoller();
		this.Controller = new MyControls();
		
	}




	// Update is called once per frame
	void Update()
    {
        
    }

	

	private void OnDestroy()
	{
		if (this.dualShotHandler != -1)
		{
			SCE.scePadClose(this.dualShotHandler);
			this.dualShotHandler = -1;
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
