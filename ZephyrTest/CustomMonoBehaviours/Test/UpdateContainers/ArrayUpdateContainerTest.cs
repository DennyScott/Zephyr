using NUnit.Framework;
using NSubstitute;
using Zephyr.CustomMonoBehaviours;
using Zephyr.CustomMonoBehaviours.UpdateContainers;

namespace ZephyrTest.CustomMonoBehaviours.Test.UpdateContainers
{
    [TestFixture]
    public class ArrayUpdateContainerTest
    {
        private ArrayUpdateContainer _updateContainer;
        private const float DeltaTime = 0.1f;

        [SetUp]
        public void Init()
        {
            _updateContainer = new ArrayUpdateContainer();
        }

        [Test]
        public void DoesUpdateForObjectGetCalledOnUpdate()
        {
            //Arrange
            var updateable = GetUpdateableMock();
            
            _updateContainer.Add(updateable);
            
            //Assert
            _updateContainer.Update(DeltaTime);

            //Act
            updateable.Received(1).OnUpdate(DeltaTime);
        }

        [Test]
        public void DoeAddUpdateableIncreaseCountByOne()
        {
            //Arrange
            var updateable = GetUpdateableMock();
            var initalCount = _updateContainer.Count;

            //Assert
            _updateContainer.Add(updateable);
            var currentSize = _updateContainer.Count;

            //Act
            Assert.That(currentSize, Is.EqualTo(initalCount + 1));
        }

        [Test]
        public void DoesRemoveUpdateableDecreaseCountByOne()
        {
            //Arrange
            var updateable = GetUpdateableMock();
            _updateContainer.Add(updateable);
            var initalCount = _updateContainer.Count;

            //Assert
            _updateContainer.Remove(updateable);
            var currentSize = _updateContainer.Count;

            //Act
            Assert.That(currentSize, Is.EqualTo(initalCount - 1));
        }

        private IUpdateable GetUpdateableMock()
        {
            return Substitute.For<IUpdateable>();
        }
    }
}