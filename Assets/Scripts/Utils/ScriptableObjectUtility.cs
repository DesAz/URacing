using System;
using UnityEngine;
using UnityEditor;
using System.IO;
//using URacing.Checkpoints;

namespace URacing.Utils
{
    public static class ScriptableObjectUtility
    {
        private const string ASSETS_FOLDER = "Assets";

        public static void CreateAsset<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
                path = ASSETS_FOLDER;
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)) ??
                                    throw new Exception("path is null"), "");
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T) + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

//        [MenuItem("Assets/Create/CheckpointSettings")]
//        public static void CreateCheckpointSettings()
//        {
//            CreateAsset<CheckpointSettings>();
//        }
    }
}