using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DualshockAdaptive.SCE;

[Serializable]
public class Stat
{
	public float HP;
	public float Speed; //move koef
	public float Power; // attach power
	public float DirectionKoef;

	public void Copy(Stat st)
	{
		this.HP = st.HP;
		this.Speed = st.Speed;
		this.Power = st.Power;
		this.DirectionKoef = st.DirectionKoef;
	}

}
