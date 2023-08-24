using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    [Header("RESOLUTION")]
    public Toggle fullscreenTog, vsyncTog;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    [Header("VOLUME COMPONENTS")]
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TextMeshProUGUI volumeTextUI = null;
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0) {
            vsyncTog.isOn = false;
        }
        else {
            vsyncTog.isOn = true;
        }
        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++) {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical) {
                foundRes = true;

                selectedResolution = i;

                UpdateResLabel();
            }
        }
        if (!foundRes) {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;

            UpdateResLabel();
        }
        LoadValues();
    }
    void Update()
    {
        
    }
    public void ResLeft() {
        selectedResolution--;
        if(selectedResolution < 0) {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }
    public void ResRight() { 
        selectedResolution++;
        if(selectedResolution > resolutions.Count - 1) {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }
    public void UpdateResLabel() {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " X " + resolutions[selectedResolution].vertical.ToString();
    }
    public void ApplyGraphics() {
        if (vsyncTog.isOn ) {
            QualitySettings.vSyncCount = 1;
        }
        else {
            QualitySettings.vSyncCount = 0;
        }
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }


    public void QuitGame() {
        Application.Quit();
        Debug.Log("Application Quit!");
    }

    //AUDIO
    public void VolumeSlider(float volume) {
        volume = Mathf.Round(Mathf.Lerp(0, 100, volume));
        volumeTextUI.text = volume.ToString();
        Debug.Log(volume);
    }
    public void SaveVolumeButton() {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadValues();
    }
    void LoadValues() {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}

[System.Serializable]
public class ResItem { //for resolution
    public int horizontal, vertical;
}
