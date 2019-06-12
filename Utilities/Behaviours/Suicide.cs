using UnityEngine;

/// <summary>
/// Simple script used for disposable objects that should destroy themselves after a set time.
/// </summary>
public class Suicide : MonoBehaviour
{
    public bool triggerOnStart = true;
    public float delay = 0f;

	void Start ()
    {
        if (triggerOnStart)
        {
            Trigger();
        }
	}

    public void Trigger()
    {
        if (delay > 0f)
            Invoke("Kill", delay);
        else
            Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
