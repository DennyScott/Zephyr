namespace Zephyr.MonoBehaviourAdditions {
    public interface IUpdateable
    {
        void OnUpdate(float delta);

        void OnStart();

        void OnAwake();
    }
}
