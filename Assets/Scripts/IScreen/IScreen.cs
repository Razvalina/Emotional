using DualshockAdaptive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DualshockAdaptive.SCE;

public interface IScreen
{
	void ToShow(StateController st);
	void ToHide();
	string getName();
}
