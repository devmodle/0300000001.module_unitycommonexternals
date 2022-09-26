#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SimpleFolderIcon.Editor {
	public class IconDictionaryCreator : AssetPostprocessor {
		private const string AssetsPath = "Externals/Etc/SimpleFolderIcon/Icons";
		internal static Dictionary<string, Texture> IconDictionary;

		private static readonly string B_DIR_P_PKGS = "Packages/Module.UnityCommonExternals/";

#if SAMPLE_PROJ
		private static readonly string B_ABS_DIR_P_PKGS = $"{Application.dataPath}/../Packages/.module.unitycommonexternals/";
#else
		private static readonly string B_ABS_DIR_P_PKGS = $"{Application.dataPath}/../Packages/.module.unitycommonexternals@{KCEditorDefine.B_VER_UNITY_MODULE}/";
#endif           // #if SAMPLE_PROJ                            

		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
			if(!ContainsIconAsset(importedAssets) &&
				!ContainsIconAsset(deletedAssets) &&
				!ContainsIconAsset(movedAssets) &&
				!ContainsIconAsset(movedFromAssetPaths)) {
				return;
			}

			BuildDictionary();
		}

		private static bool ContainsIconAsset(string[] assets) {
			foreach(string str in assets) {

				if(ReplaceSeparatorChar(Path.GetDirectoryName(str)) == B_ABS_DIR_P_PKGS + AssetsPath) {
					return true;
				}
			}
			return false;
		}

		private static string ReplaceSeparatorChar(string path) {
			return path.Replace("\\", "/");
		}

		public static void BuildDictionary() {
			var dictionary = new Dictionary<string, Texture>();

			var dir = new DirectoryInfo(B_ABS_DIR_P_PKGS + AssetsPath);
			FileInfo[] info = dir.GetFiles("*.png");
			foreach(FileInfo f in info) {
				var texture = (Texture)AssetDatabase.LoadAssetAtPath($"{B_DIR_P_PKGS}{AssetsPath}/{f.Name}", typeof(Texture2D));
				dictionary.Add(Path.GetFileNameWithoutExtension(f.Name), texture);
			}

			IconDictionary = dictionary;
		}
	}
}
#endif         // #if UNITY_EDITOR
