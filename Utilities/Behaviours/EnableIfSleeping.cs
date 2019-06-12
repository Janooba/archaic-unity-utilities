using UnityEngine;

/// <summary>
/// Enables a behaviour when a rigidbody settles movement otherwise disables the behaviour.
/// </summary>
public class EnableIfSleeping : MonoBehaviour
{
    public Behaviour Behaviour;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_rigidbody == null || Behaviour == null)
            return;

        if (_rigidbody.IsSleeping() && !Behaviour.enabled)
            Behaviour.enabled = true;

        if (!_rigidbody.IsSleeping() && Behaviour.enabled)
            Behaviour.enabled = false;
    }
}
