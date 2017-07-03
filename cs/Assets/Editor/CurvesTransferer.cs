﻿using UnityEditor;
using UnityEngine;
using System.IO;

public class CurvesTransferer
{
    [MenuItem("Character Generator/Transfer Clip Curves to Copy")]
    static void CopyClip()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is GameObject)) continue;
            if (!o.name.Contains("@")) continue;
            GameObject animationFBX = (GameObject)o;
           
                      AnimationClip srcClip = animationFBX.animation.clip;
           AnimationClip newClip = new AnimationClip();
            newClip.name = srcClip.name;

            // Create directory to store generated materials.
            if (!Directory.Exists(AnimationsPath(animationFBX)))
                Directory.CreateDirectory(AnimationsPath(animationFBX));

            string animationPath = AnimationsPath(animationFBX) + newClip.name + ".anim";

            AssetDatabase.CreateAsset(newClip, animationPath);
            AssetDatabase.Refresh();

            AnimationClipCurveData[] curveDatas = AnimationUtility.GetAllCurves(srcClip, true);
            for (int i = 0; i < curveDatas.Length; i++)
            {
                AnimationUtility.SetEditorCurve(newClip, curveDatas[i].path, curveDatas[i].type, curveDatas[i].propertyName, curveDatas[i].curve);
            }
        }
    }

    // Returns the path to the directory that holds the specified FBX.
    static string CharacterRoot(GameObject character)
    {
        string root = AssetDatabase.GetAssetPath(character);
        return root.Substring(0, root.LastIndexOf('/') + 1);
    }

    // Returns the path to the directory that holds materials generated
    // for the specified FBX.
    public static string AnimationsPath(GameObject character)
    {
        return CharacterRoot(character) + "Copy Animations/";
    }
}