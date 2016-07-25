using System.Collections.Generic;

namespace Zephyr.CustomMonoBehaviours.UpdateContainers
{
    public class ListUpdateContainer : IUpdateContainer
    {
        public int Count { get { return _updateableObjects.Count; } }
        private List<IUpdateable> _updateableObjects = new List<IUpdateable>();

        public void Add(IUpdateable updateable)
        {
            _updateableObjects.Add(updateable);
        }

        public bool Remove(IUpdateable updateable)
        {
            return _updateableObjects.Remove(updateable);
        }

        public void Update(float deltaTime)
        {
            for (var i = 0; i < _updateableObjects.Count; i++)
                if (_updateableObjects[i] != null)
                    _updateableObjects[i].OnUpdate(deltaTime);

        }

        public void OnDestroy()
        {
            _updateableObjects = null;
        }
    }
}