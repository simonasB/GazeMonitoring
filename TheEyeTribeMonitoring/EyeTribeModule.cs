﻿using Autofac;
using EyeTribe.ClientSdk;
using GazeMonitoring.Common;
using GazeMonitoring.EyeTracker.Core.Streams;

namespace TheEyeTribeMonitoring {
    public class EyeTribeModule : Module {
        protected override void Load(ContainerBuilder builder) {
            GazeManager.Instance.Activate(GazeManagerCore.ApiVersion.VERSION_1_0);
            builder.RegisterType<EyeTribeGazePointStreamFactory>().As<IGazePointStreamFactory>();
        }
    }
}
