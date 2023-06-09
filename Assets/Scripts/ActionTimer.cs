using UnityEngine;

// Timer representing whether an action with a cooldown can be performed
public class ActionTimer
{
    public bool actionAllowed { get; private set; }
    public float timeRemaining { get; private set; }
    private float cooldown;

    public ActionTimer(float cooldown)
    {
        actionAllowed = true;
        timeRemaining = 0f;
        this.cooldown = cooldown;
    }

    public void Update(float deltaTime)
    {
        if (timeRemaining > 0f)
        {
            timeRemaining = Mathf.Max(0f, timeRemaining - deltaTime);
        }

        if (!actionAllowed && timeRemaining == 0f)
        {
            actionAllowed = true;
        }
    }

    public void Start()
    {
        actionAllowed = false;
        timeRemaining = cooldown;
    }
}
