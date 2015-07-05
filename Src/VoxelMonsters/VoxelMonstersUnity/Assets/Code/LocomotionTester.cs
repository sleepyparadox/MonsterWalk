using Sleepy.UnityTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LocomotionTester : MonoBehaviour
{
    public int Generation;
    public float TopScore;
    public int TopScoreGen;
    IEnumerator DoPreform()
    {
        var logs = new LocoLogs();
        int monsPerRound = 23;
        int monsToKeep = 5; //Sampled twice

        List<Locomotion> bestLocos = new List<Locomotion>();
        while(true)
        {
            Generation++;
            var locos = new List<Locomotion>();

            var i = 0;
            for (var j = 0; j < bestLocos.Count; ++j )
            {
                locos.Add(new Locomotion(i, bestLocos[j]));
                i++;
                locos.Add(new Locomotion(i, bestLocos[j]));
                i++;
                locos.Add(new Locomotion(i, bestLocos[j]));
                i++;
                locos.Add(new Locomotion(i, bestLocos[j]));
                i++;
            }

            for (; i < monsPerRound; i++)
            {
                locos.Add(new Locomotion(i, null));
            }

            while (locos.Any(l => !l.Finished))
                yield return null;

            foreach (var loco in locos)
                logs.Save(loco);


            bestLocos = locos.OrderByDescending(loco => loco.FinalScore).Take(monsToKeep).ToList();

            if (bestLocos.First().FinalScore > TopScore)
            {
                TopScore = bestLocos.First().FinalScore;
                TopScoreGen = Generation;
            }

            foreach(var best in bestLocos)
            {
                Debug.Log("Best was " + best.Index + " with dist " + best.FinalScore);
            }

            foreach (var l in locos)
                l.Stop();
        }
    }

    void Awake()
    {
        TinyCoro.SpawnNext(DoPreform);
        var cameraRotate = new UnityObject(GameObject.Find("CameraRotate"));
        cameraRotate.UnityUpdate += u => u.Transform.Rotate(Vector3.up, -2f * Time.deltaTime);

        var guiRect = new Rect(0, 0, 500, 50);
        cameraRotate.UnityGUI += (u) => 
            {
                GUI.Label(guiRect, string.Format("Generation: {0}\nBest Score: {1} (final dist + head height, in gen: {2})\nv0.2 tunning for {3}",
                    Generation,
                    TopScore,
                    TopScoreGen,
                    TimeSpan.FromSeconds(Time.timeSinceLevelLoad)));
            };
    }

    void Update()
    {
        TinyCoro.StepAllCoros();
    }
}
