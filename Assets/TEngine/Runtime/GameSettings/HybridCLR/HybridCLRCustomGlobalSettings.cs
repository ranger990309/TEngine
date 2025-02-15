using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// HybridCLRCustomGlobalSettings.
/// </summary>
[Serializable]
public class HybridCLRCustomGlobalSettings
{
    [Header("Auto sync with [HybridCLRGlobalSettings]")]
    [Tooltip("You should modify the file form file path [Assets/CustomHybridCLR/Settings/HybridCLRGlobalSettings.asset]")]
    [SerializeField]
    private bool m_Enable = false;

    public bool Enable
    {
        get { return m_Enable; }
        set { m_Enable = value; }
    }

    [SerializeField] private bool m_Gitee = true;

    public bool Gitee
    {
        get { return m_Gitee; }
        set { m_Gitee = value; }
    }

    [Header("Auto sync with [HybridCLRGlobalSettings]")]
    [Tooltip("You should modify the file form file path [Assets/CustomHybridCLR/Settings/HybridCLRGlobalSettings.asset]")]
    public List<string> HotUpdateAssemblies = new List<string>() { "GameBase.dll","DotNet.dll","GameProto.dll","BattleCore.Runtime.dll","GameLogic.dll"};

    [Header("Need manual setting!")] public List<string> AOTMetaAssemblies= new List<string>() {"mscorlib.dll","System.dll","System.Core.dll" };

    /// <summary>
    /// Dll of main business logic assembly
    /// </summary>
    public string LogicMainDllName = "GameLogic.dll";

    /// <summary>
    /// 程序集文本资产打包Asset后缀名
    /// </summary>
    public string AssemblyTextAssetExtension = ".bytes";

    /// <summary>
    /// 程序集文本资产资源目录
    /// </summary>
    public string AssemblyTextAssetPath = "AssetRaw/DLL";

    /// <summary>
    /// Resources HybridCLRGlobalSettings Dir
    /// </summary>
    public string HybridCLRGlobalSettings = "Settings/HybridCLRGlobalSettings";
}