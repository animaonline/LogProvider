using System;
using System.Diagnostics;
using Animaonline.Utils.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class LogProviderTests
    {
        [TestMethod]
        public void TestLogProviderInit()
        {
            var logReceiver = new LogProvider.LogReceiver(OnLogProviderReceive);

            Assert.IsNotNull(logReceiver);

            var logProvider = new LogProvider(logReceiver);

            Assert.IsNotNull(logProvider);

            logProvider.Info("Hello, World!");
        }

        private void OnLogProviderReceive(LogProvider.LogEntry logEntry)
        {
            Assert.IsNotNull(logEntry);
            
            Debug.WriteLine(logEntry);
        }
    }
}
