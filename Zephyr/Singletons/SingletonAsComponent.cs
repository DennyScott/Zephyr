using UnityEngine;

namespace Zephyr.Singletons
{
    /// <summary>
    /// A SingletonMonoBehaviour class that can be extended from.  If an instance already exists within the scene, it will be used.  If multiple exist, extras will be deleted.
    /// Lastly, if it does not exist, it will create a new component that will not be destroyed on loads.  This is useful for a Game Runner.
    /// </summary>
    /// <typeparam name="T">The type of component to be a singleton</typeparam>
    public class SingletonAsComponent<T> : MonoBehaviour where T : SingletonAsComponent<T>
    {
        private bool _isAlive = true;
        private static T _instance;

        /// <summary>
        /// Gets the instance of this class.
        /// </summary>
        public static SingletonAsComponent<T> Instance
        {
            get
            {
                if (_instance) return _instance;

                var managers = FindObjectsOfType<T>();

                if (managers?.Length == 1)
                {
                    _instance = managers[0];
                    return _instance;
                }

                if (managers?.Length > 1)
                {
                    Debug.LogError("You have more then one " + typeof(T).Name +
                                   " in the scene.  You only need 1, it's a singleton!");

                    foreach (var manager in managers)
                    {
                        Destroy(manager.gameObject);
                    }
                }

                var go = new GameObject(typeof(T).Name, typeof(T));
                _instance = go.GetComponent<T>();
                DontDestroyOnLoad(_instance.gameObject);
                return _instance;
            }

            set { _instance = value as T; }
        }

        /// <summary>
        /// Returns true if this singleton is still alive.
        /// </summary>
        public static bool IsAlive => _instance != null && _instance._isAlive;

        /// <summary>
        /// On Destroy, this will set the property IsAlive to false.
        /// </summary>
        private void OnDestroy() => _isAlive = false;

        /// <summary>
        /// On Quit, this will set the property IsAlive to false.
        /// </summary>
        private void OnApplicationQuit() => _isAlive = false;
    }
}