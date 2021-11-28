using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    // create 3 animation curves - Jedi, Neutral, Sith
    // create empty list. Every triggered event, add to listsss
    // Average elements of list when evaluating

    [SerializeField]
    private AnimationCurve good;
    [SerializeField]
    private AnimationCurve bad;

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
        float good_ending = good.Evaluate(triggeredAverage);
        float bad_ending = this.bad.Evaluate(triggeredAverage);

        Debug.Log("Good Ending: " + good_ending);
        Debug.Log("Bad Ending: " + bad_ending);

        if (bad_ending > good_ending)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
