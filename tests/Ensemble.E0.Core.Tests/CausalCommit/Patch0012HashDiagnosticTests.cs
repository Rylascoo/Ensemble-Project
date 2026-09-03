using System.Collections.Immutable;
using Ensemble.E0.Core.CausalCommit;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.StateInterpreter;
using Ensemble.E0.Core.Tests.Patch0012;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Core.Tests.CausalCommit;

[TestClass]
public sealed class Patch0012HashDiagnosticTests
{
    [TestMethod]
    public void PostCommitHashDiagnostic()
    {
        var state = Patch0012TestSupport.Genesis();
        var pipeline = Patch0012TestSupport.BuildPipeline(
            new[] { Patch0012TestSupport.PressureAdd() },
            new[] { StateMutationDomain.Pressure });
        var take = Patch0012TestSupport.AcceptedTake(
            pipeline,
            "TAKE-PATCH-0012-ORACLE");
        var checkpoint = ProductionStateCheckpoint.Capture(state);
        var binding = E0TakeStateBinding.Bind(checkpoint, pipeline.Context, take);
        var materializations = E0RecordMaterializationSet.Bind(
            ImmutableArray.Create(
                E0RecordMaterialization.Create(
                    0,
                    RecordId.From("PRESSURE-PATCH-0012-ORACLE"))));

        var result = DeterministicCausalCommit.Commit(
            CommitId.From("COMMIT-PATCH-0012-ORACLE"),
            state,
            binding,
            materializations);

        Console.WriteLine($"POST COMMIT HASH ACTUAL: {result.ResultState.StateHash.Value}");
        Assert.Fail("Patch 0012 diagnostic-only hash extraction.");
    }
}
