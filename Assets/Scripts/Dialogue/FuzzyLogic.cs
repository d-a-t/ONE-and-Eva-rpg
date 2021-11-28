using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    // create 3 animation curves - Jedi, Neutral, Sith
    // create empty list. Every triggered event, add to listsss
    // Average elements of list when evaluating

    [SerializeField]
    private AnimationCurve ending;
    [SerializeField]
    private AnimationCurve good;
    [SerializeField]
    private AnimationCurve death;

    public List<int> triggerEvents;
    public float triggeredEventsTotal = 0;
    public float triggeredAverage;

    public void AddToList(int num)
    {
        triggerEvents.Add(num);
    }

    public int EvaluateFuzzy()
    {
        foreach (var eventValue in triggerEvents)
        {
            triggeredEventsTotal = triggeredEventsTotal + eventValue;
        }

        triggeredAverage = triggeredEventsTotal / triggerEvents.Count;
        float ending = this.ending.Evaluate(triggeredAverage);
        float good_ending = good.Evaluate(triggeredAverage);
        float death = this.death.Evaluate(triggeredAverage);

        Debug.Log("Ending: " + ending);
        Debug.Log("Good Ending: " + good_ending);
        Debug.Log("Death: " + death);

        if (death > good_ending && death > ending)
        {
            return -1;
        }
        else if (ending > good_ending)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}
