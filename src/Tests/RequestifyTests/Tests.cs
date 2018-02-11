using NUnit.Framework;

 

namespace RequestifyTF2.Tests

{

    [TestFixture]

   static public class RequestifyTest

    {
        [Test]
       static public void TestCommandExecute()

        {
            var sut = ReaderThread.TextChecker("nickname test lyl:) : !request https://www.youtube.com/watch?v=DZV3Xtp-BK0");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.CommandExecute));

        }
        [Test]
        static public void TestKillCrit()
        {
            var sut = ReaderThread.TextChecker(
                "DllMain | aimware killed One-Man Cheeseburger Apocalypse with sniperrifle. (crit)");
            Assert.That(sut,Is.EqualTo(ReaderThread.Result.KillCrit));
        }

        [Test]
        static public void TestKill2()
        {
            var sut = ReaderThread.TextChecker("BoyPussi killed dat boi 28 with sniperrifle. (crit)");
            Assert.That(sut,Is.EqualTo(ReaderThread.Result.KillCrit));
        }
        [Test]
        static public void TestKill()
        {
            var sut = ReaderThread.TextChecker(
                "DllMain | aimware killed One-Man Cheeseburger Apocalypse with tf_projectile_rocket.");
            Assert.That(sut,Is.EqualTo(ReaderThread.Result.Kill));
        }
       
        [Test]
        static public void TestConnect()
        {
            var sut = ReaderThread.TextChecker(
                "DllMain | aimware connected");
            Assert.That(sut,Is.EqualTo(ReaderThread.Result.Connected));
        }
        [Test]
        static public void TestSuicide()
        {
            var sut = ReaderThread.TextChecker(
                "DllMain | aimware suicided.");
            Assert.That(sut,Is.EqualTo(ReaderThread.Result.Suicide));
        }

 

    }

}

 

 