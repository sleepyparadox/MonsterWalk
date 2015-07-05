using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LocoStep
{
    public const float GaitDuration = 5f;

    public string Joint;
    public float At;
    public float Duration;
    public float Force;
    public Vector3 Rotation;

    public float LastAt = -10000;
    public float NextAt = 10000;

    public static LocoStep SpawnNew(string joint)
    {
        return new LocoStep()
        {
            Joint = joint,
            At = UnityEngine.Random.Range(0f, LocoStep.GaitDuration),
            Force = UnityEngine.Random.Range(1, 100) * 1000f,
            Duration = UnityEngine.Random.Range(0f, LocoStep.GaitDuration),
            Rotation = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))
        };
    }

    static LocoStep Mutate(LocoStep source, float mutateAmount)
    {
        //Spawn a new mutant
        var mutant = SpawnNew(source.Joint);

        //Inherit from src properties
        mutant.At = Mathf.Lerp(source.At, mutant.At, mutateAmount);
        mutant.Force = Mathf.Lerp(source.Force, mutant.Force, mutateAmount);
        mutant.Duration = Mathf.Lerp(source.Duration, mutant.Duration, mutateAmount);
        mutant.Rotation = new Vector3(Mathf.Lerp(source.Rotation.x, mutant.Rotation.x, mutateAmount),
                                        Mathf.Lerp(source.Rotation.y, mutant.Rotation.y, mutateAmount),
                                        Mathf.Lerp(source.Rotation.y, mutant.Rotation.y, mutateAmount));

        return mutant;
    }

    public static List<LocoStep> Mutate(List<LocoStep> src, float mutateAmount)
    {
        var mutants = new List<LocoStep>();
        for (var i = 0; i < src.Count; ++i)
        {
            mutants.Add(Mutate(src[i], mutateAmount));
        }
        return mutants;
    }
}

