using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Object = UnityEngine.Object;

/*
    批量修改美术资源

    功能： 1.批量修改已导入的角色、场景、特效资源属性（手动修改）
          2.批量修改新增资源角色、场景、特效资源属性（自动修改）  

    使用：批量选中已经导入且需要修改的模型或贴图使用快捷键或者菜单栏ArtBatch即可

 *  快捷键：
    Alt+R ---->修改角色模型
    Alt+S ---->修改场景模型
    Alt+F ---->修改特效贴图
 */
public class ArtBatch : AssetPostprocessor
{

	public static  string  [] config =
	{
		".PNG",".JPG",".TGA"
	};
 


    //角色模型手动设置
    [MenuItem("ArtBatch/Model_Role &R")]
    static void ModelRole()
    {
        Debug.Log("ModelRole");
        //循环查找 Object 深度资源模式
		foreach (Object o in Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets))
        {
	

            //非对象不继续
			if (!(o is GameObject))
                continue;
            //将o作为模型存储在mod中
            GameObject mod = o as GameObject;
            //将mod模型路径存储在path中
            string path = AssetDatabase.GetAssetPath(mod);
            //将路径中的模型资源导入
            ModelImporter modelimporter = ModelImporter.GetAtPath(path) as ModelImporter;


            //模型导入后 贴图格式设置细节
            modelimporter.globalScale = 0.05f;
         //   modelimporter.meshCompression = ModelImporterMeshCompression.Off;
           modelimporter.animationType = ModelImporterAnimationType.Generic;
          //  modelimporter.isReadable = false;

          //  modelimporter.optimizeMesh = true;
          //  modelimporter.optimizeGameObjects = true;

            Debug.Log("角色-重置值OK");
            AssetDatabase.ImportAsset(path);
        }
        AssetDatabase.Refresh();
    }
	

	static	string path01;
	[MenuItem("ArtBatch/Model_Material &M")]
	static void MaterialRole()
	{
//		if (Selection.activeGameObject  != null) {
//
//				foreach(GameObject g in Selection.gameObjects)
//			{
//				Renderer [] renders = g.GetComponentsInChildren<Renderer> ();
//				foreach (Renderer r in renders) {
//					if (r != null) {
//						foreach (Object o in r.sharedMaterials) {
//							string path = AssetDatabase.GetAssetPath (o);
//						
//							path01=path;
//							Material m = Resources.LoadAssetAtPath (path, typeof(Material)) as Material;
//							m.shader=Shader.Find("LB/HeroRim");	
//
//							if (m.mainTexture == null) {
//								Texture t = GetTexture (m.name);
//								if (t != null) {
//									m.mainTexture = t;
//								} else {
//									Debug.Log ("材质名:" + o.name + " 材质替换失败，请检查资源");
//								}
//							}
//		
//						}
//					}
//				}
//			}	
//				
//		}

		foreach (Object o in Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets))
		{
			if (!(o is Material ))
				continue;
			Material mat=o as Material;
			string matpath = AssetDatabase.GetAssetPath(mat);
			Debug.Log (matpath);

			Material m = Resources.LoadAssetAtPath(matpath ,typeof(Material)) as Material;
			m.shader=Shader.Find("LB/HeroRim");
			Texture t=GetTexture(m.name,matpath);
			m.mainTexture=t;
		}
		AssetDatabase.Refresh();

	}
	//-------------------------------------------------------------------------------------------//




