using Sleepy.UnityTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LocomotionTester : MonoBehaviour
{
    IEnumerator DoPreform()
    {
        int monsPerRound = 20;
        int monsToKeep = 5; //Sampled twice

        List<Locomotion> bestLocos = new List<Locomotion>();
        while(true)
        {
            var locos = new List<Locomotion>();
            var i = 0;

            for (; i < bestLocos.Count; i++)
            {
                locos.Add(new Locomotion(monsPerRound - i, bestLocos[i].Steps, 0f));
            }

            for (; i < bestLocos.Count; i++)
            {
                locos.Add(new Locomotion(monsPerRound - i, bestLocos[i].Steps, 0.5f));
            }

            for (; i < monsPerRound; i++)
            {
                locos.Add(new Locomotion(monsPerRound - i, null, 0f));
            }

            yield return TinyCoro.Wait(10f);

            bestLocos = locos.OrderByDescending(loco => loco.BestScore).Take(monsToKeep).ToList();

            foreach(var best in bestLocos)
            {
                Debug.Log("Best was " + best.Index + " with dist " + best.BestScore);
            }

            foreach (var l in locos)
                l.Stop();
        }
    }

    void Awake()
    {
        TinyCoro.SpawnNext(DoPreform);
    }

    void Update()
    {
        TinyCoro.StepAllCoros();
    }
}
