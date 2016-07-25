namespace Zephyr.CustomMonoBehaviours {
    public interface IUpdateable
    {
        void OnUpdate(float delta);

        void OnStart();

        void OnAwake();
    }
}
