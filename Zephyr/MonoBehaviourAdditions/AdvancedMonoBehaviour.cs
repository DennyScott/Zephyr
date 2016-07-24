using UnityEngine;

namespace Zephyr.MonoBehaviourAdditions
{
    public class AdvancedMonoBehaviour : MonoBehaviour, IUpdateable
    {
        /// <summary>
        /// Runs on Start for all objects.
        /// </summary>
        private void Awake()
        {
            AdvancedMonoBehvaiourRunner.Instance.RegisterUpdateableObject(this);
        }

        /// <summary>
        /// Runs when the gameobject or this component is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (AdvancedMonoBehvaiourRunner.IsAlive)
                AdvancedMonoBehvaiourRunner.Instance.UnregisterUpdateableObject(this);
        }

        /// <summary>
        /// Used to enter the update loop.  Anything within this method will be called once a frame.
        /// </summary>
        /// <param name="delta"></param>
        public virtual void OnUpdate(float delta) { }

        /// <summary>
        /// Must be used instead of Start in an AdvancedMonobehaviour.  Acts in the place of start.
        /// </summary>
        public virtual void OnStart() { }

        /// <summary>
        /// Must be used instead of Awake in an AdvancedMonobehaviour.  Acts in the place of start.
        /// </summary>
        public virtual void OnAwake() { }
    }
}
