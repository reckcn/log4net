#region Copyright & License
//
// Copyright 2001-2004 The Apache Software Foundation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Diagnostics;
using System.Globalization;

using log4net.Config;
using log4net.Util;
using log4net.Layout;
using log4net.Core;
using log4net.Appender;
using log4net.Repository;

using log4net.Tests.Appender;

using NUnit.Framework;

namespace log4net.Tests.Core
{
	/// <summary>
	/// </remarks>
	[TestFixture] public class ShutdownTest
	{
		/// <summary>
		/// Test that a repository can be shutdown and reconfigured
		/// </summary>
		[Test] public void TestShutdownAndReconfigure()
		{
			// Create unique repository
			ILoggerRepository rep = LogManager.CreateRepository(Guid.NewGuid().ToString());

			// Create appender and configure repos
			StringAppender stringAppender = new StringAppender();
			stringAppender.Layout = new PatternLayout("%m");
			BasicConfigurator.Configure(rep, stringAppender);

			// Get logger from repos
			ILog log1 = LogManager.GetLogger(rep.Name, "logger1");

			log1.Info("TestMessage1");
			Assertion.AssertEquals("Test logging configured", "TestMessage1", stringAppender.GetString());
			stringAppender.Reset();

			rep.Shutdown();

			log1.Info("TestMessage2");
			Assertion.AssertEquals("Test not logging while shutdown", "", stringAppender.GetString());
			stringAppender.Reset();

			// Create new appender and configure
			stringAppender = new StringAppender();
			stringAppender.Layout = new PatternLayout("%m");
			BasicConfigurator.Configure(rep, stringAppender);

			log1.Info("TestMessage3");
			Assertion.AssertEquals("Test logging re-configured", "TestMessage3", stringAppender.GetString());
			stringAppender.Reset();
		}
	}
}