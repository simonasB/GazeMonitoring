﻿using System;

namespace GazeMonitoring.Data.Xml {
    public class XmlFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            if (fileName == null) {
                throw new ArgumentNullException(nameof(fileName));
            }

            return $"{fileName}.xml";
        }
    }
}
