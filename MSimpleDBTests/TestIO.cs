namespace MSimpleDBTests
{
    using System.IO;
    using System.Text;
    using MSimpleDB;
    using NUnit.Framework;

    [TestFixture]
    public class TestIO
    {
        string kommentar = @"
# Kommentar
# Kommentar

";

        string testfile = @"## SimpleDB file

# Kommentar
# Kommentar

:ID
1
.Name
J�rg Kubitza
.Telefon
+49(6127)967824
.Mobiltelefon
+49(172)6635916
.Email
kubijoe@freenet.de
.Geburtstag
14.08.1972
.Adresse
Flachsweg 2
65529 Niedernhausen
.Merkmale
Privat Freund
";


        [Test]
        public void TestLoad()
        {
            EntityCollection c = new EntityCollection();
            Reader.Load(c, new StringReader(testfile));
            Assert.That(c.Count, Is.EqualTo(1));

            Entity o = c.Nth(0);
            Assert.That(o.Count, Is.EqualTo(8));
            Assert.That(o.GetAttribute("ID"), Is.EqualTo("1"));

            Assert.That(o.GetAttribute("Mobiltelefon"), Is.EqualTo("+49(172)6635916"));

            Assert.That(o.GetAttribute("Email"), Is.EqualTo("kubijoe@freenet.de"));

            Assert.That(o.HasChanges, Is.EqualTo(false));
            o["Email"] = "neue@email.de";
            Assert.That(o.HasChanges, Is.EqualTo(true));
        }

        [Test]
        public void TestStore()
        {
            EntityCollection c = new EntityCollection();
            Reader.Load(c, new StringReader(testfile));
            Entity o = c.Nth(0);
            Assert.That(c.HasChanges, Is.EqualTo(false));
            Assert.That(o.HasChanges, Is.EqualTo(false));
            o["Email"] = "neue@email.de";
            Assert.That(o.HasChanges, Is.EqualTo(true));
            Assert.That(c.HasChanges, Is.EqualTo(true));
            o["Email"] = "kubijoe@freenet.de";
            StringBuilder sb = new StringBuilder();
            Writer.Store(c, new StringWriter(sb));
            Assert.That(sb.ToString(), Is.EqualTo(testfile));
        }

        [Test]
        public void TestCommentLoad()
        {
            EntityCollection c = new EntityCollection();
            Reader.Load(c, new StringReader(testfile));
            Assert.That(c.Comment, Is.EqualTo(kommentar));
        }
    }
}
