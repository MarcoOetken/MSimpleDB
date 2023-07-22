namespace MSimpleDBTests
{
    using NUnit.Framework;

    [TestFixture]
    public class TestObject
    {
        [Test]
        public void TestObject1()
        {
            MSimpleDB.Entity o = new MSimpleDB.Entity();
            o.Add("name", "wert");
            Assert.That(o.Count, Is.EqualTo(1));

            Assert.That(o.GetAttribute("name"), Is.EqualTo("wert"));
        }

        [Test]
        public void TestIndexedAccess()
        {
            MSimpleDB.Entity o = new MSimpleDB.Entity();
            o.Add("name", "wert");
            Assert.That(o.Count, Is.EqualTo(1));

            Assert.That(o["name"], Is.EqualTo("wert"));

            o["name"] = "neu";
            Assert.That(o["name"], Is.EqualTo("neu"));
        }

        [Test]
        public void TestMultipleAdd()
        {
            MSimpleDB.Entity o = new MSimpleDB.Entity();
            o.Add("x", "y");
            o.Add("x", "z");
            Assert.That(o.Count, Is.EqualTo(2));
            Assert.That(o.GetNthAttribute(0)!.Value, Is.EqualTo("y"));
            Assert.That(o.GetNthAttribute(1)!.Value, Is.EqualTo("z"));
        }

        [Test]
        public void TestAssign()
        {
            MSimpleDB.Entity o = new MSimpleDB.Entity();
            o["name"] = "wert";
            Assert.That(o.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestAttribute()
        {
            MSimpleDB.Attribute a = new MSimpleDB.Attribute("name", "wert");
            Assert.That(a.Name, Is.EqualTo("name"));
            Assert.That(a.Value, Is.EqualTo("wert"));

            MSimpleDB.Entity o = new MSimpleDB.Entity();
            o.Add("name", "wert");
            Assert.That(o, Has.Count.EqualTo(1));

            a = o.GetNthAttribute(0)!;
            Assert.That(a.Name, Is.EqualTo("name"));
            Assert.That(a.Value, Is.EqualTo("wert"));
            Assert.That(o.HasChanges, Is.EqualTo(true));
        }

        [Test]
        public void TestSetOrAdd()
        {
            var o = new MSimpleDB.Entity();
            o.SetOrAddAttribute("name", "wert");
            Assert.That(o, Has.Count.EqualTo(1));
            Assert.That(o["name"], Is.EqualTo("wert"));
        }

        [Test]
        public void TestTryParse()
        {
            var o = new MSimpleDB.Entity();
            Assert.That(o["name"], Is.Null);

            Assert.That(int.TryParse(o["name"], out int _), Is.False);
        }
    }
}
