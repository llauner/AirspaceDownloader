using AirspaceDownloader.Services;
using NuGet.Frameworks;
using FluentAssertions;

namespace AirspaceDownloader.Test
{
    [TestClass]
    public class XCSoarDetailsParserTest

    {
        [TestMethod]
        [DeploymentItem(@"guide_aires_securite_details.txt")]
        public void TestParser()
        {
            // Arrange
            var parser = new XCSoarDetailsParser();
            var contents = File.ReadAllText(@"guide_aires_securite_details.txt");

            // Act
            var listFiles = parser.GetFilesList(contents);


            // Assert
            Assert.IsNotNull(parser);
            Assert.IsNotNull(listFiles);
            listFiles.Should().NotBeEmpty();
        }
    }
}