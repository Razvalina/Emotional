using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

using DualshockAdaptive;
using static DualshockAdaptive.SCE;
using UnityEditorInternal;

public class PlayerOld: MonoBehaviour
{
	public int PlayerID = 0;

	private static bool isInitedPS5 = false;

	private int userId = SCE.SCE_USER_SERVICE_STATIC_USER_ID_1;
	private int dualShotHandler = -1;
	DualshockAdaptive.SCE.ScePadTriggerEffectParam addaptiveTrigger;

	// Start is called before the first frame update
	void Start()
	{
		if (!isInitedPS5)
		{
			isInitedPS5 = true;
		}

		int ret = SCE.scePadInit();
		if (ret != 0)
		{
			Debug.Log("Can't init libScePad");
			return;
		}

		this.dualShotHandler = SCE.scePadOpen(userId, 0, this.PlayerID, IntPtr.Zero);
		if ((uint)this.dualShotHandler == SCE.SCE_PAD_ERROR_ALREADY_OPENED)
		{
			// need to get control
			this.dualShotHandler = SCE.scePadGetHandle(userId, 0, this.PlayerID);
			if ((uint)this.dualShotHandler == SCE.SCE_PAD_ERROR_NO_HANDLE)
			{
				Debug.Log("Controller is inited somewhere, but tool couldn't attach to it. \nDo something with contoller and restart tool");
				return;
			}
		}


		this.addaptiveTrigger = new SCE.ScePadTriggerEffectParam();
		this.addaptiveTrigger.triggerMask = SCE.SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_L2 | SCE.SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_R2;
		this.addaptiveTrigger.command = new SCE.ScePadTriggerEffectCommand[SCE.SCE_PAD_TRIGGER_EFFECT_TRIGGER_NUM];
		this.addaptiveTrigger.padding = new byte[7];


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


		ret = SCE.scePadSetTriggerEffect(this.dualShotHandler, ref this.addaptiveTrigger);
		if (ret != 0)
		{
			Debug.Log("can't assign triggerEffect to Left");
		}

	}
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	private void OnDestroy()
	{
		if (this.dualShotHandler != -1)
		{
			SCE.scePadClose(this.dualShotHandler);
			this.dualShotHandler = -1;
		}
	}


	//class FireLife
	//{
	//	public GameObject Go { get; set; }
	//	public float LifreTrime { get; set; }
	//	public float PauseTime { get; set; } = 0.3f;

	//	public int State { get; set; } = 0;

	//	public void Update(float dt)
	//	{
	//		if (State == 0)
	//		{
	//			PauseTime -= dt;

	//		}
	//		if (State == 1)
	//		{
	//			this.LifreTrime -= dt;
	//		}
	//	}
	//}

	//class FinishLife
	//{
	//	public GameObject VFX { get; set; }

	//	public float fTimeoutAnimation { get; set; }
	//	public float fTotalAwait { get; set; }

	//	public bool isPosiAnim { get; set; }
	//}

	//public GameObject FireVFX;
	//public GameObject ProgressVFX;
	//public GameObject WinObject;
	//public float ProgressSpawn = 0.5f;
	//public int EnemiesToKill = 30;
	//public float LoveRadius = 5.0f;
	//public int TakeDamage = 2;
	//public int TakeDamagemax = 5;

	//public GameObject Enemies;
	//public List<AudioClip> AttackSounds;



	//List<FireLife> fireLives = new List<FireLife>();
	//uint WalkAnimId = 1;
	//public float CoefMove = 0.001f;
	//float fCurrentAngle = 0.0f;
	//Animator animator;
	//AudioSource audioSource;
	//FinishLife finishLife = null;

	//MyControls contoller;
	//int userId = SCE.SCE_USER_SERVICE_STATIC_USER_ID_1;
	//int dualShotHandler = -1;
	//DualshockAdaptive.SCE.ScePadTriggerEffectParam addaptive;

	//bool isShoot = false;
	//float fTriggerValue = 0.0f;

	//public bool isWin()
	//{
	//	return this.EnemiesToKill <= 0;
	//}

	//public void setAngle(float fAng)
	//{
	//	this.fCurrentAngle = fAng;

	//	uint value = this.getIndexFromAngle(this.fCurrentAngle);
	//	if (value != this.WalkAnimId)
	//	{
	//		this.animator.Play("Walk_" + this.getIndexFromAngle(this.fCurrentAngle).ToString(), -1);
	//		this.WalkAnimId = value;
	//	}
	//}

