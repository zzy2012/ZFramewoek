using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 项目资源导入后处理
/// </summary>
public class ResPostprocess : AssetPostprocessor
{
    //模型导入之前调用  
    public void OnPreprocessModel()
    {
        Debug.Log("OnPreprocessModel=" + this.assetPath);
    }

    //模型导入之前调用  
    public void OnPostprocessModel(GameObject go)
    {
        Debug.Log("OnPostprocessModel=" + go.name);
    }
    //纹理导入之前调用，针对入到的纹理进行设置  
    public void OnPreprocessTexture()
    {
        Debug.Log("OnPreProcessTexture=" + this.assetPath);
        TextureImporter impor = this.assetImporter as TextureImporter;
        impor.textureFormat = TextureImporterFormat.ARGB32;
        impor.maxTextureSize = 512;
        impor.textureType = TextureImporterType.Default;
        impor.mipmapEnabled = false;
    }
    //贴图导入之后调用
    public void OnPostprocessTexture(Texture2D tex)
    {
        Debug.Log("OnPostProcessTexture=" + this.assetPath);
    }
    //音乐导入之后调用
    public void OnPostprocessAudio(AudioClip clip)
    {



    }
    //音乐导入之前调用
    public void OnPreprocessAudio()
    {

       
    }
    //所有资源的导入，删除，移动操作都会调用该方法
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            if (str.Contains("/Xls/"))
            {
                //EditorApplication.ExecuteMenuItem(("Tools/导表工具/导入全部数据 #&E"));
            }
        }
    }
}   
