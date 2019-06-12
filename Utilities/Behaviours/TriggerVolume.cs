using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerVolume : MonoBehaviour
{
    public string[] QueryTags;

    public UnityEvent onFirstEnter;
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public UnityEvent onLastExit;

    private List<GameObject> objectsInside = new List<GameObject>();

    private void Start()
    {
        QueryTags = QueryTags.Where(x => x.Trim().Length > 0).ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (DoesTagMatch(other))
        {
            if (objectsInside.Count == 0)
                onFirstEnter.Invoke();

            onEnter.Invoke();
            objectsInside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInside.Contains(other.gameObject))
        {
            objectsInside.Remove(other.gameObject);
            onExit.Invoke();

            if (objectsInside.Count == 0)
                onLastExit.Invoke();
        }
    }

    private bool DoesTagMatch(Collider other)
    {
        bool matched = false;

        if (QueryTags.Length == 0)
            matched = true;
        else
            for (int i = 0; i < QueryTags.Length; i++)
            {
                if (other.tag == QueryTags[i].Trim())
                    matched = true;
            }
        return matched;
    }
}