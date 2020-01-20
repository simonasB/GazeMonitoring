using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using FluentAssertions;
using GazeMonitoring.DataAccess;
using GazeMonitoring.HotKeys.Global;
using GazeMonitoring.HotKeys.Global.Handlers;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.HotKeys
{
    [TestFixture(Category = TestCategory.UNIT)]
    public class GlobalHotKeyManagerTests_Unit
    {
        private Mock<IConfigurationRepository> _configurationRepository;
        private Mock<IGlobalHotKeyHandlerFactory> _globalHotKeyHandlerFactory;
        private GlobalHotKeyManager _globalHotKeyManager;

        [SetUp]
        public void SetUp()
        {
            _configurationRepository = new Mock<IConfigurationRepository>();
            _globalHotKeyHandlerFactory = new Mock<IGlobalHotKeyHandlerFactory>();
            _globalHotKeyManager = new GlobalHotKeyManager(_globalHotKeyHandlerFactory.Object, _configurationRepository.Object);
            var localGlobalKey = new GlobalHotKey(Key.A, ModifierKeys.Alt, () => {}, false);
            localGlobalKey.WithMemberValue("_dictHotKeyToCalBackProc", new Dictionary<int, GlobalHotKey>());
        }

        [Test]
        public void Ctor_ShouldThrowExceptionWhenGlobalHotKeyHandlerFactoryIsNull()
        {
            Action action = () => new GlobalHotKeyManager(null, _configurationRepository.Object);

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: globalHotKeyHandlerFactory");
        }

        [Test]
        public void Ctor_ShouldThrowExceptionWhenConfigurationRepositoryIsNull()
        {
            Action action = () => new GlobalHotKeyManager(_globalHotKeyHandlerFactory.Object, null);

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: configurationRepository");
        }
        
        [Test]
        public void Ctor_ShouldInitializeGlobalHotKeysFromRepository()
        {
            // Arrange
            MockGlobalKeys();

            // Act
            _globalHotKeyManager = new GlobalHotKeyManager(_globalHotKeyHandlerFactory.Object, _configurationRepository.Object);

            // Assert
            var globalHotKeys = _globalHotKeyManager.GetMemberValue<Dictionary<EGlobalHotKey, GlobalHotKey>>("_globalHotKeys");
            globalHotKeys.Should().NotBeNull();
            globalHotKeys.Should().HaveCount(2);

            _globalHotKeyHandlerFactory.Verify(o => o.Create(EGlobalHotKey.CreateScreenConfiguration), Times.Once);
            _globalHotKeyHandlerFactory.Verify(o => o.Create(EGlobalHotKey.EditScreenConfiguration), Times.Once);

            globalHotKeys.Should().ContainKey(EGlobalHotKey.CreateScreenConfiguration);
            globalHotKeys.Should().ContainKey(EGlobalHotKey.EditScreenConfiguration);

            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].Handler.Should().NotBeNull();
            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].Key.Should().Be(Key.A);
            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].KeyModifiers.Should().Be(ModifierKeys.Alt);

            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].Handler.Should().NotBeNull();
            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].Key.Should().Be(Key.T);
            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].KeyModifiers.Should().Be(ModifierKeys.Control);
        }

        [Test]
        public void Change_ShouldThrowExceptionWhenKeyIsNotRegistered()
        {
            Action action = () => _globalHotKeyManager.Change(EGlobalHotKey.CreateScreenConfiguration, Key.A, ModifierKeys.Alt);

            action.Should().Throw<ArgumentException>().WithMessage("Global key not registered\r\nParameter name: eGlobalHotKey");
        }

        [Test]
        public void Change_ShouldCallRepostoryUpdate()
        {
            MockGlobalKeys();

            _globalHotKeyManager = new GlobalHotKeyManager(_globalHotKeyHandlerFactory.Object, _configurationRepository.Object);
            _globalHotKeyManager.Change(EGlobalHotKey.CreateScreenConfiguration, Key.D, ModifierKeys.Shift);

            _configurationRepository.Verify(o => o.Update(It.IsAny<GlobalHotKeyEntity>()), Times.Once);
        }

        [Test]
        public void Change_ShouldWorkCorrectlyWhenKeyDoesNotExistInRepository()
        {
            MockGlobalKeys();

            _globalHotKeyManager = new GlobalHotKeyManager(_globalHotKeyHandlerFactory.Object, _configurationRepository.Object);
            _globalHotKeyManager.Change(EGlobalHotKey.CreateScreenConfiguration, Key.D, ModifierKeys.Shift);

            _configurationRepository.Verify(o => o.Update(It.IsAny<GlobalHotKeyEntity>()), Times.Once);
            _configurationRepository.Verify(o => o.Update(It.Is<GlobalHotKeyEntity>(
                x => x.EGlobalHotKey == EGlobalHotKey.CreateScreenConfiguration && x.Key == Key.D && x.KeyModifiers == ModifierKeys.Shift)), Times.Once);

            var globalHotKeys = _globalHotKeyManager.GetMemberValue<Dictionary<EGlobalHotKey, GlobalHotKey>>("_globalHotKeys");
            globalHotKeys.Should().NotBeNull();
            globalHotKeys.Should().HaveCount(2);

            globalHotKeys.Should().ContainKey(EGlobalHotKey.CreateScreenConfiguration);
            globalHotKeys.Should().ContainKey(EGlobalHotKey.EditScreenConfiguration);

            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].Handler.Should().NotBeNull();
            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].Key.Should().Be(Key.D);
            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].KeyModifiers.Should().Be(ModifierKeys.Shift);

            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].Handler.Should().NotBeNull();
            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].Key.Should().Be(Key.T);
            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].KeyModifiers.Should().Be(ModifierKeys.Control);
        }

        [Test]
        public void Change_ShouldWorkCorrectlyWhenKeyExistsInRepository()
        {
            // Arrange
            MockGlobalKeys();
            _configurationRepository.Setup(o => o.SearchOne(It.IsAny<Expression<Func<GlobalHotKeyEntity, bool>>>())).Returns(new GlobalHotKeyEntity
            {
                EGlobalHotKey = EGlobalHotKey.CreateScreenConfiguration,
                Id = 1,
                Key = Key.A,
                KeyModifiers = ModifierKeys.Alt
            });

            // Act
            _globalHotKeyManager = new GlobalHotKeyManager(_globalHotKeyHandlerFactory.Object, _configurationRepository.Object);
            _globalHotKeyManager.Change(EGlobalHotKey.CreateScreenConfiguration, Key.D, ModifierKeys.Shift);

            // Assert
            _configurationRepository.Verify(o => o.Update(It.IsAny<GlobalHotKeyEntity>()), Times.Once);
            _configurationRepository.Verify(o => o.Update(It.Is<GlobalHotKeyEntity>(
                x => x.EGlobalHotKey == EGlobalHotKey.CreateScreenConfiguration && x.Key == Key.D && x.KeyModifiers == ModifierKeys.Shift)), Times.Once);

            _configurationRepository.Verify(o => o.SearchOne(It.IsAny<Expression<Func<GlobalHotKeyEntity, bool>>>()), Times.Once);

            var globalHotKeys = _globalHotKeyManager.GetMemberValue<Dictionary<EGlobalHotKey, GlobalHotKey>>("_globalHotKeys");
            globalHotKeys.Should().NotBeNull();
            globalHotKeys.Should().HaveCount(2);

            globalHotKeys.Should().ContainKey(EGlobalHotKey.CreateScreenConfiguration);
            globalHotKeys.Should().ContainKey(EGlobalHotKey.EditScreenConfiguration);

            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].Handler.Should().NotBeNull();
            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].Key.Should().Be(Key.D);
            globalHotKeys[EGlobalHotKey.CreateScreenConfiguration].KeyModifiers.Should().Be(ModifierKeys.Shift);

            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].Handler.Should().NotBeNull();
            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].Key.Should().Be(Key.T);
            globalHotKeys[EGlobalHotKey.EditScreenConfiguration].KeyModifiers.Should().Be(ModifierKeys.Control);
        }

        private void MockGlobalKeys()
        {
            var globalHotKeyEntities = new List<GlobalHotKeyEntity>
            {
                new GlobalHotKeyEntity
                {
                    EGlobalHotKey = EGlobalHotKey.CreateScreenConfiguration,
                    Id = 1,
                    Key = Key.A,
                    KeyModifiers = ModifierKeys.Alt
                },
                new GlobalHotKeyEntity
                {
                    EGlobalHotKey = EGlobalHotKey.EditScreenConfiguration,
                    Id = 2,
                    Key = Key.T,
                    KeyModifiers = ModifierKeys.Control
                }
            };
            _configurationRepository.Setup(o => o.Search<GlobalHotKeyEntity>()).Returns(globalHotKeyEntities);
            var mockHandler = new Mock<IGlobalHotKeyHandler>().Object;
            var mockHandler2 = new Mock<IGlobalHotKeyHandler>().Object;
            _globalHotKeyHandlerFactory.Setup(o => o.Create(EGlobalHotKey.CreateScreenConfiguration)).Returns(mockHandler);
            _globalHotKeyHandlerFactory.Setup(o => o.Create(EGlobalHotKey.EditScreenConfiguration)).Returns(mockHandler2);
        }
    }
}
