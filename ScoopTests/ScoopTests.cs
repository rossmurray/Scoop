using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Rhino.Mocks;
using Scoop;

namespace ScoopTests
{
	[TestClass]
	public class ScoopTests
	{
		private static string SampleJson = "{\"Seconds\": 1099, \"Filename\": \"file/name/here\", \"Id\": \"bd5fc971-cea0-4c52-a27a-d4fb81fb75d9\"}";
		private static SampleClass SampleObject = new SampleClass
		{
			Filename = "file/name/here",
			Id = Guid.Parse("bd5fc971-cea0-4c52-a27a-d4fb81fb75d9"),
			Seconds = 1099
		};
		public class SampleClass
		{
			public int Seconds { get; set; }
			public string Filename { get; set; }
			public Guid Id { get; set; }
		}

		[TestMethod]
		public void CanConstructConfigProviderFromString()
		{
			var c = Scoop.Configuration.Provide<object>(@"C:/nope/does.not.exist");
			Assert.IsNotNull(c);
		}

		[TestMethod]
		public void CanConstructConfigProviderFromFileInfo()
		{
			var c = Scoop.Configuration.Provide<object>(new FileInfo(@"C:/nope/does.not.exist"));
			Assert.IsNotNull(c);
		}

		[TestMethod]
		public void ConfigurationReadThrowsOnNoFile()
		{
			var failed = false;
			try
			{
				var c = Scoop.Configuration.Read<object>(new FileInfo(@"C:/does.not.exist"));
			}
			catch (FileNotFoundException)
			{
				failed = true;
			}
			Assert.IsTrue(failed);
		}

		[TestMethod]
		public void ConfigurationReadThrowsOnNoDir()
		{
			var failed = false;
			try
			{
				var c = Scoop.Configuration.Read<object>(new FileInfo(@"C:/nope/does.not.exist"));
			}
			catch (DirectoryNotFoundException)
			{
				failed = true;
			}
			Assert.IsTrue(failed);
		}

		[TestMethod]
		public void ConfigWorkerCallsConfigReaderRead()
		{
			var stubReader = MockRepository.GenerateStub<IConfigReader>();
			var stubDeserializer = MockRepository.GenerateStub<IDeserializer>();
			var workerTest = new ConfigWorker<object>(stubReader, stubDeserializer);
			workerTest.Work();
			stubReader.AssertWasCalled(r => r.ReadConfig(), o => o.Repeat.Once());
		}

		[TestMethod]
		public void ConfigWorkerCallsDeserializerDeserialize()
		{
			var stubReader = MockRepository.GenerateStub<IConfigReader>();
			var stubDeserializer = MockRepository.GenerateStub<IDeserializer>();
			var workerTest = new ConfigWorker<SampleClass>(stubReader, stubDeserializer);
			workerTest.Work();
			stubDeserializer.AssertWasCalled(d => d.Deserialize<SampleClass>(Arg<string>.Is.Anything), o => o.Repeat.Once());
		}

		[TestMethod]
		public void BasicConfigProviderCallsWorkerWork()
		{
			var stubWorker = MockRepository.GenerateStub<IConfigWorker<SampleClass>>();
			var providerTest = new BasicConfigProvider<SampleClass>(stubWorker);
			providerTest.GetConfig();
			stubWorker.AssertWasCalled(w => w.Work());
		}

		[TestMethod]
		public void CachedConfigProviderCallsWorkerWork()
		{
			var stubWorker = MockRepository.GenerateStub<IConfigWorker<SampleClass>>();
			var providerTest = new CachingConfigProvider<SampleClass>(stubWorker);
			providerTest.GetConfig();
			stubWorker.AssertWasCalled(w => w.Work());
		}

		[TestMethod]
		public void CachingConfigProviderThrowsOnBadPath()
		{
			var provider = new CachingConfigProvider<object>(new FileInfo(@"bad/path/"));
			var failed = false;
			try
			{
				var config = provider.GetConfig();
			}
			catch(DirectoryNotFoundException)
			{
				failed = true;
			}
			Assert.IsTrue(failed);
		}

		[TestMethod]
		public void CachingConfigProviderThrowsOnFileNotExist()
		{
			var provider = new CachingConfigProvider<object>(new FileInfo(@"C:/does.not.exist"));
			var failed = false;
			try
			{
				var config = provider.GetConfig();
			}
			catch (FileNotFoundException)
			{
				failed = true;
			}
			Assert.IsTrue(failed);
		}

		[TestMethod]
		public void JsonDeserializerGivesExpectedObject()
		{
			var configText = ScoopTests.SampleJson;
			var deserializerTest = new JsonDeserializer();
			var objResult = deserializerTest.Deserialize<SampleClass>(configText);
			Assert.AreEqual(objResult.Filename, SampleObject.Filename);
			Assert.AreEqual(objResult.Id, SampleObject.Id);
			Assert.AreEqual(objResult.Seconds, SampleObject.Seconds);
		}

		[TestMethod]
		public void JsonDeserializerGivesNotNull()
		{
			var configText = ScoopTests.SampleJson;
			var deserializerTest = new JsonDeserializer();
			var objResult = deserializerTest.Deserialize<SampleClass>(configText);
			Assert.IsNotNull(objResult);
		}

		[TestMethod]
		public void BasicConfigProviderCallsWorkForEachConfig()
		{
			var workerStub = MockRepository.GenerateStub<IConfigWorker<SampleClass>>();
			var providerTest = new BasicConfigProvider<SampleClass>(workerStub);
			providerTest.GetConfig();
			providerTest.GetConfig();
			workerStub.AssertWasCalled(w => w.Work(), o => o.Repeat.Twice());
		}

		[TestMethod]
		public void CachingConfigProviderCallsWorkOnce()
		{
			var workerStub = MockRepository.GenerateStub<IConfigWorker<SampleClass>>();
			workerStub.Stub(w => w.Work()).Return(SampleObject);
			var providerTest = new CachingConfigProvider<SampleClass>(workerStub);
			providerTest.GetConfig();
			providerTest.GetConfig();
			workerStub.AssertWasCalled(w => w.Work(), o => o.Repeat.Once());
		}

		[TestMethod]
		public void CachingConfigProviderReturnsSameConfigObjectEachTime()
		{
			var workerStub = MockRepository.GenerateStub<IConfigWorker<SampleClass>>();
			workerStub.Stub(w => w.Work()).Return(SampleObject);
			var providerTest = new CachingConfigProvider<SampleClass>(workerStub);
			var a = providerTest.GetConfig();
			var b = providerTest.GetConfig();
			var c = providerTest.GetConfig();
			Assert.AreEqual(a, b);
			Assert.AreEqual(b, c);
		}
	}
}
