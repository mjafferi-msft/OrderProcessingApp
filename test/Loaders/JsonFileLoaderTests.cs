using OrderProcessingApp.Constants;
using OrderProcessingApp.Loaders;

namespace OrderProcessingApp.Tests.Loaders
{
    public class DummyModel
    {
        public string Name { get; set; }
    }

    [TestFixture]
    public class JsonFileLoaderTests
    {
        private string _inputDir;

        [SetUp]
        public void SetUp()
        {
            _inputDir = Path.Combine(AppContext.BaseDirectory, AppConstants.InputDirectory);
            Directory.CreateDirectory(_inputDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_inputDir))
                Directory.Delete(_inputDir, true);
        }

        [Test]
        public void Load_ValidJson_ReturnsObject()
        {
            var filePath = Path.Combine(_inputDir, "valid.json");
            File.WriteAllText(filePath, "{\"Name\":\"Test\"}");

            var loader = new JsonFileLoader<DummyModel>();
            var result = loader.Load("valid.json");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test"));
        }

        [Test]
        public void Load_FileNotFound_Throws()
        {
            var loader = new JsonFileLoader<DummyModel>();
            Assert.Throws<FileNotFoundException>(() => loader.Load("missing.json"));
        }

        [Test]
        public void Load_EmptyFile_Throws()
        {
            var filePath = Path.Combine(_inputDir, "empty.json");
            File.WriteAllText(filePath, "");

            var loader = new JsonFileLoader<DummyModel>();
            Assert.Throws<InvalidDataException>(() => loader.Load("empty.json"));
        }

        [Test]
        public void Load_DeserializationReturnsNull_Throws()
        {
            var filePath = Path.Combine(_inputDir, "null.json");
            File.WriteAllText(filePath, "123");

            var loader = new JsonFileLoader<DummyModel>();
            Assert.Throws<InvalidDataException>(() => loader.Load("null.json"));
        }

        [Test]
        public void Load_InvalidJson_Throws()
        {
            var filePath = Path.Combine(_inputDir, "bad.json");
            File.WriteAllText(filePath, "{not valid}");

            var loader = new JsonFileLoader<DummyModel>();
            Assert.Throws<InvalidDataException>(() => loader.Load("bad.json"));
        }
    }
}