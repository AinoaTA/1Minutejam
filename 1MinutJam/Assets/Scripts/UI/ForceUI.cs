using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceUI : MonoBehaviour
{
    public TMP_Text UI;
    public TMP_Text Objects;
    public Camera MainCamera;

    public GameObject WinScreen;
    public GameObject LooseScreen;

    public Animator BlackScreen;

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
        //Timer();
        UpdateLooking();
    }
    public void Winner()
    {
        WinScreen.SetActive(true);
        StartCoroutine(DelayTime());
    }

    public void Loose()
    {
        LooseScreen.SetActive(true);
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
    private void UpdateText(float value)
    {
        //ForceText.text = value.ToString();
    }

    public void UpdateTextObjects()
    {
        Objects.text = GameController.GetGameController().CurrentSavedObjectsInteractables.Count.ToString() + "/" + GameController.GetGameController().AllObjectsInteractables.Count.ToString();
    }

    private void UpdateLooking()
    {
        RaycastHit hit;
        if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, 4.5f))
        {
            if (!hit.collider.CompareTag("Untagged") && GameController.GetGameController().GetPlayer().CanAttach())
            {
                if (hit.collider.CompareTag("Comoda"))
                {
                    if (GameController.GetGameController().GetComoda().Opened)
                    {
                        UI.text = "";
                    }
                    else
                        UI.text = "LMB";
                }
                else
                    UI.text = "LMB";

            }
            else if (hit.collider.CompareTag("Untagged"))
                UI.text = "";

        }
        else
            UI.text = "";

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //public void ResetScene()
    //{
    //    SceneManager.LoadSceneAsync(0);
    //}

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void BackMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);

    }

}
