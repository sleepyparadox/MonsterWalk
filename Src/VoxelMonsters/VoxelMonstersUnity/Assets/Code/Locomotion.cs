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
    public float BestScore;
    public List<LocoStep> Steps;
    private TinyCoro _coro;
    private Monster _monster;
    
    public Locomotion(int index, List<LocoStep> parentSteps, float mutateAmount)
        : base()
    {
        Index = index;
        _monster = new Monster();
        //Place in rows of 10
        _monster.Transform.position = new Vector3((index % 10) * 3f, -1.38f, Mathf.Floor(index / 10) * 6f);
        //Don't collide
        _monster.GameObject.layer = 8 + index;
        
        _coro = TinyCoro.SpawnNext(DoLocomotion);

        if(parentSteps != null)
        {
            Steps = LocoStep.Mutate(parentSteps, mutateAmount);
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
    }


    IEnumerator DoLocomotion()
    {
        //Init .NextAt
        for (var i = 0; i < Steps.Count; ++i)
            Steps[i].NextAt = Steps[i].At;

        float time = 0f;
        var scores = new List<float>();
        while(true)
        {
            time += Time.deltaTime;
            for (var i = 0; i < Steps.Count; ++i )
            {
                var step = Steps[i];
                if (time >= step.NextAt)
                {
                    step.LastAt = step.NextAt;
                    step.NextAt += LocoStep.GaitDuration;
                }

                if (time >= step.LastAt && time < step.LastAt + step.Duration)
                {
                    //Todo fix literal edge case
                    //var dt = Mathf.Min(time - step.LastAt, Time.deltaTime);
                    var dt = Time.deltaTime;
                    _monster.Joints[step.Joint].AddRelativeTorque(step.Rotation * step.Force * dt);
                }

            }

            var score = _monster.Joints.Values.Max(j => j.transform.position.z) - _monster.WorldPosition.z;
            BestScore = Mathf.Max(BestScore, score);

            yield return null;
        }
    }

    

    public void Stop()
    {
        _coro.Kill();
        _monster.Dispose();
    }
}
