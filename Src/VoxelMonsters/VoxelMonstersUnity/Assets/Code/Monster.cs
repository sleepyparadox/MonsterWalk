using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Monster : UnityObject
{
    public Dictionary<string, Rigidbody> Joints;
    static Dictionary<string, Material> ColorBuffer = new Dictionary<string,Material>();

    public Monster(int layer, Color color)
        :base(Assets.RagdollCharmeleonPrefab)
    {
        Joints = new Dictionary<string, Rigidbody>();
        RecursivelyPopulateJoints(GameObject);
        RecursivelySetLayer(GameObject, layer);
        RecursivelySetColor(GameObject, color);
    }

    void RecursivelySetColor(GameObject obj, Color color)
    {
        var r = obj.GetComponent<Renderer>();
        if (r != null)
        {
            //var key = r.sharedMaterial.name + color.ToString();
            //if(!ColorBuffer.ContainsKey(key))
            //{
            //    var material = new Material(r.sharedMaterial);
            //    material.color = color;
            //    ColorBuffer.Add(key, r.material);
            //}
            //r.sharedMaterial = ColorBuffer[key];
            r.material.color = color;
        }
        for (var i = 0; i < obj.transform.childCount; ++i)
            RecursivelySetColor(obj.transform.GetChild(i).gameObject, color);
    }

    void RecursivelySetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        for (var i = 0; i < obj.transform.childCount; ++i)
            RecursivelySetLayer(obj.transform.GetChild(i).gameObject, layer);
    }

    void RecursivelyPopulateJoints(GameObject obj)
    {
        //var joint = obj.GetComponent<CharacterJoint>();
        var rigid = obj.GetComponent<Rigidbody>();
        if (/*joint != null &&*/ rigid != null)
        {
            Joints.Add(obj.name, rigid);
        }
        for (var i = 0; i < obj.transform.childCount; ++i)
            RecursivelyPopulateJoints(obj.transform.GetChild(i).gameObject);
    }
}
