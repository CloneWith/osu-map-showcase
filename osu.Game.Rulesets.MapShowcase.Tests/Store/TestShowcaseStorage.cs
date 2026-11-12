// Copyright (c) CloneWith. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using NUnit.Framework;
using osu.Framework.Testing;
using osu.Game.Rulesets.MapShowcase.Models;
using osu.Game.Rulesets.MapShowcase.Store;

namespace osu.Game.Rulesets.MapShowcase.Tests.Store
{
    [TestFixture]
    public class TestShowcaseStorage
    {
        private TemporaryNativeStorage storage = null!;
        private ShowcaseStorage showcaseStorage = null!;

        [SetUp]
        public void SetUp()
        {
            storage = new TemporaryNativeStorage($"{nameof(TestShowcaseStorage)}-{Guid.NewGuid()}");
            showcaseStorage = new ShowcaseStorage(storage);
        }

        [TearDown]
        public void TearDown()
        {
            storage.Dispose();
        }

        [Test]
        public void TestLastEditedIsUpdatedOnSave()
        {
            var config = createProfile();
            config.LastEdited.Value = null;

            showcaseStorage.SaveChangesTo(config, "edited.json");

            DateTime? firstWrite = config.LastEdited.Value;

            Assert.That(firstWrite, Is.Not.Null);

            showcaseStorage.SaveChangesTo(config, "edited.json");

            Assert.That(config.LastEdited.Value, Is.GreaterThan(firstWrite!.Value));
        }

        [Test]
        public void TestSaveChangesRequiresFilename()
        {
            var config = createProfile();

            Assert.Throws<InvalidOperationException>(() => showcaseStorage.SaveChanges(config));
        }

        [Test]
        public void TestSaveChangesUsesFilenameOnConfig()
        {
            var config = createProfile();
            config.Filename.Value = "from-config.json";

            showcaseStorage.SaveChanges(config);

            Assert.That(showcaseStorage.Exists("from-config.json"), Is.True);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("a/b")]
        [TestCase("../escape")]
        public void TestSaveChangesRejectsUnsafeFilename(string filename)
        {
            Assert.Throws<ArgumentException>(() => showcaseStorage.SaveChangesTo(createProfile(), filename));
        }

        [Test]
        public void TestMissingProfileReturnsNull()
        {
            Assert.That(showcaseStorage.GetConfig("does-not-exist.json"), Is.Null);
        }

        [Test]
        public void TestMalformedProfileReturnsNullInsteadOfThrowing()
        {
            using (var stream = showcaseStorage.CreateFileSafely("broken.json"))
            using (var writer = new StreamWriter(stream))
                writer.Write(@"{ this is not json");

            Assert.That(showcaseStorage.GetConfig("broken.json"), Is.Null);
        }

        [Test]
        public void TestListTournamentsFindsSavedProfiles()
        {
            showcaseStorage.SaveChangesTo(createProfile(), "one.json");
            showcaseStorage.SaveChangesTo(createProfile(), "two.json");

            Assert.That(showcaseStorage.ListTournaments(), Is.EquivalentTo(["one.json", "two.json"]));
        }

        [Test]
        public void TestListTournamentsIgnoresOtherFiles()
        {
            showcaseStorage.SaveChangesTo(createProfile(), "profile.json");

            using (var stream = showcaseStorage.CreateFileSafely("notes.txt"))
            using (var writer = new StreamWriter(stream))
                writer.Write(@"not a profile");

            Assert.That(showcaseStorage.ListTournaments(), Is.EquivalentTo(["profile.json"]));
        }

        [Test]
        public void TestProfileChangeEvent()
        {
            int fired = 0;
            showcaseStorage.OnProfileChange += () => fired++;

            showcaseStorage.SaveChangesTo(createProfile(), "event.json");
            Assert.That(fired, Is.EqualTo(1));

            showcaseStorage.TriggerProfileChange();
            Assert.That(fired, Is.EqualTo(2));
        }

        [Test]
        public void TestSerialisedConfigOmitsFilename()
        {
            var config = createProfile();
            config.Filename.Value = "secret.json";

            Assert.That(ShowcaseStorage.GetSerialisedConfig(config), Does.Not.Contain("secret.json"));
        }

        private static ShowcaseConfig createProfile()
        {
            var config = new ShowcaseConfig
            {
                TournamentName = { Value = "Test Cup" },
                RoundName = { Value = "Grand Finals" },
                StartCountdown = { Value = 15000 },
            };

            config.Beatmaps.Add(new ShowcaseBeatmap
            {
                BeatmapId = 42,
                BeatmapHash = @"abc123",
                ModString = { Value = @"TB" },
                Selector = { Value = new ShowcaseUser { OnlineID = 4787150, Username = @"mrekk" } },
            });

            return config;
        }
    }
}
