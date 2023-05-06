using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static DualshockAdaptive.SCE;

public interface PlayerActionDelegate
{
	void onDie(Player pl);
	void onFire(Player pl, Skill skill);
	void onUlt(Player pl, Skill skill);
}