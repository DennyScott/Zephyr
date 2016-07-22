using System;
using UnityEngine;
using Zephyr.Singletons;

namespace Zephyr.MonoBehaviourAdditions
{
    /// <summary>
    /// In charge of running the update loop.  This update loop is more efficient then the unity built update loop.  Later this class should be extended to include high priority update items adnd such to cusomize the level of priority
    /// items in the update loop have.
    /// </summary>
    public class AdvanceMonoBehvaiourRunner : SingletonAsComponent<AdvanceMonoBehvaiourRunner>
    {
        #region Constants

        private const int UpdateableSize = 200;
        private const int UpdateableScaleFactor = 2;

        #endregion

        #region Private Attributes

        private IUpdateable[] _updateableObjects = new IUpdateable[UpdateableSize];
        private int _index;
        private float _timePassed;
        private bool _isNeedingCleanup;

        #endregion

        #region Properties

        public static AdvanceMonoBehvaiourRunner Instance
        {
            get { return (AdvanceMonoBehvaiourRunner) _Instance; }
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
            AddNewUpdateable(obj);
            obj.OnAwake();
            obj.OnStart();
        }

        /// <summary>
        /// Unregisters an update-able component from the update loop
        /// </summary>
        /// <param name="obj">The object to remove from the Update loop</param>
        public bool UnregisterUpdateableObject(IUpdateable obj)
        {
            for (var i = 0; i < _index; i++)
            {
                if (_updateableObjects[i] != obj) continue;

                _updateableObjects[i] = null;
                _isNeedingCleanup = true;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Updates once a frame.
        /// </summary>
        public void Update()
        {
            var delta = Time.deltaTime;
            for (var i = 0; i < Instance._updateableObjects.Length; i++)
                if (_updateableObjects[i] != null)
                    _updateableObjects[i].OnUpdate(delta);

            if (!_isNeedingCleanup) return;

            _timePassed += delta;

            if (_timePassed < 1) return;

            _timePassed = 0;
            CleanUpUpdateables();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds a new updateable object to the updateables list.  If needed, it will also increase the size
        /// of the updateables array.
        /// </summary>
        /// <param name="obj">The updatable object to add to the array.</param>
        private void AddNewUpdateable(IUpdateable obj)
        {
            if (_index == _updateableObjects.Length - 1)
                Array.Resize(ref _updateableObjects, _updateableObjects.Length*UpdateableScaleFactor);

            _updateableObjects[_index] = obj;
            _index++;
        }

        /// <summary>
        /// Cleans up the Updateables by removing the empty references and moving the objects up the array for faster
        /// traversal and setting the new index.
        /// </summary>
        private void CleanUpUpdateables()
        {
            var placementIndex = FindFirstNull();

            if (placementIndex == -1) return;

            ShiftDownUpdateables(ref placementIndex);

            ClearRemainingUpdateables(placementIndex + 1);

            _index = placementIndex;
        }

        /// <summary>
        /// Clears all positions between the new Index and old index as all items have been squeezed downwards in
        /// the array.
        /// </summary>
        /// <param name="newIndex">The newIndex to clear from</param>
        private void ClearRemainingUpdateables(int newIndex)
        {
            for (var i = newIndex; i < _index; i++)
                _updateableObjects[i] = null;
        }

        /// <summary>
        /// Shifts all elements down the array to be at the start for faster traversal and memory management
        /// </summary>
        /// <param name="placementIndex">The position to start the moving down from.</param>
        private void ShiftDownUpdateables(ref int placementIndex)
        {
            for (var i = placementIndex + 1; i < _index; i++)
            {
                if (_updateableObjects[i] == null)
                    continue;

                _updateableObjects[placementIndex] = _updateableObjects[i];
                placementIndex++;
            }
        }

        /// <summary>
        /// Finds the first null in the updateable array and returns the index.
        /// </summary>
        /// <returns>The first null position.  If not found, returns -1.</returns>
        private int FindFirstNull()
        {
            for (var i = 0; i < _index; i++)
            {
                if (_updateableObjects[i] == null)
                    return _index;
            }
            return -1;
        }

        #endregion

        #region Inherited Members

        /// <summary>
        /// Called when this object is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _updateableObjects = null;
        }

        #endregion
    }
}