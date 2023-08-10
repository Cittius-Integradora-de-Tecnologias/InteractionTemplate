using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LiquidControl : MonoBehaviour
{
    //Material of liquid
    Material liquidMaterial;

    //Variables to control the process

    [Header("Settings of Liquid Behaviour")]
    [Tooltip("Transfer time of each mL value")]
    [SerializeField] private float timeToFill = 0.2f;

    private Coroutine routine;

    //LIQUID EFFET
    [Header("Liquid Animation")]
    [SerializeField] private Stream lineLiquid;
    [SerializeField] private Transform originLiquid;

    private float AnguloObjeto;
    [SerializeField] private float minAngle = 45;

    void Start()
    {
        liquidMaterial = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        AnguloObjeto = Vector3.Angle(transform.up, Vector3.up);

        if (AnguloObjeto >= minAngle)
        {
            if (lineLiquid.gameObject.activeSelf)
            {
                lineLiquid.Begin(originLiquid);
            }
            else
            {
                lineLiquid.gameObject.SetActive(true);
            }
        }
        else
        {
            if (lineLiquid.gameObject.activeSelf)
            {
                lineLiquid.End();
            }
        }
    }

    public void ToFill(float transferenceAmount, float maxLimit)
    {
        float percentageTranfer = (transferenceAmount / maxLimit);
        float fill = liquidMaterial.GetFloat("_Fill");
        DOTween.To(() => fill, x => fill = x, fill + percentageTranfer, timeToFill).OnUpdate(() =>
        {
            if (fill < 1)
                liquidMaterial.SetFloat("_Fill", fill + percentageTranfer);
            else
                liquidMaterial.SetFloat("_Fill", 1);
        });
    }


    public void StartFill(float qtdMl, float totalMl)
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(StartFillInfinity(qtdMl, totalMl));
    }

    bool running;
    private IEnumerator StartFillInfinity(float qtdMl, float totalMl)
    {
        // Set the function as running
        running = true;

        // Do the job until running is set to false
        while (running)
        {
            if (liquidMaterial.GetFloat("_Fill") < 1)
                ToFill(qtdMl, totalMl);
            else
                liquidMaterial.SetFloat("_Fill", 1);
            yield return new WaitForSeconds(timeToFill);
        }
    }

    public void Drain(float transferenceAmount, float maxLimit)
    {
        float percentageTranfer = (transferenceAmount / maxLimit);
        float fill = liquidMaterial.GetFloat("_Fill");
        DOTween.To(() => fill, x => fill = x, fill - percentageTranfer, timeToFill).OnUpdate(() =>
        {
            if (fill > 0)
                liquidMaterial.SetFloat("_Fill", fill - percentageTranfer);
            else
                liquidMaterial.SetFloat("_Fill", 0);
        });
    }

    public void StartDrain(float qtdMl, float totalMl)
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(StartDrainInfinity(qtdMl, totalMl));
    }
    private IEnumerator StartDrainInfinity(float qtdMl, float totalMl)
    {
        // Set the function as running
        running = true;

        // Do the job until running is set to false
        while (running)
        {
            if (liquidMaterial.GetFloat("_Fill") > 0)
                Drain(qtdMl, totalMl);
            else
                liquidMaterial.SetFloat("_Fill", 0);
            yield return new WaitForSeconds(timeToFill);
        }
    }

    public void StopFill()
    {
        if (routine != null)
            StopCoroutine(routine);
    }

}
