using UnityEngine;
using UnityEngine.Events;

namespace HotQueen.Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : InteractBase
    {
        private Rigidbody rb;
        public UnityEvent<GrabArg> OnGrabbed;
        public UnityEvent<GrabArg> OnDropped;
        private Transform attach;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            interacted += (ctx) =>
            {
                if (ctx.interactor.TryGetComponent<Interactor>(out Interactor grabber))
                {
                    Grab(new GrabArg(grabber, this));
                }
            };

            stoppedInteraction += (ctx) =>
            {
                if (ctx.interactor.TryGetComponent<Interactor>(out Interactor grabber))
                {
                    Drop(new GrabArg(grabber, this));
                }
            };
        }

        private void Grab(GrabArg args)
        {
            attach = args.grabber.attach;
            rb.isKinematic = true;
            OnGrabbed?.Invoke(args);
        }
        private void Drop(GrabArg args)
        {
            attach = null;
            rb.isKinematic = false;
            OnDropped?.Invoke(args);
        }
        public void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (attach)
            {
                this.transform.position = attach.position;
                this.transform.rotation = attach.rotation;
            }
        }
    }
}
