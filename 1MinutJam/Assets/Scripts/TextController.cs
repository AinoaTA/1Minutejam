using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    [SerializeField] private string[] phrases;
    private bool timer;
    private float currentTimer;

    public TMP_Text text;
    public AudioSource MomsSteps;

    public AudioSource MusicAudio;
    public AudioSource MusicDoorOpen;
    public AudioSource hitFlipFlop;
    public void Writing(int numer)
    {
        text.text = phrases[numer];

  
            
    }

    public void StepsMoon()
    {
        MomsSteps.Play();
    }
    private void Update()
    {
        print(currentTimer);
        if (timer)
        {
            currentTimer += Time.deltaTime;
        }
        TimerOver();
           
    }
    public void PlayerStop() 
    {
        GameController.GetGameController().GetPlayer().m_CharacterController.enabled = false;
    }
    public void PlayerGO()
    {
        timer = true;
        StartCoroutine(IcreaseAudioCo());
        GameController.GetGameController().GetPlayer().m_CharacterController.enabled = true;
    }

    private IEnumerator IcreaseAudioCo()
    {
        float counter = 0f;
        MusicAudio.Play();
        while (counter < 1f)
        {
            counter += Time.deltaTime;

            MusicAudio.volume = Mathf.Lerp(0f, 0.5f, counter / 1.5f);

            yield return null;
        }
    }

    private void TimerOver()
    {
        if (currentTimer >= 61 && !GameController.GetGameController().ReviseCount())
        {
            GameController.GetGameController().GetHud().Loose();
        }
    }

    public void OpenDoor()
    {
        MusicDoorOpen.Play();
    }

    public void HitFlipFlop()
    {
        hitFlipFlop.Play();
    }

}
