// (c) D. Sawyer <damiensawyer@gmail.com> 2025

namespace DepthChart.Tests.FixtureSetup.Learning;

// see IClassFixture<>??
[CollectionDefinition("DamoCollection")]
public class DamoCollection : ICollectionFixture<DamoFixture>
{
  // This class has no code, and is never created. Its purpose is simply
  // to be the place to apply [CollectionDefinition] and all the
  // ICollectionFixture<> interfaces.
}