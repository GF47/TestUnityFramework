/***************************************************************
 * @File Name       : ResetAndroidShaderInEditor
 * @Author          : GF47
 * @Description     : TODO what's the use of the [ResetAndroidShaderInEditor]
 * @Date            : 2017/8/17/星期四 19:13:45
 * @Edit            : none
 **************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetAndroidShaderInEditor
{
    [MenuItem("Tools/GF47 Editor/重置Android的shader")]
    static void Reset()
    {
        Transform[] objs = Selection.transforms;
        foreach (var transform in objs)
        {
            ResetShader(transform);
        }
    }

    private static void ResetShader(Transform trans)
    {
        Renderer renderer = trans.GetComponent<Renderer>();
        if (renderer != null)
        {
            for (int j = 0; j < renderer.materials.Length; j++)
            {
                string shaderName = renderer.materials[j].shader.name;
                Shader shader = Shader.Find(shaderName);
                if (shader != null)
                {
                    renderer.materials[j].shader = shader;
                }
            }
        }

        foreach (Transform t in trans)
        {
            ResetShader(t);
        }
    }
}