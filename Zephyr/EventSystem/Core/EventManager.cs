using UnityEngine;

namespace Zephyr.EventSystem.Core
{
    public class EventManager : MonoBehaviour
    {
        public bool LimitQueueProcesing = false;
        public float QueueProcessTime = 0.0f;

        private static EventManagerModel _instance;


        public static EventManagerModel Instance
        {
            get { return _instance ?? (_instance = new EventManagerModel()); }
        }

        public void OnDestroy()
        {
            if (_instance == null) return;

            _instance.ClearAll();
            _instance = null;
        }

        private void Update()
        {
            Instance.ProcessEvents();
        }
    }
}