using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateConfig : MonoBehaviour {

    [MenuItem("Tools/CreateConfig")]
    private static void Create()
    {
        CreateRoleDataConfig();
    }

    private static void CreateRoleDataConfig()
    {
        RoleConfig roleConfig = ScriptableObject.CreateInstance<RoleConfig>();

        //填充好数据后就可以打包到Assetbundle中了
        //第一步必须先创建一个保存了配置数据的Asset文件，后缀必须为asset
        AssetDatabase.CreateAsset(roleConfig, "Assets/Data/RoleConfig.asset");

    }
    [MenuItem("Assets/Create/Song")]
    public static void CreateNewSongAsset()
    {
        PlayerSongData asset = ScriptableObject.CreateInstance<PlayerSongData>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/NewSong.asset");
    }

    [MenuItem("Assets/Create/WaterMusic")]
    public static void CreateWaterMusicAsset()
    {
        WaterMusicData asset = ScriptableObject.CreateInstance<WaterMusicData>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/WaterMusic.asset");
    }

    [MenuItem("Assets/Create/DishMusic")]
    public static void CreateDishMusicAsset()
    {
        MusicGameConfig asset = ScriptableObject.CreateInstance<MusicGameConfig>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/DishMusic.asset");
    }

    [MenuItem("Assets/Create/RecordMusic")]
    public static void CreateRecordMusicAsset()
    {
        RecordConfig asset = ScriptableObject.CreateInstance<RecordConfig>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/RecordConfig.asset");

    }
}
