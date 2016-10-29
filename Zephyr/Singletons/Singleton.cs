namespace Zephyr.Singletons
{
    /// <summary>
    /// A regular singleton class for a non monobehaviour class
    /// </summary>
    /// <typeparam name="T">The class to be a singleton</typeparam>
    public class Singleton<T> where T : Singleton<T>, new()
    {
        protected static T InnerInstance;

        /// <summary>
        /// Returns the instance of this singleton.
        /// </summary>
        public static T Instance => InnerInstance ?? (InnerInstance = new T());
    }
}