	//public void SetDirection(Vector2 dir)
	//{
	//	this.transform.position += new Vector3(dir.x, dir.y) * CoefMove;

	//	Vector3 camPos = Camera.main.transform.position;
	//	camPos.x = this.transform.position.x;
	//	camPos.y = this.transform.position.y;
	//	Camera.main.transform.position = camPos;


	//}

	//// Start is called before the first frame update
	//void Start()
	//{
	//	this.animator = GetComponent<Animator>();
	//	this.audioSource = GetComponent<AudioSource>();

	//	if (contoller == null)
	//	{


	//		contoller = new MyControls();
	//		//contoller.Player.SetCallbacks(this);
	//	}
	//	contoller.Player.Enable();
	//	contoller.Player.Fire.started += context => testFire();
	//	contoller.Player.Fire.canceled += context => testFireOff();
	//	contoller.Player.FireShoulder.performed += context => this.fTriggerValue = context.ReadValue<float>();
	//	contoller.Player.FireShoulder.canceled += context => this.fTriggerValue = 0.0f;






	//}

	//private void testFire()
	//{
	//	this.TriggerFire();
	//}
	//private void testFireOff()
	//{
	//	//this.TriggerFire();
	//}


	//// Update is called once per frame
	//void Update()
	//{
	//	foreach (FireLife fireLife in fireLives)
	//	{
	//		fireLife.Update(Time.deltaTime);
	//		if (fireLife.State == 0 && fireLife.PauseTime < 0.0f)
	//		{
	//			fireLife.State = 1;
	//			GameObject go = Instantiate(this.FireVFX, this.transform.position, Quaternion.identity);
	//			fireLife.Go = go;

	//			// play attack
	//			int value = Random.Range(0, this.AttackSounds.Count);
	//			this.audioSource.clip = this.AttackSounds[value];
	//			this.audioSource.Play();

	//			this.BoundEnemies();
	//		}

	//		if (fireLife.LifreTrime < 0.0f)
	//		{
	//			Destroy(fireLife.Go);
	//		}
	//	}

	//	if (this.finishLife != null)
	//	{
	//		finishLife.fTimeoutAnimation -= Time.deltaTime;
	//		if (finishLife.fTimeoutAnimation < 0.0f)
	//		{
	//			// launch move animation
	//			//this.finishLife.VFX.GetComponent<Animator>().Play("Butter_fly_away", -1);
	//			finishLife.fTimeoutAnimation = 999989.0f;
	//			finishLife.isPosiAnim = true;
	//		}
	//		if (finishLife.isPosiAnim)
	//		{
	//			Vector3 pos = this.finishLife.VFX.transform.position;
	//			pos.y += (10.0f * Time.deltaTime);
	//			this.finishLife.VFX.transform.position = pos;
	//		}
	//		finishLife.fTotalAwait -= Time.deltaTime;
	//		if (finishLife.fTotalAwait < 0.0f)
	//		{
	//			// move to titles here
	//			SceneManager.LoadScene("Titles");
	//		}
	//	}


	//	if(!this.isShoot && this.fTriggerValue > 0.85f)
	//	{
	//		this.isShoot = true;
	//		this.TriggerFire();
	//	}
	//	if (this.isShoot && this.fTriggerValue < 0.2f)
	//	{
	//		this.isShoot = false;
	//	}

	//	var gamepad = Gamepad.current;
	//	if (gamepad == null)
	//		return; // No gamepad connected.



	//	if (gamepad.rightTrigger.wasPressedThisFrame)
	//	{
	//		// 'Use' code here
	//	}

	//	Vector2 move = gamepad.leftStick.ReadValue();
	//	float Radius = move.magnitude;
	//	if (Radius < 0.2f)
	//	{
	//		// deadzone
	//		Direction = Vector2.zero;
	//		return;
	//	}
	//	Radius = Math.Clamp(Radius, 0.0f, 0.4f);// hardco
	//	Direction += move * Radius * 0.25f;
	//	Debug.Log(move.x);
	//}

	//private void BoundEnemies()
	//{
	//	uint uiKilledEnemies = 0;

