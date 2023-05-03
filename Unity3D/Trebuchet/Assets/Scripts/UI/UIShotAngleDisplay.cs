using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShotAngleDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI degreeLabel;
    [SerializeField]
    private RectTransform angleIndicator;

    public void SetAngleValue(float angle)
    {
        degreeLabel.text = $"{angle.ToString("0.0")}°";

        angleIndicator.rotation = Quaternion.AngleAxis(-angle, angleIndicator.forward);
    }
}
