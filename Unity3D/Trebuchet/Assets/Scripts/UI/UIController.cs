using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public delegate void OnAngleSliderChanged(float value, float min, float max);
    public event OnAngleSliderChanged onAngleSliderChanged;

    [SerializeField]
    private UIShotAngleDisplay angleDisplay;
    [SerializeField]
    private Slider shotAngleSlider;
    [SerializeField]
    private GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        shotAngleSlider.onValueChanged.AddListener(AngleSliderChanged);
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
    }

    public void OnButtonEvent_ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void AngleSliderChanged(float value)
    {
        onAngleSliderChanged?.Invoke(value, shotAngleSlider.minValue, shotAngleSlider.maxValue);
    }

    public void TrebuchetReleaseAngleUpdated(float angle, float min, float max)
    {
        angleDisplay.SetAngleValue(angle);
        SetShotAngleSliderValue(angle, min, max);
    }

    private void SetShotAngleSliderValue(float value, float min, float max)
    {
        shotAngleSlider.onValueChanged.RemoveListener(AngleSliderChanged);
        shotAngleSlider.value = Utility.Remap(value, min, max, shotAngleSlider.minValue, shotAngleSlider.maxValue);
        shotAngleSlider.onValueChanged.AddListener(AngleSliderChanged);
    }

}
