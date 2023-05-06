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
	
}
