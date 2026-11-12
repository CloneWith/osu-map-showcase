// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.MapShowcase.Utils;

namespace osu.Game.Rulesets.MapShowcase.Tests.Utils
{
    [TestFixture]
    public class TestStringExtensions
    {
        [TestCase("", 10, "")]
        [TestCase("short", 10, "short")]
        [TestCase("exactlyten", 10, "exactlyten")]
        [TestCase("abcdefghij", 7, "ab...ij")]
        [TestCase("abcdefghij", 4, "...j")]
        public void TestTruncateMiddleWithEllipsis(string text, int maxLength, string expected)
        {
            Assert.That(text.TruncateMiddleWithEllipsis(maxLength), Is.EqualTo(expected));
        }

        [Test]
        public void TestTruncateKeepsRequestedLength()
        {
            string text = new string('a', 100);

            for (int maxLength = 4; maxLength <= 40; maxLength++)
                Assert.That(text.TruncateMiddleWithEllipsis(maxLength).Length, Is.EqualTo(maxLength), $"maxLength {maxLength}");
        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void TestTruncateTooShortForEllipsis(int maxLength)
        {
            Assert.That("abcdef".TruncateMiddleWithEllipsis(maxLength), Is.EqualTo("abcdef"[..maxLength]));
        }

        [Test]
        public void TestTruncateNull()
        {
            Assert.That(((string)null!).TruncateMiddleWithEllipsis(10), Is.Null);
        }

        [TestCase("mappool grand final")]
        [TestCase("Grand Finals 2024 (BO5)")]
        [TestCase("决赛谱面")]
        public void TestSafeFilenames(string filename)
        {
            Assert.That(filename.IsSafeForFilename(50), Is.True);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("a/b")]
        [TestCase("a\\b")]
        [TestCase("a:b")]
        [TestCase("a*b")]
        [TestCase("a?b")]
        [TestCase("a\"b")]
        [TestCase("a<b")]
        [TestCase("a>b")]
        [TestCase("a|b")]
        public void TestUnsafeFilenames(string filename)
        {
            Assert.That(filename.IsSafeForFilename(50), Is.False);
        }

        [Test]
        public void TestUnsafeBecauseTooLong()
        {
            Assert.That(new string('a', 51).IsSafeForFilename(50), Is.False);
        }

        [Test]
        public void TestUnsafeBecauseOfSymbols()
        {
            // Currency, math and other symbols are rejected so that a profile name can safely be used as a path.
            Assert.That("mappool ★".IsSafeForFilename(50), Is.False);
            Assert.That("mappool €".IsSafeForFilename(50), Is.False);
            Assert.That("mappool ±".IsSafeForFilename(50), Is.False);
        }

        [Test]
        public void TestErrorMessageProvided()
        {
            Assert.That("".IsSafeForFilename(out var emptyError), Is.False);
            Assert.That(emptyError.ToString(), Is.Not.Empty);

            Assert.That("a/b".IsSafeForFilename(out var charError), Is.False);
            Assert.That(charError.ToString(), Is.Not.Empty);

            Assert.That("valid".IsSafeForFilename(out var noError), Is.True);
            Assert.That(noError.ToString(), Is.Empty);
        }

        [Test]
        public void TestUnicodeLengthIsCountedInChars()
        {
            // Filename length checks are done on `string.Length`, which is what the game stores profiles under.
            Assert.That(new string('决', 50).IsSafeForFilename(50), Is.True);
            Assert.That(new string('决', 51).IsSafeForFilename(50), Is.False);
        }
    }
}
