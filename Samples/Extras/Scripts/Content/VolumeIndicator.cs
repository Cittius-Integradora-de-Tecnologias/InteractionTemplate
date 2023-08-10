using Cittius.Interaction;
using TMPro;
using UnityEngine;

/// <summary>
/// This component will indicate how much content the indicated recipient have stored
/// </summary>
public class VolumeIndicator : MonoBehaviour
{
    [SerializeField] private Recipient recipient;
    [SerializeField] private TMP_Text mesurementText;
    [SerializeField] private Vector3 mesurementPos = Vector3.up;
    [SerializeField] private string measurementUnit = "ml";

    private void Update()
    {
        this.transform.position = mesurementPos + recipient.transform.position;
        this.transform.eulerAngles = Vector3.forward;
        UpdateMesurement(recipient, measurementUnit);
    }

    private void UpdateMesurement(Recipient recipient, string measurementUnit)
    {
        mesurementText.text = recipient.GetQuantity().ToString() + measurementUnit;
    }
}
