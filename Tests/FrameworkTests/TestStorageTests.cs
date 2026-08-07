namespace Tests.FrameworkTests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Context;
    using Tiver.Fowl.TestingBase;

    /// <summary>
    /// The public per-test storage API. Everything here is written the way a consumer would write
    /// it — no internals, no reflection — because that is the point of the API existing.
    ///
    /// This fixture drives scope creation itself rather than deriving from
    /// <see cref="BaseTestForNUnit"/>, so it can assert on scopes that have ended.
    /// </summary>
    public class TestStorageTests
    {
        private sealed class PlainTest
        {
        }

        [Test]
        public void TestStorage_RoundTripsAValue()
        {
            Flow.Setup(typeof(PlainTest), "round-trip");

            Context.TestStorage.Write("userId", "u-42");

            ClassicAssert.AreEqual("u-42", Context.TestStorage.Read<string>("userId"));
        }

        [Test]
        public void TestStorage_ThrowsWhenNoScopeIsActive()
        {
            Context.ClearTestContext();

            var exception = Assert.Throws<InvalidOperationException>(
                () => Context.TestStorage.Write("k", "v"));
            StringAssert.Contains("No Tiver.Fowl test scope is active", exception!.Message);
        }

        [Test]
        public void TestStorage_DoesNotSurviveTheTestThatWroteIt()
        {
            Flow.Setup(typeof(PlainTest), "first");
            Context.TestStorage.Write("carried", "value");

            Flow.Setup(typeof(PlainTest), "second");

            ClassicAssert.IsFalse(Context.TestStorage.TryRead<string>("carried", out _));
        }

        /// <summary>
        /// The isolation guarantee that makes this API usable from a parallel suite: the scope is
        /// ambient, so storage follows its own test across awaits and spawned work rather than
        /// belonging to a thread.
        /// </summary>
        [Test]
        public async Task TestStorage_FollowsItsTestAcrossAwaitsAndSpawnedWork()
        {
            Flow.Setup(typeof(PlainTest), "async-test");
            Context.TestStorage.Write("token", "mine");

            await Task.Yield();
            ClassicAssert.AreEqual("mine", Context.TestStorage.Read<string>("token"));

            var fromSpawnedWork = await Task.Run(() => Context.TestStorage.Read<string>("token"));
            ClassicAssert.AreEqual("mine", fromSpawnedWork);
        }

        /// <summary>
        /// Test authors and the framework do not share a store. Writing a key the framework also
        /// uses, or clearing the whole store, would otherwise break teardown in a way that looks
        /// nothing like its cause.
        /// </summary>
        [Test]
        public void TestStorage_CannotDisturbFrameworkState()
        {
            Flow.Setup(typeof(PlainTest), "framework-state-intact");
            TestExecutionContext.TestStep = 7;

            Context.TestStorage.Write("TestName", "hijacked");
            Context.TestStorage.Write("TestStep", "not even an int");
            Context.TestStorage.Clear();

            ClassicAssert.AreEqual("framework-state-intact", TestExecutionContext.TestName);
            ClassicAssert.AreEqual(7, TestExecutionContext.TestStep);
        }

        [Test]
        public void SessionStorage_IsReachableOutsideATest()
        {
            Context.ClearTestContext();

            Context.SessionStorage.Write("session-key", "session-value");

            ClassicAssert.AreEqual("session-value", Context.SessionStorage.Read<string>("session-key"));
        }

        [Test]
        public void SessionStorage_IsSeparateFromTestStorage()
        {
            Flow.Setup(typeof(PlainTest), "separate-stores");

            Context.SessionStorage.Write("scope-probe", "session");
            Context.TestStorage.Write("scope-probe", "test");

            ClassicAssert.AreEqual("session", Context.SessionStorage.Read<string>("scope-probe"));
            ClassicAssert.AreEqual("test", Context.TestStorage.Read<string>("scope-probe"));
        }
    }
}
