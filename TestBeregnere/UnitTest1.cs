using Moq;
using Xunit;
using EjendomBeregner;


namespace TestBeregnere
{
    public class UnitTest1
    {
        [Fact]
        public void BeregnKvadratmeter_ShouldReturnCorrectSum_WhenGivenValidData()
        {
            var mockReader = new Mock<ILejemaalReader>();
            var mockParser = new Mock<ILejemaalDataParser>();

            var testData = new List<string>()
            {
                "lejemaal1,\"50\"",
                "lejemaal2,\"75\"",
                "lejemaal3,\"25\""
            };

            mockReader.Setup(r => r.ReadLejemaalData(It.IsAny<string>())).Returns(testData);
            mockParser.Setup(p => p.ParseKvadratmeter(It.IsAny<string>())).Returns((string lejemaal) =>
            {
                var parts = lejemaal.Split(',');
                return double.Parse(parts[1].Replace("\"", "").Trim());
            });

            var calculator = new KvadratmeterCalculator(mockReader.Object, mockParser.Object);

            var result = calculator.BeregnKvadratmeter("filename.csv");

            Assert.Equal(150.0, result);
        }
    }
}