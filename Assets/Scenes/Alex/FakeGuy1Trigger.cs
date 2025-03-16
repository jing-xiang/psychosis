using UnityEngine;

public class FakeGuy1Trigger : MonoBehaviour
{
    public AudioSource creepyLaugh;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered by: " + other.gameObject.name);

        if (IsHandOrController(other))
        {
            Debug.Log("Controller or hand detected! Attempting to play creepy laugh...");

            if (!creepyLaugh.isPlaying)
            {
                creepyLaugh.Play();
                Debug.Log("Sound should be playing now!");
            }
            else
            {
                Debug.Log("Sound was already playing!");
            }
        }
    }

    private bool IsHandOrController(Collider other)
    {
        return other.CompareTag("LeftController") ||
               other.CompareTag("RightController") ||
               other.CompareTag("LeftHand") ||
               other.CompareTag("RightHand");
    }
}
