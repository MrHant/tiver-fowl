namespace Tests.FrameworkTests
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Configuration;

    /// <summary>
    /// <see cref="ActiveConfiguration.Get{T}(string, T)"/> must distinguish "no configured value"
    /// from "configured to a value that happens to equal <c>default(T)</c>". Reading a configured
    /// <c>false</c> as the caller's <c>true</c> default is silent and total — the config file says
    /// one thing and every caller sees the opposite.
    ///
    /// The <c>Flags</c> section these tests read lives only in <c>config.json</c> and deliberately
    /// not in <c>config.qa.json</c>, so the values are identical under either layering. That keeps
    /// the fixture independent of <see cref="ConfigurationTests"/>, which switches the environment
    /// globally and does not switch it back.
    /// </summary>
    [Parallelizable(ParallelScope.All)]
    public class ConfigurationValueTests
    {
        [Test]
        public void Get_ReturnsConfiguredFalse_RatherThanATrueDefault()
        {
            ClassicAssert.IsFalse(ActiveConfiguration.Get<bool>("Flags:Enabled", true));
        }

        [Test]
        public void Get_ReturnsConfiguredZero_RatherThanANonZeroDefault()
        {
            ClassicAssert.AreEqual(0, ActiveConfiguration.Get<int>("Flags:Count", 42));
        }

        [Test]
        public void Get_ReturnsConfiguredEmptyString_RatherThanADefault()
        {
            ClassicAssert.AreEqual(string.Empty, ActiveConfiguration.Get<string>("Flags:Name", "fallback"));
        }

        [Test]
        public void Get_ReturnsTheDefault_WhenThePathIsAbsent()
        {
            ClassicAssert.IsTrue(ActiveConfiguration.Get<bool>("Flags:NoSuchFlag", true));
            ClassicAssert.AreEqual("fallback", ActiveConfiguration.Get<string>("Flags:NoSuchFlag", "fallback"));
        }

        /// <summary>
        /// An explicit <c>null</c> in the config file reads as absent: the JSON provider records the
        /// key with a null value, and a null value is not something a typed read can convert.
        /// </summary>
        [Test]
        public void Get_ReturnsTheDefault_WhenTheValueIsExplicitlyNull()
        {
            ClassicAssert.AreEqual("fallback", ActiveConfiguration.Get<string>("Flags:ExplicitNull", "fallback"));
        }

        /// <summary>
        /// A parent section holds children but no scalar value of its own. It is the case that makes
        /// <c>Exists()</c> the wrong predicate here — <c>Exists()</c> is true for it, yet a typed
        /// read of it yields nothing, so the default must still apply.
        /// </summary>
        [Test]
        public void Get_ReturnsTheDefault_WhenThePathNamesASectionNotAValue()
        {
            ClassicAssert.AreEqual("fallback", ActiveConfiguration.Get<string>("Flags", "fallback"));
        }

        [Test]
        public void Get_ReturnsNull_WhenAReferenceTypeIsAbsentAndNoDefaultIsSupplied()
        {
            ClassicAssert.IsNull(ActiveConfiguration.Get<string>("Flags:NoSuchFlag"));
        }
    }
}
