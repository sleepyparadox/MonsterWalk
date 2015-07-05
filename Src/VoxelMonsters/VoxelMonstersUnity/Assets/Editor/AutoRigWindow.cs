using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class AutoRigWindow : EditorWindow
{
    [MenuItem("SleepyParadox/AutoRig")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        AutoRigWindow window = (AutoRigWindow)EditorWindow.GetWindow(typeof(AutoRigWindow));
        window.Show();
    }

    public GameObject _fbxImport;
    public string _outputFolder;
    
    public void OnGUI()
    {
        GUILayout.Label("SrcMesh");

        _fbxImport = (GameObject)EditorGUILayout.ObjectField(_fbxImport, typeof(GameObject), false);

        _outputFolder = Application.dataPath + "/VoxOut";

        GUILayout.Label(_outputFolder);

        if(!Application.isPlaying)
        {
            GUILayout.Label("Editor must be playing");
            GUI.enabled = false;
        }
        else
        {
            GUILayout.Label("");
        }

        if (GUILayout.Button("Generate"))
        {
            GenerateRig(_fbxImport, _outputFolder);
        }
    }

    static void GenerateRig(GameObject src, string outputFolder)
    {
        var animator = src.GetComponent<Animation>();


        //var maxVert = new Vector3();
        //var minVert = new Vector3();
        //foreach(var vert in src.vertices)
        //{
        //    maxVert.x = Mathf.Max(maxVert.x, vert.x);
        //    maxVert.y = Mathf.Max(maxVert.y, vert.x);
        //    maxVert.z = Mathf.Max(maxVert.z, vert.x);
        //    minVert.x = Mathf.Max(minVert.x, vert.x);
        //    minVert.y = Mathf.Max(minVert.y, vert.x);
        //    minVert.z = Mathf.Max(minVert.z, vert.x);
        //}
        //var vertSize = maxVert + (-1 * minVert);

        //var meshObject = new GameObject("Mesh");
        //meshObject.transform.localPosition = maxVert + (vertSize * -0.5f);
        //var meshFilter = meshObject.AddComponent<MeshFilter>();
        //meshFilter.mesh = src;
        //var meshRenderer = meshObject.AddComponent<MeshRenderer>();
    }

    static T GetComponentInTargetOrChildren<T>(GameObject obj) where T : MonoBehaviour
    {
        var t = obj.GetComponent<T>();
        if (t != null)
            return t;

        for (var i = 0; i < obj.transform.childCount; ++i)
        {
            t = GetComponentInTargetOrChildren<T>(obj.transform.GetChild(i).gameObject);
            if (t != null)
                return t;
        }

        return null;
    }
}
