using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Monster : UnityObject
{
    public Dictionary<string, Rigidbody> Joints;
    public Monster()
        :base(Assets.RagdollCharmeleonPrefab)
    {
        Joints = new Dictionary<string, Rigidbody>();
        RecursivelyPopulateJoints(GameObject);
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
