using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceCorrect;
    public AudioSource audioSourceIncorrect;

    public void SonidoRespuesta(bool isCorrect)
    {
        if (isCorrect)
            audioSourceCorrect.Play();
        else
            audioSourceIncorrect.Play();
    }
}
