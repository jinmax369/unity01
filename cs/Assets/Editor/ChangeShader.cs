using UnityEngine;
using System.Collections;
using UnityEditor;

public class ChangeShader :  EditorWindow {
	
  static TextAsset textAsset;
  static  string [] lineArray;
   [MenuItem ("Window/ChangerShader")]	
    static void AddWindow ()
	{       
		textAsset = Resources.LoadAssetAtPath("Assets/Editor/shader.txt",typeof(TextAsset))as TextAsset;
		
		lineArray = textAsset.text.Split ("&"[0]);
		
		Rect  wr = new Rect (0,0,500,500);
        ChangeShader window = (ChangeShader)EditorWindow.GetWindowWithRect (typeof (ChangeShader),wr,true,"widow name");	
		window.Show();

    }
	
	private Shader shader;
	
	void OnGUI ()
	{
		for(int i =0; i<lineArray.Length; i++)
		{
			string shaderName = lineArray[i].Trim();
			if(GUILayout.Button(shaderName))
			{
				Change(Shader.Find(shaderName));		
			}
		}
	}
	
	void Change(Shader shader)
	{
		if(Selection.activeGameObject != null)
	//	if(Selection.gameObjects != null)
		{
			foreach(GameObject g in Selection.gameObjects)
			{
				Renderer []renders = g.GetComponentsInChildren<Renderer>(g);
				foreach(Renderer r in renders)
				{
					if(r  !=  null)
					{
						foreach(Object o in r.sharedMaterials)
						{
								string path = AssetDatabase.GetAssetPath(o);
								Material m = Resources.LoadAssetAtPath(path,typeof(Material)) as Material;
								m.shader = shader;				
						}
					}
				}
			}	
			this.ShowNotification(new GUIContent("选择的对象批量修改shader成功"));
		}else
		{
			this.ShowNotification(new GUIContent("没有在Hierarchy视图中选择对象"));

		}
	}
	
}
