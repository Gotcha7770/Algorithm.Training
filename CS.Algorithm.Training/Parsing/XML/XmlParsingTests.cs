using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using AwesomeAssertions;
using Xunit;

namespace Algorithm.Training.Parsing.XML;

public class XmlParsingTests
{
    private const string PathToXml = "Algorithm.Training.Parsing.XML.example.xml";

    [Fact]
    public void XmlDocument()
    {
        var xDocument = new XmlDocument();
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PathToXml))
        {
            xDocument.Load(stream);
        }

        xDocument.Should().NotBeNull();
        xDocument.DocumentElement.Name.Should().Be("Document");
    }

    [Fact]
    public void LinqToXml()
    {
        XDocument xDocument;
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PathToXml))
        {
            xDocument = XDocument.Load(stream);
        }

        xDocument.Should()
            .NotBeNull()
            .And.HaveRoot("Document")
            .And.HaveElement("Property");
    }
}