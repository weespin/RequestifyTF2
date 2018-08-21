// RequestifyTests
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using NUnit.Framework;

namespace RequestifyTF2.Tests

{
    [TestFixture]
    public static class RequestifyTest
    {
        [Test]
        public static void TestKill()
        {
            var sut = ReaderThread.TextChecker("BoyPussi killed dat boi 28 with sniperrifle. (crit)");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.KillCrit));
            sut = ReaderThread.TextChecker(
                "DllMain | aimware killed One-Man Cheeseburger Apocalypse with tf_projectile_rocket.");
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
            var sut = ReaderThread.TextChecker("DllMain | aimware connected");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.Connected));
        }

        [Test]
        public static void TestKillCrit()
        {
            var sut = ReaderThread.TextChecker(
                "DllMain | aimware killed One-Man Cheeseburger Apocalypse with sniperrifle. (crit)");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.KillCrit));
        }

        [Test]
        public static void TestSuicide()
        {
            var sut = ReaderThread.TextChecker("DllMain | aimware suicided.");
            Assert.That(sut, Is.EqualTo(ReaderThread.Result.Suicide));
        }
    }
}