using System.IO;
using UnityEditor;
using UnityEngine;

public partial class FolderInitialTool : EditorWindow
{
   static string[] folderList = {"Materials","Scripts","Prefabs","Shaders","Textures","Models" };
    [MenuItem("Folder Creater/Creat Init Folders")] 
    private static void CreateFolders()
    {
#if UNITY_EDITOR
        var unityAssetpath = Application.dataPath;
#endif
        foreach (var folder in folderList)
        {
            Directory.CreateDirectory(Path.Combine(unityAssetpath,folder));
        }
        EditorUtility.DisplayDialog(
            "Files Created!!",
            FilesInfo(),"Thanks");
    }
  static string FilesInfo()
    {
        var info = "";
        foreach (var folder in folderList)
        {
            info +=folder+"  ";
        }
        info += "was created !";
        return info;
    }
}

