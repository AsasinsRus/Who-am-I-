using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioSource[] audioSources;

    private void Awake()
    {
        slider.value = LevelDataHolder.audioVolume;
        SetVolume(LevelDataHolder.audioVolume);
    }

    public void SetVolume(float volume)
    {
        LevelDataHolder.audioVolume = volume;

        foreach(var audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }
}
