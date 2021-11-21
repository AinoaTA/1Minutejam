using System.Collections;
using TMPro;
using UnityEngine;

public class ForceUI : MonoBehaviour
{
    public TMP_Text UI;
    public TMP_Text Objects;
    public TMP_Text TimerText;
    public float timer;
    public Camera MainCamera;

    public GameObject WinScreen;

    private void OnEnable()
    {

        PlayerController.delegateForceImpulse += UpdateText;
    }

    private void OnDisable()
    {
        PlayerController.delegateForceImpulse -= UpdateText;
    }

    private void Awake()
    {
        GameController.GetGameController().SetHUD(this);
    }
    private void Start()
    {
        UpdateTextObjects();
    }
    private void Update()
    {
        Timer();
        UpdateLooking();
    }
    public void Winner()
    {
        WinScreen.SetActive(true);
        StartCoroutine(DelayTime());
    }

    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    //texts

    public void Timer()
    {
        timer += Time.deltaTime;
        TimerText.text = timer.ToString();
    }
    private void UpdateText(float value)
    {
        //ForceText.text = value.ToString();
    }

    public void UpdateTextObjects()
    {
        Objects.text = GameController.GetGameController().CurrentSavedObjectsInteractables.Count.ToString()+ "/" + GameController.GetGameController().AllObjectsInteractables.Count.ToString();
    }

    private void UpdateLooking()
    {
        RaycastHit hit;
        if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, 5f))
        {
            if (!hit.collider.CompareTag("Untagged") && GameController.GetGameController().GetPlayer().CanAttach())
            {
                if (hit.collider.CompareTag("Comoda"))
                {
                    if (GameController.GetGameController().GetComoda().Opened)
                    {
                        UI.text = "";
                    }else
                        UI.text = "RMB";
                }
                else
                    UI.text = "RMB";

            }
            else if (hit.collider.CompareTag("Untagged"))
                UI.text = "";
            
        }
        else
            UI.text = "";
        
    }
}
