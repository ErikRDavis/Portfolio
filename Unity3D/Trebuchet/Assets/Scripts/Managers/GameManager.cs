using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Trebuchet trebuchet;
    [SerializeField]
    private TrebuchetData trebuchetData;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private CameraViewManager cameraViewManager;
    [SerializeField]
    private MunitionManager munitionManager;

    // Start is called before the first frame update
    void Start()
    {
        RegisterEventHandlers();

        InitializeTrebuchet();
    }

    private void RegisterEventHandlers()
    {
        uiController.onAngleSliderChanged += UI_onAngleSliderChanged;
        player.onChangeCameraView += Player_onChangeCameraView;
        player.onRotateCamera += Player_onRotateCamera;
        player.onToggleMenu += Player_onToggleMenu;
        trebuchet.onReleaseAngleChanged += Trebuchet_onReleaseAngleChanged;
        munitionManager.onMunitionSpawned += MunitionManager_onMunitionSpawned;
        munitionManager.onMunitionDespawned += MunitionManager_onMunitionDespawned;
    }

    private void UI_onAngleSliderChanged(float percentage, float min, float max)
    {
        float remapped = Utility.Remap(percentage, min, max, trebuchetData.minReleaseAngle, trebuchetData.maxReleaseAngle);

        SetTrebuchetReleaseAngle(remapped);
    }

    private void SetTrebuchetReleaseAngle(float angle)
    {
        trebuchet.SetMunitionReleaseAngle(angle);
        SetUITrebuchetReleaseAngle(angle);
    }

    private void SetUITrebuchetReleaseAngle(float angle)
    {
        uiController.TrebuchetReleaseAngleUpdated(angle, trebuchetData.minReleaseAngle, trebuchetData.maxReleaseAngle);
    }

    private void Player_onChangeCameraView(int value)
    {
        cameraViewManager.SwitchCameraView(value);
    }

    private void Player_onRotateCamera(Vector2 value)
    {
        cameraViewManager.RotateCamera(value);
    }

    private void Player_onToggleMenu()
    {
        uiController.ToggleMenu();
    }

    private void Trebuchet_onReleaseAngleChanged(float angle)
    {
        SetUITrebuchetReleaseAngle(angle);
    }

    private void MunitionManager_onMunitionSpawned(Munition munition)
    {
        cameraViewManager.AddCameraViewTarget(munition.transform, new Vector3(), new Vector3(0,0,-15));
    }

    private void MunitionManager_onMunitionDespawned(Munition munition)
    {
        cameraViewManager.RemoveCameraViewTarget(munition.transform);
    }

    private void InitializeTrebuchet()
    {
        SetTrebuchetReleaseAngle(trebuchetData.defaultReleaseAngle);

        trebuchet.SetData(trebuchetData);

        if (trebuchetData.autoRearm)
        {
            trebuchet.RearmTrebuchet();
        }
    }
}
