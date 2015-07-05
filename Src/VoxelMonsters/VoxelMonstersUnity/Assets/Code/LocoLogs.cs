using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class LocoLogs
{
    string _dir;
    StreamWriter _indexWriter;

    public static List<List<LocoLogs>> GetBestMonstersOnFile()
    {
        var mons = new List<List<LocoLogs>> ();
        var indexPath = Directory.GetCurrentDirectory() + "/Locos/Local/_index.csv";

        if (!File.Exists(indexPath))
            return mons;

        var lines = File.ReadAllLines(indexPath);

        return mons;
    }


    public LocoLogs()
    {
        var dirUnique = Guid.NewGuid().ToString().Split('-')[0];
        _dir = Directory.GetCurrentDirectory() + "/Locos/Local";
        Debug.Log("Start logging to " + _dir);
        if (!Directory.Exists(_dir))
            Directory.CreateDirectory(_dir);
        _indexWriter = new StreamWriter(_dir + "/_index.csv");
        _indexWriter.WriteLine("score,name");
    }

    public void Save(Locomotion loco)
    {
        var name = Guid.NewGuid().ToString();

        _indexWriter.WriteLine(loco.FinalScore + "," + name);
        _indexWriter.Flush();

        //Write complex data
        var lines = new List<string>();
        {
            var cells = new string[]
            {
                "Joint",
                "At",
                "Duration",
                "Force",
                "Rotation.x",
                "Rotation.y",
                "Rotation.z",
            };
            lines.Add(string.Join(",", cells));
        }
        foreach(var step in loco.Steps)
        {
            var cells = new string[]
            {
                step.Joint,
                step.At.ToString(),
                step.Duration.ToString(),
                step.Force.ToString(),
                step.Rotation.x.ToString(),
                step.Rotation.y.ToString(),
                step.Rotation.z.ToString(),
            };
            lines.Add(string.Join(",", cells));
        }
        File.WriteAllLines(_dir + "/" + name + ".csv", lines.ToArray());
    }
}
