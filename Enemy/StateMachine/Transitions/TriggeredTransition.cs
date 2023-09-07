using UnityEngine;

public class TriggeredTransition : Transition
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Trigger();
    }

    public void Trigger()
    {
        Transit();
    }
}
