namespace EjendomBeregner.BusinessLogic
{
    #region before
    public class EjendomBeregnerService1
    {
        public double BeregnKvadratmeter1(string lejemaalDataFilename) //SRP - Beregner og henter fil, skal deles op i to metoder
        {
            var lejemaalene = File.ReadAllLines(lejemaalDataFilename);
            var kvadratmeter = 0.0;


            foreach (var lejemaal in lejemaalene)
            {
                var lejemaalParts = lejemaal.Split(',');
                double lejemaalKvadratmeter; //Clean code - double skal ændres til var
                double.TryParse(RemoveQuotes1(lejemaalParts[1]), out lejemaalKvadratmeter); //double ændres til var
                kvadratmeter += lejemaalKvadratmeter;
            }

            return kvadratmeter;
        }

        

        private string RemoveQuotes1(string lejemaalPart)
        {
            return lejemaalPart.Replace('"', ' ').Trim();
        }
    }

    #endregion

    #region after


    /* 
    Her er en after version af ovenstående kode som overholder alle SOLID-Principper.
    
    SRP - 
    FileLejemaalReader klassen har kun ansvar for at læse filer
    FileLejemaalDataParser klassen har kun ansvar for at parse filer

    OCP - 
    Hvis man ønsker at ændre måden som data bliver læst kan man oprette en ny klasse som implementerer ILejemaalReader uden det ændre på noget andet.
    Hvis man gerne vil parse en anden filformat end CSV kan man oprette en klasse som implemeterer ILejemaalDataParser.

    LSP -
    FileLejemaalReader kan erstattes af enhver anden implementering af ILejemaalReader uden at påvirke andet
    Samme med FileLejemaalDataParser

    ISP -
    Jeg bruger 2 specifikke interfaces ILejemaalReader og ILejemaalDataParser. Hver klasse implementerer kun det interface der er nødvendigt

    DIP -
    Kvadratmeterberegner afhænger af abstraktionerne ILejemaalReader og IlejemaalDataParser og ikke af konkrete implementeringer som FileLejemaalReader
    Eller CsvLejemaalReader

    */

    // Interface for at læse lejemål data
    public interface ILejemaalReader
    {
        IEnumerable<string> ReadLejemaalData(string filename);
    }

    // Interface for at parse lejemål data
    public interface ILejemaalDataParser
    {
        double ParseKvadratmeter(string lejemaal);
    }

    // Implementering af lejemål reader, der læser data fra en fil 
    public class FileLejemaalReader : ILejemaalReader
    {
        public IEnumerable<string> ReadLejemaalData(string filename)
        {
            return File.ReadAllLines(filename);
        }
    }

    //Implementering af håndtering af CSV data 
    public class CsvLejemaalDataParser : ILejemaalDataParser
    {
        public double ParseKvadratmeter(string lejemaal)
        {
            var lejemaalParts = lejemaal.Split(",");
            double.TryParse(RemoveQuotes(lejemaalParts[1]), out var lejemaalKvadratmeter);
            return lejemaalKvadratmeter;
        }
        private string RemoveQuotes(string lejemaalPart)
        {
            return lejemaalPart.Replace('"', ' ').Trim();
        }
    } 
    
    //Beregning af kvadratmeter
    public class KvadratmeterCalculator
    {
        private readonly ILejemaalReader _reader;
        private readonly ILejemaalDataParser _parser;

        public KvadratmeterCalculator(ILejemaalReader reader, ILejemaalDataParser parser)
        {
            _reader = reader;
            _parser = parser;
        }
        public double BeregnKvadratmeter(string lejemaalDataFilename)
        {
            var lejemaalene = _reader.ReadLejemaalData(lejemaalDataFilename);
            var kvadratmeter = 0.0;

            foreach (var lejemaal in lejemaalene)
            {
                kvadratmeter += _parser.ParseKvadratmeter(lejemaal);
            }

            return kvadratmeter;
        }

    }
    #endregion
}


