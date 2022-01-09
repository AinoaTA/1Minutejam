using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public TMP_Text UI;
    public TMP_Text Objects;
    public Camera MainCamera;

    public GameObject WinScreen;
    public GameObject LooseScreen;

    public Animator BlackScreen;
    public GameObject quitLose, quitWin;
  

    private void Awake()
    {
        GameController.GetGameController().SetHUD(this);
    }
    private void Start()
    {
#if UNITY_WEBGL
        quitWin.SetActive(false);
        quitLose.SetActive(false);
#endif
        UpdateTextObjects();
    }
    private void Update()
    {
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
