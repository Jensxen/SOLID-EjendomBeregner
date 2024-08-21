using Moq;
using Xunit;
using EjendomBeregner;


namespace TestBeregnere
{
    public class ReturnZeroTest
    {
        [Fact]
        public void BeregnKvadratmeter_ShouldReturnZero_IfFileEmpty()
        {
            var mockReader = new Mock<ILejemaalReader>();
            var mockParser = new Mock<ILejemaalDataParser>();

            mockReader.Setup(r => r.ReadLejemaalData(It.IsAny<string>())).Returns(new List<string>());
            var calculator = new KvadratmeterCalculator(mockReader.Object, mockParser.Object);

            var result = calculator.BeregnKvadratmeter("lortefil.csv");

            Assert.Equal(0.0, result);
        }
    }
}