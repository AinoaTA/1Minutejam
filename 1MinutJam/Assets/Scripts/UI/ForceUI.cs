using TMPro;
using UnityEngine;

public class ForceUI : MonoBehaviour
{
    public TMP_Text ForceText;


    private void OnEnable()
    {

        PlayerController.delegateForceImpulse += UpdateText;
    }

    private void OnDisable()
    {
        PlayerController.delegateForceImpulse -= UpdateText;
    }

    private void UpdateText(float value)
    {
        ForceText.text = value.ToString();
    }
}
