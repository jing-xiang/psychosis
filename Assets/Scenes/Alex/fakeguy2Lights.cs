
using UnityEngine;
using System.Collections;

public class fakeguy2Lights : MonoBehaviour
{
    public float rotationSpeed = 1.5f;  // Adjust speed as needed
    public float maxAngle = 90f; // Maximum rotation (from 90 to -90)
    private bool isSwinging = false;
    private Coroutine swingCoroutine;

    void Start()
    {
        transform.localRotation = Quaternion.Euler(90, 0, 0); // Set starting rotation
    }

    public void StartSwinging()
    {
        if (!isSwinging)
        {
            isSwinging = true;
            swingCoroutine = StartCoroutine(SwingLight());
        }
    }

    public void StopSwinging()
    {
        isSwinging = false;
        if (swingCoroutine != null)
        {
            StopCoroutine(swingCoroutine);
        }
        transform.localRotation = Quaternion.Euler(90, 0, 0); // Reset rotation
    }

    IEnumerator SwingLight()
    {
        while (isSwinging)
        {
            // Swing from -maxAngle to maxAngle
            for (float t = 0; t <= 1; t += Time.deltaTime * rotationSpeed)
            {
                float angle = Mathf.Lerp(-maxAngle, maxAngle, t);
                transform.localRotation = Quaternion.Euler(angle, 0, 0);
                yield return null;
            }

            for (float t = 0; t <= 1; t += Time.deltaTime * rotationSpeed)
            {
                float angle = Mathf.Lerp(maxAngle, -maxAngle, t);
                transform.localRotation = Quaternion.Euler(angle, 0, 0);
                yield return null;
            }
        }
    }
}
