using System;
using System.Collections.Generic;
using FluentAssertions;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Messaging
{
    [TestFixture]
    public class MessengerTests_Unit
    {
        [Test]
        public void Ctor_NewRegistryShouldBeInitialized()
        {
            var messenger = new Messenger();

            var registry = messenger.GetMemberValue<Dictionary<Type, List<Action<object>>>>("_registry");

            registry.Should().NotBeNull();
            registry.Should().HaveCount(0);
        }

        [Test]
        public void Register_ShouldAddNewEntryToRegistryForNewType()
        {
            var messenger = new Messenger();

            var action = new Action<object>(o => { });

            messenger.Register<IMessage>(action);

            var registry = messenger.GetMemberValue<Dictionary<Type, List<Action<object>>>>("_registry");
            registry.Should().HaveCount(1);
            registry.Should().ContainKey(typeof(IMessage))
                .WhichValue.Should().HaveCount(1);
        }

        [Test]
        public void Register_ShouldAddEntriesOfEqualTypeToSameList()
        {
            var messenger = new Messenger();

            var action = new Action<object>(o => { });
            var action2 = new Action<object>(o => { });

            messenger.Register<IMessage>(action);
            messenger.Register<IMessage>(action2);

            var registry = messenger.GetMemberValue<Dictionary<Type, List<Action<object>>>>("_registry");
            registry.Should().HaveCount(1);
            registry.Should().ContainKey(typeof(IMessage))
                .WhichValue.Should().HaveCount(2);
        }

        [Test]
        public void Register_ShouldAddMultipleEntriesforDifferentTypes()
        {
            var messenger = new Messenger();

            var action = new Action<object>(o => { });
            var action2 = new Action<object>(o => { });

            messenger.Register<IMessage>(action);
            messenger.Register<ShowMainNavigationMessage>(action2);

            var registry = messenger.GetMemberValue<Dictionary<Type, List<Action<object>>>>("_registry");
            registry.Should().HaveCount(2);
            registry.Should().ContainKey(typeof(IMessage))
                .WhichValue.Should().HaveCount(1);
        }

        [Test]
        public void Send_ShouldThrowExceptionWhenMessageIsNotRegistered()
        {
            var messenger = new Messenger();

            var action = new Action<object>(o => { });
            messenger.Register<IMessage>(action);
            Action sendAction = () => messenger.Send(new ShowEditScreenConfigurationMessage());
            sendAction.Should().Throw<ArgumentException>().WithMessage($"Message type '{nameof(ShowEditScreenConfigurationMessage)}' is not registered.");

            var registry = messenger.GetMemberValue<Dictionary<Type, List<Action<object>>>>("_registry");
            registry.Should().HaveCount(1);
        }

        [Test]
        public void Send_ShouldInvokeAllRegisteredFunctionsOfSpecifiedType()
        {
            var messenger = new Messenger();

            var action = new Mock<Action<object>>();
            var action2 = new Mock<Action<object>>();
            messenger.Register<ShowEditScreenConfigurationMessage>(action.Object);
            messenger.Register<ShowEditScreenConfigurationMessage>(action2.Object);
            
            messenger.Send(new ShowEditScreenConfigurationMessage());

            action.Verify(o => o.Invoke(It.IsAny<object>()), Times.Once);
            action2.Verify(o => o.Invoke(It.IsAny<object>()), Times.Once);
        }
    }
}