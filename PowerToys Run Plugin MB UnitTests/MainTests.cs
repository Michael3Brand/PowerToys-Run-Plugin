using Community.PowerToys.Run.Plugin.PowerToys_Run_Plugin_MB;
using System.Windows;

namespace PowerToys_Run_Plugin_MB_UnitTests
{
    [TestClass]
    public sealed class MainTests
    {
        private Main _subject = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new Main();
        }

        [TestMethod]
        public void Query_should_display_open_destination_folder()
        {
            var results = _subject.Query(new(""));
            Assert.AreEqual("Words: 0", results[0].Title);

        }

        [TestMethod]
        public void Query_should_output_the_destination_filename()
        {


        }
    }
}
