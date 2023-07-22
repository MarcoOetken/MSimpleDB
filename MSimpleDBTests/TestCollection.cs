namespace MSimpleDBTests
{
    using System.IO;
    using MSimpleDB;
    using NUnit.Framework;

    [TestFixture]
    public class TestCollection
    {
        const string kommentar = @"
# Kommentar
# Kommentar

";

        [Test]
        public void TestComment()
        {
            var c = new EntityCollection
            {
                Comment = "Hallo"
            };
            Assert.That(c.Comment, Is.EqualTo("# Hallo"));
        }

        [Test]
        public void TestComment2()
        {
            var c = new EntityCollection
            {
                Comment = @"
Kommentar
Kommentar

"
            };
            Assert.That(c.Comment, Is.EqualTo(kommentar));
        }
    }
}
