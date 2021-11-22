using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HudController : MonoBehaviour
{

    public Animator Camera;
    

    public AudioSource MusicAudio;
    public Animator BlackScreen;

    private void Awake()
    {
      

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        StartCoroutine(IcreaseAudioCo());
       
    }
    public void StartGame()
    {
        BlackScreen.gameObject.SetActive(true);
        BlackScreen.SetTrigger("BlackScreen");
        StartCoroutine(LoadScene());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        Camera.SetTrigger("Options");
    }
    public void Menu()
    {
        Camera.SetTrigger("Start");
    }

   

    private IEnumerator LoadScene()
    {
       
        StartCoroutine(DecreaseAudioCo());
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
    private IEnumerator DecreaseAudioCo()
    {
        float counter = 0f;

        while (counter < 0.5f)
        {
            counter += Time.deltaTime;

            MusicAudio.volume = Mathf.Lerp(1f, 0f, counter / 0.5f);

            yield return null;
        }
    }
    private IEnumerator IcreaseAudioCo()
    {
        float counter = 0f;

        while (counter < 5f)
        {
            counter += Time.deltaTime;

            MusicAudio.volume = Mathf.Lerp(0f, 1f, counter / 1.5f);

            yield return null;
        }
    }
}
