using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DualshockAdaptive.SCE;

[Serializable]
public class Modifier
{
	public float Timer;
	public Stat Stat;

	public bool isActive => this.Timer > 0.0f;
	public void Update()
	{
		if (this.Timer < 0.0f)
		{
			return;
		}
		this.Timer -= Time.deltaTime;

	}

}
