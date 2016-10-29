using UnityEngine;

namespace Zephyr.Singletons
{
    /// <summary>
    /// A simple singleton.  This singleton will not exist through scenes, and will not create itself if it does not exist.
    /// </summary>
    /// <typeparam name="T">The type of singleton</typeparam>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        protected static T InnerInstance;

        /// <summary>
        /// Returns the instance of this singleton.
        /// </summary>
        public static T Instance => InnerInstance ?? (InnerInstance = (T) FindObjectOfType(typeof(T)));
    }
}