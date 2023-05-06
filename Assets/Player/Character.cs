using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static DualshockAdaptive.SCE;


[Serializable]
public class Character
{
	public Stat DefaultState;
	public List<Modifier> Modifiers;

	public Stat Current = new Stat();

	public bool isDead => this.Current?.HP <= 0;

	public void Update()
	{
		// dead should not update itself
		if (this.Current.HP < 0)
			return;
		
		this.Current.Copy(this.DefaultState);

		foreach (Modifier modifier in this.Modifiers)
		{
			modifier.Update();
			if (!modifier.isActive)
			{
				this.Modifiers.Remove(modifier);
				continue;
			}

			// apply modifier
			this.Current.HP += modifier.Stat.HP;
			this.Current.Speed *= modifier.Stat.Speed;
			this.Current.Power += modifier.Stat.Power;
			this.Current.DirectionKoef *= modifier.Stat.DirectionKoef;
		}
	}
}
