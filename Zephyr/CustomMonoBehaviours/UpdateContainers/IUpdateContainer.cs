namespace Zephyr.CustomMonoBehaviours.UpdateContainers
{
    public interface IUpdateContainer
    {
        void Add(IUpdateable updateable);
        bool Remove(IUpdateable updateable);
        void Update(float deltaTime);
        void OnDestroy();
    }
}