//		foreach (Object p in Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets))
//		{
//			if (!(p is Texture))
//				continue;
//			Texture tex=p as  Texture;
//			string texpath = AssetDatabase.GetAssetPath(tex);
//			Debug.Log (texpath);
//			Texture t = Resources.LoadAssetAtPath(texpath,typeof(Texture)) as Texture;
//			m.mainTexture=t;
//		}
		
		

	
	
	static Texture GetTexture(string name,string path02)
	{
		foreach(string suffix in config)
		{

		//	Texture t = Resources.LoadAssetAtPath("Assets/Textures/" + name+suffix,typeof(Texture)) as Texture;
	//		string path=path01.Split('/')[0]+"/"+path01.Split('/')[1]+"/"+path01.Split('/')[2]+"/"+ name+suffix;
	
			string[] stringSeparators = new string[] {"Materials"}; 
			string[] result;
			result = path02.Split(stringSeparators,System.StringSplitOptions.RemoveEmptyEntries);


			string path=result[0]+ name+suffix;
			Debug.Log (path);
			Texture t = Resources.LoadAssetAtPath(path,typeof(Texture)) as Texture;
			if(t != null)
				return t;
		}
		return null;						
	}
	

	//场景模型后置设置
    [MenuItem("ArtBatch/Model_Scene &S")]
    static void ModelScene()
    {
        Debug.Log("ModelScene");
        //循环查找 Object 深度资源模式
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            //非模型不继续
            if (!(o is GameObject))
                continue;
            //将o作为模型存储在Smod中
            GameObject Smod = (GameObject)o;
            //将Smod模型路径存储在path中
            string path = AssetDatabase.GetAssetPath(Smod);
            //将路径中的模型资源导入
            ModelImporter modelimporter = (ModelImporter)ModelImporter.GetAtPath(path);

            //模型设置细节↓
            modelimporter.globalScale = 1.0f;
            modelimporter.meshCompression = ModelImporterMeshCompression.Off;
            modelimporter.isReadable = false;
            modelimporter.animationType = ModelImporterAnimationType.None;
            Debug.Log("场景-重置值OK");
            AssetDatabase.ImportAsset(path);
        }
        AssetDatabase.Refresh();
    }

    //特效贴图后置设置
    [MenuItem("ArtBatch/Texture_Fx &A")]
    static void Fx_Texture()
    {
        //循环查找 Object类型 深度资源模式
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            //非贴图不继续
            if (!(o is Texture))
                continue;
            //将o作为贴图存储在tex中
            Texture tex = o as Texture;
            //将tex的贴图路径存储在path中
            string path = AssetDatabase.GetAssetPath(tex);
            //将路径中的贴图资源导入
            Debug.Log(path);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            //贴图格式设置细节↓
            textureImporter.textureType = TextureImporterType.Advanced;
            textureImporter.anisoLevel = 0;
            Debug.Log("贴图-重置值OK");
            AssetDatabase.ImportAsset(path);
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("ArtBatch/增加节点 &V")]
    static void AddEemty()
    {
        ////  循环查找 Object类型 深度资源模式
        //foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        //{
        //    //非贴图不继续
        //    if (!(o is Texture))
        //        continue;
        //    //将o作为贴图存储在tex中
        //    Texture tex = o as Texture;
        //    //将tex的贴图路径存储在path中
        //    string path = AssetDatabase.GetAssetPath(tex);
        //    //将路径中的贴图资源导入
        //    Debug.Log(path);
        //    TextureImporter textureimporter = AssetImporter.GetAtPath(path) as TextureImporter;
        //    //贴图格式设置细节↓
        //    textureimporter.textureType = TextureImporterType.Advanced;
        //    textureimporter.anisoLevel = 0;
        //    Debug.Log("贴图-重置值OK");
        //    AssetDatabase.ImportAsset(path);
        //}
        //AssetDatabase.Refresh();
        if (Selection.activeGameObject !=null)
        {
            GameObject g = Selection.activeGameObject;
            Renderer[] renders = g.GetComponentsInChildren<Renderer>(g);

            foreach (Renderer r in renders)
            {
                if (r != null)
                {
                    foreach (Object o in r.sharedMaterials)
                    {
                        string path = AssetDatabase.GetAssetPath(o);

       //         string animat=path.Split(new string[] { "Materials" } , StringSplitOptions.None)[0];

                    }
                }
            }
            if (renders != null)
            {
                renders[0].name = "body";
            }

            Animator animator = g.GetComponent<Animator>();


            if (GameObject.Find("life")==null)
            {
                GameObject eemty = new GameObject("life");
                eemty.transform.position = new Vector3(0, 1.5f, 0);
                GameObject eemty01 = GameObject.Find("Bip001 Head");
                eemty.transform.parent = eemty01.transform;
            }






        }


    }

    //新增导入资源自动设置↓
    //Model-tex预设设置↓
    void OnPreprocessModel()
    {
        ModelImporter modelimporter = (ModelImporter)assetImporter;
        //Role模型预设值↓
        if (assetImporter.assetPath.Contains("/CharacterSource/"))
        {
            // 判定路径可随意修改 --->"/***/"
            modelimporter.globalScale = 1.0f;
            modelimporter.meshCompression = ModelImporterMeshCompression.Off;
            modelimporter.isReadable = false;
            modelimporter.animationType = ModelImporterAnimationType.Generic;
            modelimporter.optimizeGameObjects = true;
            Debug.Log("角色-预设值OK");
        }
        else if (assetImporter.assetPath.Contains("/SceneSource/"))
        {
            //Scene模型预设值↓
            modelimporter.globalScale = 1.0f;
            modelimporter.meshCompression = ModelImporterMeshCompression.Off;
            modelimporter.isReadable = false;
            modelimporter.animationType = ModelImporterAnimationType.None;
            Debug.Log("场景-预设值OK");
        }
    }
    //特效贴图预设值↓
    void OnPreprocessTexture()
    {
        if (assetPath.Contains("/GFXSource/"))
        {
            TextureImporter textureimport = (TextureImporter)assetImporter;
           textureimport.textureType = TextureImporterType.Sprite;
          //  textureimport.anisoLevel = 0;
	//		textureimport.generateCubemap=true;
			textureimport.mipmapEnabled=false;
			textureimport.maxTextureSize=256;
            Debug.Log("贴图-预设值OK");
        }
    }
}