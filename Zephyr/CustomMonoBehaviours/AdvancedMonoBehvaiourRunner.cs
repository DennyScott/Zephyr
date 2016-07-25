using UnityEngine;
using Zephyr.CustomMonoBehaviours.UpdateContainers;
using Zephyr.Singletons;

namespace Zephyr.CustomMonoBehaviours
{
    /// <summary>
    /// In charge of running the update loop.  This update loop is more efficient then the unity built update loop.  Later this class should be extended to include high priority update items adnd such to cusomize the level of priority
    /// items in the update loop have.
    /// </summary>
    public class AdvancedMonoBehvaiourRunner : SingletonAsComponent<AdvancedMonoBehvaiourRunner>
    {
        #region Private Attributes

        private IUpdateContainer _updateContainer = new ArrayUpdateContainer();

        #endregion

        #region Properties

        public static AdvancedMonoBehvaiourRunner Instance
        {
            get { return (AdvancedMonoBehvaiourRunner) _Instance; }
            set { _Instance = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers an updateable object into the Update loop
        /// </summary>
        /// <param name="obj">The object to register into the update loop</param>
        public void RegisterUpdateableObject(IUpdateable obj)
        {
            _updateContainer.Add(obj);
            obj.OnAwake();
            obj.OnStart();
        }

        /// <summary>
        /// Unregisters an update-able component from the update loop
        /// </summary>
        /// <param name="obj">The object to remove from the Update loop</param>
        public bool UnregisterUpdateableObject(IUpdateable obj)
        {
            return _updateContainer.Remove(obj);
        }


        /// <summary>
        /// Updates once a frame.
        /// </summary>
        public void Update()
        {
            _updateContainer.Update(Time.deltaTime);
        }

        #endregion

        #region Inherited Members

        /// <summary>
        /// Called when this object is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _updateContainer.OnDestroy();
            _updateContainer = null;
        }

        #endregion
    }
}