using UnityEngine;

public class TriggerPlayerGuide : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Go Up");
            GameObject.Find("Indicator").SendMessage("StateDown");
            // GameObject.Find("Player").SendMessage("ActuateThePGM");

        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Go Down");
            GameObject.Find("Indicator").SendMessage("StateHigh");
            // GameObject.Find("Player").SendMessage("RelaxThePGM");
        }
    }
}
