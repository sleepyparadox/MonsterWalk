using Sleepy.UnityTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class LocomotionTester : MonoBehaviour
{
    public int Generation;
    public float TopScore;
    public int TopScoreGen;
    private int _monsPerRound;
    private int _monsToKeep;
    private int _childrenToMutate;
    private string[] _configLines;

    Locomotion _bestAllTime;
    private int _bestInject;

    IEnumerator DoPreform()
    {
        var logs = new LocoLogs();
        _monsPerRound = 40;
        _monsToKeep = 6;
        _childrenToMutate = 6;
        _bestInject = 1;

        Application.RegisterLogCallback(OnLog);

        List<Locomotion> bestLocos = new List<Locomotion>();
        while(true)
        {
            LoadConfigSettings();

            Generation++;
            var locos = new List<Locomotion>();

            var i = 0;
            for (var bestIndex = 0; bestIndex < bestLocos.Count; ++bestIndex )
            {
                if (_bestInject > 0 
                    && bestIndex >= _monsToKeep - _bestInject && _bestAllTime != null)
                {
                    //Use best alltime
                    for (var childIndex = 0; childIndex < _childrenToMutate; ++childIndex)
                    {
                        locos.Add(new Locomotion(_monsPerRound - i - 1, _bestAllTime));
                        i++;
                    }
                    continue;
                }

                for (var childIndex = 0; childIndex < _childrenToMutate; ++childIndex)
                {
                    //Index modified so children appear at front
                    locos.Add(new Locomotion(_monsPerRound - i - 1, bestLocos[bestIndex]));
                    i++;
                }
            }

            for (; i < _monsPerRound; i++)
            {
                //Index modified so children appear at front
                locos.Add(new Locomotion(_monsPerRound - i - 1, null));
            }

            while (locos.Any(l => !l.Finished))
                yield return null;

            foreach (var loco in locos)
                logs.Save(loco);


            bestLocos = locos.OrderByDescending(loco => loco.FinalScore).Take(_monsToKeep).ToList();

            var best = bestLocos.First();
            if (_bestAllTime == null || best.FinalScore > _bestAllTime.FinalScore)
                _bestAllTime = best;

            if (bestLocos.First().FinalScore > TopScore)
            {
                TopScore = bestLocos.First().FinalScore;
                TopScoreGen = Generation;
            }

            foreach (var l in locos)
                l.Stop();
        }
    }

    private void OnLog(string condition, string stackTrace, LogType type)
    {
        var writer = new StreamWriter("logs.txt", true);
        writer.WriteLine(type.ToString());
        writer.WriteLine(condition);
        writer.WriteLine(stackTrace);
        writer.Close();
    }

    void LoadConfigSettings()
    {
        try
        {
            var path = Directory.GetCurrentDirectory() + "/config.txt";
            if (!File.Exists(path))
                return;
            var config = new Dictionary<string, string>();
            _configLines = File.ReadAllLines(path);
            foreach (var line in _configLines)
            {
                var cells = line.Split('=');
                config.Add(cells[0], cells[1]);
            }

            Gaussian.StdDev = float.Parse(config["stdDev"]);
            Locomotion.FramesPerTest = int.Parse(config["FramesPerTest"]);
            _monsPerRound = int.Parse(config["monsPerRound"]);
            _monsToKeep = int.Parse(config["monsToKeep"]);
            _childrenToMutate = int.Parse(config["childrenToMutate"]);
            _bestInject = int.Parse(config["bestInject"]);

            Time.timeScale = float.Parse(config["timescale"]);
        }
        catch (Exception e)
        {
            File.WriteAllText("exception" + DateTime.UtcNow.Ticks + ".txt", e.ToString());
            Application.Quit();
        }
    }

    void Awake()
    {
        TinyCoro.SpawnNext(DoPreform);
        var cameraRotate = new UnityObject(GameObject.Find("CameraRotate"));
        cameraRotate.UnityUpdate += u => u.Transform.Rotate(Vector3.up, -2f * Time.deltaTime);


        var guiRect = new Rect(0, 0, Screen.width, Screen.height);
        cameraRotate.UnityGUI += (u) => 
            {
                var guiText =  string.Format("Generation: {0}\nBest Score: {1} (distance + head height + waist height, in gen: {2})\nv0.4 running for {3}",
                    Generation,
                    TopScore,
                    TopScoreGen,
                    TimeSpan.FromSeconds(Time.timeSinceLevelLoad));
                if(_configLines != null)
                    guiText += "\n" + string.Join("\n", _configLines);

                GUI.Label(guiRect, guiText);
            };
    }

    void Update()
    {
        TinyCoro.StepAllCoros();
    }
}
