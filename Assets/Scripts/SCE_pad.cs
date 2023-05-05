using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace DualshockAdaptive
{
	public static class SCE
	{
		// PAD 
		public const uint SCE_PAD_ERROR_ALREADY_OPENED = 0x80920004;
		public const uint SCE_PAD_ERROR_NO_HANDLE = 0x80920008;

		


		public const Byte SCE_USER_SERVICE_STATIC_USER_ID_1 = 1;

		public const Byte SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_L2 = 0x01;
		public const Byte SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_R2 = 0x02;

		public const Byte SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2 = 0;
		public const Byte SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2 = 1;

		public const Byte SCE_PAD_TRIGGER_EFFECT_TRIGGER_NUM = 2;

		/* Definition of control point num */
		public const Byte SCE_PAD_TRIGGER_EFFECT_CONTROL_POINT_NUM = 10;

		public enum ScePadTriggerEffectMode
		{
			SCE_PAD_TRIGGER_EFFECT_MODE_OFF,
			SCE_PAD_TRIGGER_EFFECT_MODE_FEEDBACK,
			SCE_PAD_TRIGGER_EFFECT_MODE_WEAPON,
			SCE_PAD_TRIGGER_EFFECT_MODE_VIBRATION,
			SCE_PAD_TRIGGER_EFFECT_MODE_MULTIPLE_POSITION_FEEDBACK,
			SCE_PAD_TRIGGER_EFFECT_MODE_SLOPE_FEEDBACK,
			SCE_PAD_TRIGGER_EFFECT_MODE_MULTIPLE_POSITION_VIBRATION,
		};

		/**
		 *E  
		 *  @brief parameter for setting the trigger effect to off mode.
		 *         Off Mode: Stop trigger effect.
		 **/


		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectOffParam
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
			public Byte[] _data;
		};


		/**
		 *E  
		 *  @brief parameter for setting the trigger effect to Feedback mode.
		 *         Feedback Mode: The motor arm pushes back trigger.
		 *                        Trigger obtains stiffness at specified position.
		 **/
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectFeedbackParam
		{
			public Byte position;   /*E position where the strength of target trigger start changing(0~9). */
			public Byte strength;   /*E strength that the motor arm pushes back target trigger(0~8 (0: Same as Off mode)). */

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 46)]
			public Byte[] padding;
		};

		/**
		 *E  
		 *  @brief parameter for setting the trigger effect to Weapon mode.
		 *         Weapon Mode: Emulate weapon like gun trigger.
		 **/
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectWeaponParam
		{
			public Byte startPosition;  /*E position where the stiffness of triger start changing(2~7). */
			public Byte endPosition;    /*E position where the stiffness of trigger finish changing(startPosition+1~8). */
			public Byte strength;       /*E strength of gun trigger(0~8 (0: Same as Off mode)). */

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
			public Byte[] padding;
		};

		/**
		 *E  
		 *  @brief parameter for setting the trigger effect to Vibration mode.
		 *         Vibration Mode: Vibrates motor arm around specified position.
		 **/
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectVibrationParam
		{
			public Byte position;   /*E position where the motor arm start vibrating(0~9). */
			public Byte amplitude;  /*E vibration amplitude(0~8 (0: Same as Off mode)). */
			public Byte frequency;  /*E vibration frequency(0~255[Hz] (0: Same as Off mode)). */

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
			public Byte[] padding;
		};

		/**
		 *E  
		 *  @brief parameter for setting the trigger effect to ScePadTriggerEffectMultiplePositionFeedbackParam mode.
		 *         Multi Position Feedback Mode: The motor arm pushes back trigger.
		 *                                       Trigger obtains specified stiffness at each control point.
		 **/ 
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectMultiplePositionFeedbackParam
		{
			/*E strength that the motor arm pushes back target trigger at position(0~8 (0: Same as Off mode)).
																*  strength[0] means strength of motor arm at position0.
																*  strength[1] means strength of motor arm at position1.
																*  ...
																* */
			public Byte strength0;
			public Byte strength1;
			public Byte strength2;
			public Byte strength3;
			public Byte strength4;
			public Byte strength5;
			public Byte strength6;
			public Byte strength7;
			public Byte strength8;
			public Byte strength9;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 38)]
			public Byte[] padding;
		};

		/**
		 *E  
		 *  @brief parameter for setting the trigger effect to Feedback3 mode.
		 *         Slope Feedback Mode: The motor arm pushes back trigger between two spedified control points.
		 *                              Stiffness of the trigger is changing depending on the set place.
		 **/
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectSlopeFeedbackParam
		{

			public Byte startPosition;  /*E position where the strength of target trigger start changing(0~endPosition). */
			public Byte endPosition;    /*E position where the strength of target trigger finish changing(startPosition+1~9). */
			public Byte startStrength;  /*E strength when trigger's position is startPosition(1~8) */
			public Byte endStrength;    /*E strength when trigger's position is endPosition(1~8) */

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
			public Byte[] padding;
		};

		/**
		 *E  
		 *  @brief parameter for setting the trigger effect to Vibration2 mode.
		 *         Multi Position Vibration Mode: Vibrates motor arm around specified control point.
		 *                                        Trigger vibrates specified amplitude at each control point.
		 **/
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectMultiplePositionVibrationParam
		{
			public Byte frequency;                                              /*E vibration frequency(0~255 (0: Same as Off mode)) */

				/* E vibration amplitude at position(0~8 (0: Same as Off mode))
				*  amplitude[0] means amplitude of vibration at position0.
				*  amplitude[1] means amplitude of vibration at position1.
				*  ...
				* */
			public Byte amplitude0;
			public Byte amplitude1;
			public Byte amplitude2;
			public Byte amplitude3;
			public Byte amplitude4;
			public Byte amplitude5;
			public Byte amplitude6;
			public Byte amplitude7;
			public Byte amplitude8;
			public Byte amplitude9;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
			public Byte[] padding;
		};

		/**
		 *E  
		 *  @brief parameter for setting the trigger effect mode.
		 **/
		[System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
		public class ScePadTriggerEffectCommandData
		{
			[System.Runtime.InteropServices.FieldOffset(0)]
			public ScePadTriggerEffectOffParam offParam;

			[System.Runtime.InteropServices.FieldOffset(0)]
			public ScePadTriggerEffectFeedbackParam feedbackParam;

			[System.Runtime.InteropServices.FieldOffset(0)]
			public ScePadTriggerEffectWeaponParam weaponParam;

			[System.Runtime.InteropServices.FieldOffset(0)]
			public ScePadTriggerEffectVibrationParam vibrationParam;

			[System.Runtime.InteropServices.FieldOffset(0)]
			public ScePadTriggerEffectMultiplePositionFeedbackParam multiplePositionFeedbackParam;

			[System.Runtime.InteropServices.FieldOffset(0)]
			public ScePadTriggerEffectSlopeFeedbackParam slopeFeedbackParam;

			[System.Runtime.InteropServices.FieldOffset(0)]
			public ScePadTriggerEffectMultiplePositionVibrationParam multiplePositionVibrationParam;

			public ScePadTriggerEffectCommandData()
			{
				this.offParam = new ScePadTriggerEffectOffParam();// all zeros
				this.offParam._data = new byte[48];
			}
		};


		/**
		 *E  
		 *  @brief parameter for setting the trigger effect.
		 **/
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectCommand
		{
			public int mode;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public Byte[] padding;

			public ScePadTriggerEffectCommandData commandData;
		};



		/**
		 *E  
		 *  @brief parameter for the scePadSetTriggerEffect function.
		 **/
		[StructLayout(LayoutKind.Sequential)]
		public struct ScePadTriggerEffectParam
		{

			public Byte triggerMask;        /*E Set trigger mask to activate trigger effect commands.
									*  SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_L2 : 0x01
									*  SCE_PAD_TRIGGER_EFFECT_TRIGGER_MASK_R2 : 0x02
									* */
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
			public Byte[] padding;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = SCE_PAD_TRIGGER_EFFECT_TRIGGER_NUM)]
			public ScePadTriggerEffectCommand[] command;                            /*E command[SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_L2] is for L2 trigger setting
																			 *  and param[SCE_PAD_TRIGGER_EFFECT_PARAM_INDEX_FOR_R2] is for R2 trgger setting.
																			 * */
		};

		[DllImport("libScePad.dll")]
		public static extern int scePadInit();

		[DllImport("libScePad.dll")]
		public static extern int scePadClose(int handle);

		[DllImport("libScePad.dll")]
		public static extern int scePadOpen(int userId, int type, int index, IntPtr pParam);

		[DllImport("libScePad.dll")]
		public static extern int scePadGetHandle(int userId, int type, int index);

		[DllImport("libScePad.dll")]
		public static extern int scePadSetTriggerEffect(int handle, ref ScePadTriggerEffectParam pParam);
	};

}
