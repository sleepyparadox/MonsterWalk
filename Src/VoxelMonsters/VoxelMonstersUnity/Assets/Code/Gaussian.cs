using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gaussian
{
    public static float RandomHalfHalf(float mean = 0, float stdDev = 0.1f)
    {
        var u1 = Random.Range(0f, 1f); //these are uniform(0,1) random doubles
        var u2 = Random.Range(0f, 1f);
        var randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        var randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        return randNormal;
    }
}
