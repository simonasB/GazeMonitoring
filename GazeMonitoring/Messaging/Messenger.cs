using System;
using System.Collections.Generic;

namespace GazeMonitoring.Messaging
{
    public interface IMessenger
    {
        void Register<T>(Action<T> callback) where T : IMessage;
        void Send<T>(T message) where T : IMessage;
    }

    public class Messenger : IMessenger
    {
        private readonly Dictionary<Type, List<Action<object>>> _registry;

        public Messenger()
        {
            _registry = new Dictionary<Type, List<Action<object>>>();
        }

        public void Register<T>(Action<T> callback) where T : IMessage
        {
            var type = typeof(T);

            if (_registry.TryGetValue(type, out var callbacks))
            {
                callbacks.Add(o => callback((T)o));
                return;
            }

            _registry[type] = new List<Action<object>> { o => callback((T)o) };
        }

        public void Send<T>(T message) where T : IMessage
        {
            if (!_registry.TryGetValue(typeof(T), out var callbacks))
            {
                throw new ArgumentException($"Message type '{typeof(T).Name}' is not registered.");
            }

            callbacks.ForEach(o => o.Invoke(message));
        }
    }
}
