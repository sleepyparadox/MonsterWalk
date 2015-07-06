using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LocoStep
{
    public const float GaitDuration = 2.5f;

    public string Joint;
    public float At;
    public float Duration;
    public float Force;
    public Vector3 Rotation;

    public float LastAt = -10000;
    public float NextAt = 10000;

    const float MaxForce = 10000000f;

    public static LocoStep SpawnNew(string joint)
    {
        return new LocoStep()
        {
            Joint = joint,
            At = UnityEngine.Random.Range(0f, LocoStep.GaitDuration),
            Force = MaxForce * MaxForce,
            Duration = UnityEngine.Random.Range(0f, LocoStep.GaitDuration),
            Rotation = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))
        };
    }

    static LocoStep Mutate(LocoStep source)
    {
        //Spawn a new mutant
        var mutant = SpawnNew(source.Joint);

        //Inherit from src properties
        mutant.At += Gaussian.RandomHalfHalf() * LocoStep.GaitDuration;
        while (mutant.At >= LocoStep.GaitDuration)
            mutant.At -= LocoStep.GaitDuration;
        while (mutant.At < 0)
            mutant.At += LocoStep.GaitDuration;

        mutant.Force += Gaussian.RandomHalfHalf() * MaxForce;
        mutant.Force = Mathf.Clamp(mutant.Force, 0, MaxForce);

        mutant.Duration += Gaussian.RandomHalfHalf() * LocoStep.GaitDuration;
        mutant.Duration = Mathf.Clamp(mutant.Duration, 0, MaxForce);

        mutant.Rotation += new Vector3(Gaussian.RandomHalfHalf(), Gaussian.RandomHalfHalf(), Gaussian.RandomHalfHalf());
        mutant.Duration = Mathf.Clamp(mutant.Duration, 0, MaxForce);

        return mutant;
    }

    public static List<LocoStep> Mutate(List<LocoStep> src)
    {
        var mutants = new List<LocoStep>();
        for (var i = 0; i < src.Count; ++i)
        {
            mutants.Add(Mutate(src[i]));
        }
        return mutants;
    }

    public static Color Mutate(Color src)
    {
        return new Color(Mathf.Clamp01(src.r + Gaussian.RandomHalfHalf()),
                    Mathf.Clamp01(src.g + Gaussian.RandomHalfHalf()),
                    Mathf.Clamp01(src.b + Gaussian.RandomHalfHalf()));
    }
}

