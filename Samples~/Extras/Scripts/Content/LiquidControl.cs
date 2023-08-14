using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Cittius.Interaction.Extras
{

    [RequireComponent(typeof(Recipient))]
    public class LiquidControl : MonoBehaviour
    {
        //Material of liquid
        Material liquidMaterial;

        //Variables to control the process

        [Header("Settings of Liquid Behaviour")]
        [Tooltip("Transfer time of each mL value")]
        private Recipient recipient;

        private Coroutine routine;

        //LIQUID EFFET
        [Header("Liquid Animation")]
        [SerializeField] private Stream m_lineLiquid;
        public Stream lineLiquid { get => m_lineLiquid; }
        [SerializeField] private Transform originLiquid;

        private float AnguloObjeto;
        [SerializeField] private float minAngle = 45;

        void Start()
        {
            recipient = GetComponent<Recipient>();
            liquidMaterial = GetComponent<MeshRenderer>().material;
            recipient.onStartTranference.AddListener((arg) =>
            {
                //arg.filled StartFill(recipient.transferenceAmount, recipient.maxLimite, recipient.transferenceDelay);
                if (arg.filled.TryGetComponent(out LiquidControl liquidControl))
                {
                    liquidControl.StartFill(arg.filled.transferenceAmount, arg.filled.maxLimite, arg.filled.transferenceDelay);
                }
                if (arg.drained.TryGetComponent(out liquidControl))
                {
                    liquidControl.StartDrain(arg.drained.transferenceAmount, arg.drained.maxLimite, arg.drained.transferenceDelay);
                }

            });
            recipient.onStopTranference.AddListener((arg) =>
            {
                StopFill();
            });

            liquidMaterial.SetFloat("_Fill", (float)recipient.GetQuantity() / (float)recipient.maxLimite);

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

        public void ToFill(float transferenceAmount, float maxLimit, float timeToFill)
        {
            float percentageTranfer = (transferenceAmount / maxLimit);
            float fill = liquidMaterial.GetFloat("_Fill");
            DOTween.To(() => fill, x => fill = x, fill + percentageTranfer, timeToFill).OnUpdate(() =>
            {
                if (fill < 1)
                    liquidMaterial.SetFloat("_Fill", fill + percentageTranfer);
                else
                {
                    liquidMaterial.SetFloat("_Fill", 1);
                    StopFill();
                }
            });
        }


        public void StartFill(float qtdMl, float totalMl, float timeToFill)
        {
            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(StartFillInfinity(qtdMl, totalMl, timeToFill));
        }

        bool running;
        private IEnumerator StartFillInfinity(float qtdMl, float totalMl, float timeToFill)
        {
            // Set the function as running
            running = true;

            // Do the job until running is set to false
            while (running)
            {
                if (liquidMaterial.GetFloat("_Fill") < 1)
                    ToFill(qtdMl, totalMl, timeToFill);
                else
                    liquidMaterial.SetFloat("_Fill", 1);
                yield return new WaitForSeconds(timeToFill);
            }
        }

        public void Drain(float transferenceAmount, float maxLimit, float timeToFill)
        {
            float percentageTranfer = (transferenceAmount / maxLimit);
            float fill = liquidMaterial.GetFloat("_Fill");
            DOTween.To(() => fill, x => fill = x, fill - percentageTranfer, timeToFill).OnUpdate(() =>
            {
                if (fill > 0)
                    liquidMaterial.SetFloat("_Fill", fill - percentageTranfer);
                else
                {
                    liquidMaterial.SetFloat("_Fill", 0);
                    StopFill();
                }
            });
        }

        public void StartDrain(float qtdMl, float totalMl, float timeToFill)
        {
            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(StartDrainInfinity(qtdMl, totalMl, timeToFill));
        }
        private IEnumerator StartDrainInfinity(float qtdMl, float totalMl, float timeToFill)
        {
            // Set the function as running
            running = true;

            // Do the job until running is set to false
            while (running)
            {
                if (liquidMaterial.GetFloat("_Fill") > 0)
                    Drain(qtdMl, totalMl, timeToFill);
                else
                    liquidMaterial.SetFloat("_Fill", 0);
                yield return new WaitForSeconds(timeToFill);
            }
        }

        public void StopFill()
        {
            if (routine != null)
                StopCoroutine(routine);

            if (lineLiquid.gameObject.activeSelf)
            {
                lineLiquid.End();
            }
        }

    }

}