using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    private AudioSource _audioSource;

    void Start()
    {
      
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            _audioSource = musicManager.GetAudioSource();
        }

        if (_audioSource != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume", _audioSource.volume);
            _audioSource.volume = savedVolume;

            _slider.value = savedVolume * 100;
            _sliderText.text = (savedVolume * 100).ToString("0") + "%";

            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogWarning("AudioSource bulunamadý!");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (_audioSource != null)
        {
            _audioSource.volume = value / 100;
            PlayerPrefs.SetFloat("Volume", _audioSource.volume); 
            PlayerPrefs.Save();
            _sliderText.text = value.ToString("0") + "%";
        }
    }
}
