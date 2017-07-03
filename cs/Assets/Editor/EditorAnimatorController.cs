using UnityEditor;
	using UnityEngine;
	using System.Collections;
	using System.IO;

	public class EditorAnimatorController
{
		//controller模板名和拷贝后的名称
		private static string fileName = "AnimatorController.controller";

		[MenuItem("AnimatorController/CreateAnimatorController")]
		static void CreateAnimatorController()
		{
			//获得当前选中为GameObject格式的文件
			object[] objs = Selection.GetFiltered (typeof(GameObject), SelectionMode.Assets);
			GameObject selectObj = null;

			if (objs.Length < 1) {
				Debug.LogError ("Please select a model file");
				return;
			}
			
			//判断是否选中了skin文件，名字含有"@skin"
			foreach (GameObject obj in objs) {
				if (obj.name.Contains ("")) {
				selectObj = obj;
			
				}
			}
			//	//如果没有选中，则提示错误并返回
			if (selectObj == null) {
				Debug.LogError ("Please select a skin file");
				return;
			}


			//获得模板文件的路径（没有找到获得上一级上上级目录的API，只好使用字符切割了）
			string selectObjPath = AssetDatabase.GetAssetPath ((Object)selectObj);



			string[] array = selectObjPath.Split ('/');
			string templetControlPath = null;
			string animatorControllerPath = null;
			for (int i = 0; i < array.Length; ++i) {
				if (i == (array.Length - 1))
					continue;
				
				animatorControllerPath += array [i] + "/";
				
				if (i == (array.Length - 2))
					continue;
				
				templetControlPath += array [i] + "/";
			}
			string fileName01 = array [array.Length - 2] + ".controller";
			//判断模板文件是否在该类型人物文件夹下
			if (!File.Exists (templetControlPath + fileName)) {
			Debug.Log("The templet control file is missing");
				return;
			}
			//如果还没有生成AnimatorController文件，则将模板文件拷贝
			if (!File.Exists (animatorControllerPath + fileName)) {

				//FileUtil.CopyFileOrDirectory (templetControlPath + fileName , animatorControllerPath +fileName  );
				FileUtil.ReplaceFile (templetControlPath + fileName, animatorControllerPath + fileName01);
				Debug.Log ("copy animator control success");
			}
			//注意下方高能坑！之前没有刷新，导致下面的animatorController一直为null
			AssetDatabase.Refresh ();
			
			//通过AssetDatabase类load新拷贝的文件，获得animatorController对象
		UnityEditorInternal.AnimatorController animatorController = AssetDatabase.LoadAssetAtPath (animatorControllerPath + fileName01, typeof(UnityEditorInternal.AnimatorController)) as UnityEditorInternal.AnimatorController;
			//获得layer信息
		   UnityEditorInternal.AnimatorControllerLayer layer = animatorController.GetLayer(0);
			//获得stateMachine信息
		    UnityEditorInternal.StateMachine sm = layer.stateMachine;

		    
			//获得该人物文件夹下Anim文件夹中的动作资源 
			string animPath = animatorControllerPath + "Anim";
			//string[] assets = AssetDatabase.FindAssets( "t:GameObject", animPath.Split() );
			string[] assets = AssetDatabase.FindAssets ("t:AnimationClip", animPath.Split ());
			AnimationClip[] animClip = new AnimationClip[assets.Length];
			//获得目录下所有的AnimationClip对象
			for (int i = 0; i < assets.Length; ++i) {
				string path = AssetDatabase.GUIDToAssetPath (assets [i]);
				animClip [i] = AssetDatabase.LoadAssetAtPath (path, typeof(AnimationClip)) as AnimationClip;
			}

		UnityEditorInternal.BlendTree move = null;

			//通过名称匹配，将AnimationClip一一对应上state的motion
		for (int i = 0; i < sm.stateCount; ++i)
			{
				for (int j = 0; j < animClip.Length; ++j)
			{


		    
				if (sm.GetState(i).uniqueName==("Base Layer.Move"))
				{

					if(animClip [j].name.Contains("Run"))
					{ 
		//			animClip [j].isLooping=true;
//						SerializedObject serializedClip = new SerializedObject(animClip [j]);
//						AnimationClipSettings clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
//						clipSettings.loopTime = true;
//						serializedClip.ApplyModifiedProperties();

						move=sm.GetState(i).CreateBlendTree(layer);
						move.blendParameter="speed";
						move.name="Blend Tree";
						move.automaticThresholds=false;
						AssetDatabase.Refresh ();

						move.AddAnimationClip(animClip [j]);
						LoopTrue(animClip [j]);
						Debug.Log("Move");

					}
					if(animClip [j].name==("Standby"))
					{
					

						move.AddAnimationClip(animClip [j]);
					
						move.SetChildThreshold(0,1f);
						move.SetChildThreshold(1,0f);
						AssetDatabase.Refresh ();
						LoopTrue(animClip [j]);
						Debug.Log("Standby");

					}
				

				}

				if (sm.GetState(i).uniqueName==("Base Layer.attack"))
						{
							if(animClip [j].name.Contains("Skill01"))
							{
						sm.GetState(i).SetAnimationClip(animClip[j]);

						Debug.Log("attack");
							}
							
						}
				if (sm.GetState(i).uniqueName==("Base Layer.skill"))
						{
							if(animClip [j].name.Contains("Skill02"))
							{
						sm.GetState(i).SetAnimationClip(animClip[j]);

				
							}
							
						}

				if (sm.GetState(i).uniqueName== ("Base Layer.bigskill"))
						{
							if(animClip [j].name.Contains("Skill03"))
							{
						sm.GetState(i).SetAnimationClip(animClip[j]);
								
							}
						
					}
				if (sm.GetState(i).uniqueName== ("Base Layer.stun"))
					{
						if(animClip [j].name.Contains("dizziness"))
						{

						sm.GetState(i).SetAnimationClip(animClip[j]);
						LoopTrue(animClip [j]);	
						}
						
					}
				if (sm.GetState(i).uniqueName==("Base Layer.hurt"))
					{
						if(animClip [j].name.Contains("Underattack"))
						{
						sm.GetState(i).SetAnimationClip(animClip[j]);
							
						}
						
					}
				if (sm.GetState(i).uniqueName==("Base Layer.die"))
					{
						if(animClip [j].name.Contains("Died"))
						{
						sm.GetState(i).SetAnimationClip(animClip[j]);
							
						}
						
					}
				if (sm.GetState(i).uniqueName== ("Base Layer.show"))
					{
						if(animClip [j].name.Contains("SpecialStandby01"))
						{
						sm.GetState(i).SetAnimationClip(animClip[j]);
							
						}
						
					}
	


				
				}

			}
		Debug.Log (selectObj.name);
		}



	static void LoopTrue(AnimationClip clip) {
		
//		foreach (object o in Selection.GetFiltered(typeof(AnimationClip),SelectionMode.DeepAssets))
//		{
//			AnimationClip clip =(AnimationClip)o;
			
			SerializedObject serializedClip = new SerializedObject (clip);
			AnimationClipSettings clipSettings = new AnimationClipSettings (serializedClip.FindProperty ("m_AnimationClipSettings"));
			clipSettings.loopTime = true;
			serializedClip.ApplyModifiedProperties ();
			
//		}
	}
	class AnimationClipSettings
	{
		SerializedProperty m_Property;
		
		private SerializedProperty Get (string property) { return m_Property.FindPropertyRelative(property); }
		
		public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }
		
		public float startTime   { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
		public float stopTime	{ get { return Get("m_StopTime").floatValue; }  set { Get("m_StopTime").floatValue = value; } }
		public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
		public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
		public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }
		
		public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
		public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
		public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
		public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
		public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
		public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
		public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
		public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
		public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
		public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
	}
	

	}



	
