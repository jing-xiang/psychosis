using UnityEngine;

public class CameraRigMovement : MonoBehaviour
{
    public float speed = 3.0f; // Movement speed

    private OVRCameraRig cameraRig;

    void Start()
    {
        // Ensure the script is attached to the OVRCameraRig
        cameraRig = GetComponent<OVRCameraRig>();
        if (cameraRig == null)
        {
            Debug.LogError("CameraRigMovement must be attached to an OVRCameraRig.");
        }
    }

    void Update()
    {
        // Get joystick input from the primary thumbstick (usually the left thumbstick)
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Convert to movement direction
        Vector3 moveDirection = new Vector3(joystickInput.x, 0, joystickInput.y);

        // Move the camera rig
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.Self);
    }
}