	//	// iterate over groups
	//	uint maxGroup = 16;
	//	for (uint i = 1; i < maxGroup; ++i)
	//	{
	//		GameObject goGroup = null;
	//		try
	//		{
	//			goGroup = this.Enemies.transform.Find("Group_" + i.ToString()).gameObject;
	//		}
	//		catch (Exception ex)
	//		{
	//			//Debug.Log(ex.Message);
	//		}
	//		if (goGroup != null)
	//		{
	//			// testing 
	//			for (uint j = 1; j < 5; ++j)
	//			{
	//				GameObject goEnemy = null;
	//				try
	//				{
	//					goEnemy = goGroup.transform.Find("enemy_" + j.ToString()).gameObject;
	//				}
	//				catch (Exception ex)
	//				{
	//					//Debug.Log(ex.Message);
	//				}
	//				if (goEnemy != null)
	//				{
	//					// try access by rad
	//					Vector2 my = this.transform.position;
	//					Vector2 enemy_pos = goEnemy.transform.position;
	//					Vector2 diff = my - enemy_pos;
	//					float fLenth = diff.magnitude;
	//					//Debug.Log(fLenth);
	//					if (fLenth > this.LoveRadius)
	//					{
	//						// too far
	//						continue;
	//					}

	//					// attack them!
	//					Enemy en = goEnemy.GetComponent<Enemy>();
	//					if (en != null && !en.isDead())
	//					{
	//						int value = Random.Range(this.TakeDamage, this.TakeDamagemax);
	//						en.TakeDamage(value);

	//						if (en.isDead())
	//						{
	//							// YEY! 
	//							// killed
	//							uiKilledEnemies++;
	//						}
	//					}
	//				}
	//			}

	//		}
	//	}


	//	for (uint i = 0; i < uiKilledEnemies; ++i)
	//	{
	//		for (int h = 0; h < 4; ++h)
	//		{


	//			GameObject go = Instantiate(this.ProgressVFX, this.transform, true);
	//			Vector3 vfx_pox = go.transform.position;
	//			vfx_pox.x += Random.Range(-this.ProgressSpawn, this.ProgressSpawn);
	//			vfx_pox.y += Random.Range(-this.ProgressSpawn, this.ProgressSpawn);


	//			go.transform.position = vfx_pox;
	//			float fScaleRand = Random.Range(1.2f, 1.7f);
	//			go.transform.localScale = new Vector3(fScaleRand, fScaleRand, fScaleRand);
	//		}

	//			EnemiesToKill--;
	//		if (EnemiesToKill == 0)
	//		{
	//			// final
	//			this.Finish();
	//			break;
	//		}


	//	}
	//}


	//private void Finish()
	//{
	//	this.animator.Play("FinalAnim");

	//	this.finishLife = new FinishLife();
	//	this.finishLife.VFX = Instantiate(this.WinObject, this.transform.position, Quaternion.identity);
	//	this.finishLife.fTotalAwait = 7.0f;
	//	this.finishLife.fTimeoutAnimation = 4.0f;
	//}

	//internal void TriggerFire()
	//{
	//	this.animator.Play("Attack_" + this.WalkAnimId.ToString());

	//	FireLife fl = new FireLife();
	//	fl.LifreTrime = 1.0f;
	//	fireLives.Add(fl);
	//}


	//private uint getIndexFromAngle(float fAngle)
	//{
	//	if (fAngle > 0.0f)
	//	{
	//		// top
	//		// [  0 - 22.5 ] = 7
	//		// [ 22.5 - 67.5 ] = 6
	//		// [ 67.5 - 112.5 ] = 5
	//		// [ 112.5 - 157.5 ] = 4
	//		// [ 157.5 - 180 ] = 3

	//		float fStart = 0.0f;
	//		float fTop = 22.5f;
	//		for (uint i = 7; i >= 3; i--)
	//		{
	//			if (fAngle >= fStart && fAngle <= fTop)
	//			{
	//				return i;
	//			}
	//			fStart = fTop;
	//			fTop += 45.0f;
	//		}
	//	}
	//	else
	//	{
	//		fAngle = -fAngle;
	//		uint[] inds = { 7, 8, 1, 2, 3 };
	//		// bottom
	//		// [  0 - 22.5 ] = 7
	//		// [ 22.5 - 67.5 ] = 8
	//		// [ 67.5 - 112.5 ] = 1
	//		// [ 112.5 - 157.5 ] = 2
	//		// [ 157.5 - 180 ] = 3

	//		float fStart = 0.0f;
	//		float fTop = 22.5f;
	//		for (int i = 0; i < 5; i++)
	//		{
	//			if (fAngle >= fStart && fAngle <= fTop)
	//			{
	//				return inds[i];
	//			}
	//			fStart = fTop;
	//			fTop += 45.0f;
	//		}
	//	}
	//	return 1;
	//}
	//private void OnCollisionEnter2D(Collision2D collion)
	//{

	//}


}
