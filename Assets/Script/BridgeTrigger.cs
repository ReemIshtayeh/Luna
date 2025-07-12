using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    public Transform[] bridgesToRotate;        // Assign Bridge1, Bridge2, Bridge3 in Inspector
    public Transform[] StepsToApear;
    public Vector3 targetRotation = new Vector3(0, 0, -90); // Rotation when triggered
    public float rotationSpeed = 90f;          // Degrees per second

    private bool shouldRotate = false;
    public bool shouldStepsAppear = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))        // Make sure your Player has the tag "Player"
        {
            //play picked sound
            AudioManager.Instance.PlayPickedSFX();
            shouldRotate = true;
        }
    }

    private void Update()
    {
        if (!shouldRotate) return;

        foreach (Transform bridge in bridgesToRotate)
        {
            Quaternion target = Quaternion.Euler(targetRotation);
            bridge.rotation = Quaternion.RotateTowards(bridge.rotation, target, rotationSpeed * Time.deltaTime);
        }
        if (shouldStepsAppear)
        {
            foreach (Transform step in StepsToApear)
            {
                step.gameObject.SetActive(true);
            }
        }
    }
    
}
