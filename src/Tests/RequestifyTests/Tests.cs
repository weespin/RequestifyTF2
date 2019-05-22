using NUnit.Framework;

namespace RequestifyTF2.Tests

{
    [TestFixture]
    public static class RequestifyTest
    {
        public static void TestKill()
        {
            var sut = ReaderThread.TextChecker("BoyPussi killed dat boi 28 with sniperrifle. (crit)");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.KillCrit));
            sut = ReaderThread.TextChecker(
                "[NCC] DllMain killed $20 users with tf_projectile_rocket.");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.Kill));
        }

        [Test]
        public static void TestCommandExecute()
        {
            var sut = ReaderThread.TextChecker(
                "nickname test lyl:) : !request https://www.youtube.com/watch?v=DZV3Xtp-BK0");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.CommandExecute));
        }
        [Test]
        public static void TestChat()
        {
            var sut = ReaderThread.TextChecker(
                "*DEAD* Hey : ImJust a test lul");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.Chatted));
        }

        [Test]
        public static void TestConnect()
        {
            var sut = ReaderThread.TextChecker("[NCC]Effie connected");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.Connected));
        }

        [Test]
        public static void TestKillCrit()
        {
            var sut = ReaderThread.TextChecker(
                "[RLS]V952 killed Tom with sniperrifle. (crit)");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.KillCrit));
        }

        [Test]
        public static void TestSuicide()
        {
            var sut = ReaderThread.TextChecker("DurRud suicided.");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.Suicide));
        }
    }
}