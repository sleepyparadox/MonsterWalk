using Sleepy.UnityTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Locomotion
{
    public readonly int Index;
    public float FinalScore;
    public Color Color;
    public List<LocoStep> Steps;
    private Monster _monster;
    public bool Finished;
    int _frames;
    float _time = 0f;

    public Locomotion(int index, Locomotion parent)
        : base()
    {
        Index = index;

        if (parent != null)
        {
            Color = LocoStep.Mutate(parent.Color);
        }
        else
        {
            Color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        }

        const int MonsPerRow = 7;
        var pos = new Vector3((index % MonsPerRow) * 3f, -1.38f, Mathf.Floor(index / MonsPerRow) * 6f);

        var physicsLayer = 8 + index;
        _monster = new Monster(physicsLayer, Color);
        _monster.Transform.position = pos;


        if (parent != null)
        {
            Steps = LocoStep.Mutate(parent.Steps);
        }
        else
        {
            Steps = new List<LocoStep>();
            foreach (var joint in _monster.Joints.Keys)
            {
                var temp = joint;
                Steps.Add(LocoStep.SpawnNew(temp));
                Steps.Add(LocoStep.SpawnNew(temp));
            }
        }

        for (var i = 0; i < Steps.Count; ++i)
            Steps[i].NextAt = Steps[i].At;
        _monster.UnityFixedUpdate += OnFixedUpdate;
    }

   
    void OnFixedUpdate(UnityObject me)
    {
        _time += Time.fixedDeltaTime;
        
        if(!Finished)
        {
            _frames++;
            if (_frames > 60 * 10)
                Finished = true;

            for (var i = 0; i < Steps.Count; ++i )
            {
                var step = Steps[i];
                if (_time >= step.NextAt)
                {
                    step.LastAt = step.NextAt;
                    step.NextAt += LocoStep.GaitDuration;
                }

                if (_time >= step.LastAt && _time < step.LastAt + step.Duration)
                {
                    //Todo fix literal edge case
                    //var dt = Mathf.Min(_time - step.LastAt, Time.fixedDeltaTime);
                    var dt = Time.fixedDeltaTime;
                    _monster.Joints[step.Joint].AddRelativeTorque(step.Rotation * step.Force * dt);
                }
            }

            var dist = _monster.Joints.Values.Max(j => j.transform.position.z) - _monster.WorldPosition.z;

            FinalScore = dist + _monster.Joints["Head"].position.y;
        }
    }

    public void Stop()
    {
        _monster.Dispose();
    }
}
