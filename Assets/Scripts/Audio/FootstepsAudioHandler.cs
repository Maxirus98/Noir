using UnityEngine;
using UnityEngine.SceneManagement;

public class FootstepsAudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] woodFS;
    [SerializeField]
    private AudioClip[] reverbCimentFS;
    [SerializeField]
    private AudioClip[] cimentFS;

    private float volume = 1f;

    public void OnPlayFootstep(float pVolume = 1)
    {
        volume = pVolume;
        var activeScene = SceneManager.GetActiveScene();
        switch (activeScene.name)
        {
            case "BureauPlaytestD":
                PlayRandomFootsteps(woodFS);
                break;
            case "BarD":
                PlayRandomFootsteps(woodFS);
                break;
            case "LaboratoireD":
                PlayRandomFootsteps(reverbCimentFS);
                break;
            case "RuePlaytestD":
                PlayRandomFootsteps(cimentFS);
                break;
        }
    }

    private void PlayRandomFootsteps(AudioClip[] footsteps)
    {
        int randomIndex = Random.Range(0, footsteps.Length);
        AudioSource.PlayClipAtPoint(footsteps[randomIndex], transform.position, volume);
    }
}
