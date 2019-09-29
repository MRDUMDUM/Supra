// Copyright (c) 2014 Make Code Now! LLC

#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
#define UNITY_4
#endif

using UnityEngine;
using System.Collections;

[AddComponentMenu("SECTR/Demos/SECTR Complete Demo UI")]
public class SECTR_CompleteDemoUI : SECTR_DemoUI
{
	#region Private Details
	private string originalDemoMessage;
	#endregion
	
	#region Public Interface
	[Multiline] public string Unity4PerfMessage;
	#endregion

	#region Unity Interface
	void Start()
	{
		if(PipController && PipController.GetComponent<SECTR_CullingCamera>() == null &&
		   GetComponent<SECTR_CullingCamera>() && GetComponent<Camera>())
		{
			SECTR_CullingCamera proxyCamera = PipController.gameObject.AddComponent<SECTR_CullingCamera>();
			proxyCamera.cullingProxy = GetComponent<Camera>();
		}
	}

	protected override void OnEnable()
	{
		originalDemoMessage = DemoMessage;

		base.OnEnable();
		SECTR_StartLoader startLoader = GetComponent<SECTR_StartLoader>();
		if(startLoader)
		{
			startLoader.Paused = true;
		}
	}

	protected override void OnGUI()
	{
		if(Application.isEditor && Application.isPlaying && !string.IsNullOrEmpty(Unity4PerfMessage))
		{
			DemoMessage = originalDemoMessage;
			DemoMessage += "\n\n";
			DemoMessage += Unity4PerfMessage;
		}

		base.OnGUI();
		
		SECTR_StartLoader startLoader = GetComponent<SECTR_StartLoader>();
		if(passedIntro && startLoader && startLoader.Paused)
		{
			startLoader.Paused = false;
		}
	}
	#endregion
}
