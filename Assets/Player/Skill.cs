using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static DualshockAdaptive.SCE;

[Serializable]
public class Skill
{
	public Modifier Modifier;
	public GameObject Visual;
	public float CountDown;
	public float LifeTime;
	public float Power = 1.0f;

	public float CurrentPower => CalcPower();

	private float CalcPower()
	{
		float res = this.Power;

		if (this.Modifier != null)
		{
			this.Modifier.Update();
			res += this.Modifier.Stat.Power;
			if (!this.Modifier.isActive)
			{
				this.Modifier = null;
			}
		}

		return res;
	}



}
