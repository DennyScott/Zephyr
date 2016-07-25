﻿using System;

namespace Zephyr.CustomMonoBehaviours.UpdateContainers
{
    public class ArrayUpdateContainer : IUpdateContainer
    {
        public int Count { get; private set; }

        private const int UpdateableSize = 200;
        private const int UpdateableScaleFactor = 2;
        private const float CleanupThreshold = 0.10f;


        private IUpdateable[] _updateableObjects = new IUpdateable[UpdateableSize];
        private int _index;
        private int _cleanupAmount = (int) (UpdateableSize*CleanupThreshold);

        public void Add(IUpdateable updateable)
        {
            if (_index == _updateableObjects.Length - 1)
                ResizeArray();


            _updateableObjects[_index] = updateable;
            _index++;
            Count++;
        }

        private void ResizeArray()
        {
            Array.Resize(ref _updateableObjects, _updateableObjects.Length*UpdateableScaleFactor);
            _cleanupAmount = (int) (_updateableObjects.Length*CleanupThreshold);
        }

        public bool Remove(IUpdateable updateable)
        {
            for (var i = 0; i < _index; i++)
            {
                if (_updateableObjects[i] != updateable) continue;

                _updateableObjects[i] = null;
                Count--;
                return true;
            }
            return false;
        }

        public void Update(float deltaTime)
        {
            for (var i = 0; i < _updateableObjects.Length; i++)
                if (_updateableObjects[i] != null)
                    _updateableObjects[i].OnUpdate(deltaTime);

            CheckForCleanup();
        }

        public void OnDestroy()
        {
            _updateableObjects = null;
        }

        private void CheckForCleanup()
        {
            if (_index - Count >= _cleanupAmount)
                CleanUpUpdateables();
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
                if (_updateableObjects[i] == null)
                    return _index;

            return -1;
        }
    }
}