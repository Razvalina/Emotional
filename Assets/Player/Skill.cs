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
	public float Boost = 1.0f;
	public float RadiusTarget = 0.0f;
	public float MaxRadiusTarget = 1.0f;

	// projectiles only
	public float Speed = 1.0f;


	public float RemainCountDown = 0.0f;


	public void Update()
	{
		if (this.RemainCountDown > 0.0f)
		{
			this.RemainCountDown -= Time.deltaTime;
		}
		else
		{
			this.RemainCountDown = 0.0f;
		}
	}

	public float CalcPower()
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


	public float Use()
	{
		float ret = -1.0f;

		if (this.RemainCountDown > 0.0f)
		{
			// blocked!
			return ret;
		}
		this.RemainCountDown = this.CountDown;

		ret = this.CalcPower();

		return ret;
	}



}
