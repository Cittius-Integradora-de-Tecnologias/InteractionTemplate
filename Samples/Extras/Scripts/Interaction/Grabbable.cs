using UnityEngine;
using UnityEngine.Events;

namespace Cittius.Interaction.Extras
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : Interactable
    {
        private Rigidbody rb;
        public UnityEvent<GrabArg> OnGrabbed;
        public UnityEvent<GrabArg> OnDropped;
        private Transform attach;

        private void Start()
        {
            base.Start();
            rb = GetComponent<Rigidbody>();
            interacted += (ctx) =>
            {
                    Grab(new GrabArg(ctx.interactor, this));
            };

            stoppedInteraction += (ctx) =>
            {
                    Drop(new GrabArg(ctx.interactor, this));
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
