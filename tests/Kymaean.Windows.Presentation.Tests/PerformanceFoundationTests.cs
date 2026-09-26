using Kymaean.Application;
using Kymaean.Infrastructure.Execution;
using Kymaean.Infrastructure.Persistence;
using Kymaean.Windows.Presentation;

namespace Kymaean.Windows.Presentation.Tests;

[TestClass]
public sealed class PerformanceFoundationTests
{
    private static MainPageViewModel OpenRequest(Fixture f, ProductOperationCoordinator owner)
    {
        var vm = new MainPageViewModel(PresentationStartupResult.Owned(owner));
        vm.NavigateShell(ShellRoute.CurrentProduction);
        vm.OpenScenes();
        Assert.IsTrue(vm.InspectScene(f.Scene));
        Assert.IsTrue(vm.OpenPerformance(f.Actor));
        return vm;
    }

    [TestMethod]
    public async Task CreatorSurfacePreservesIdentityPendingExactEmptySuccessAndBack()
    {
        using var f = new Fixture(); f.Runtime.Text = "";
        var scheduler = new ManualScheduler(); var owner = f.Owner(scheduler.Enqueue);
        var vm = OpenRequest(f, owner);
        Assert.AreEqual(f.Actor, vm.PerformanceIdentity!.CharacterId);
        Assert.AreEqual(f.Scene, vm.PerformanceIdentity.SceneId);
        Assert.IsNotNull(vm.PerformanceIdentity.CharacterCode);
        Assert.IsTrue(vm.CanRequestPerformance);
        Assert.AreEqual(0, f.Runtime.Calls); // Opening is inspection only.
        var request = vm.SubmitPerformanceAsync();
        Assert.IsTrue(vm.IsApplicationBusy);
        Assert.IsFalse(vm.CanRequestPerformance);
        Assert.IsNull(vm.ClosePerformance());
        Assert.AreEqual(PerformanceAdmission.Busy, await vm.SubmitPerformanceAsync());
        scheduler.Take()(); await request;
        Assert.AreEqual("Performance recorded.", vm.PerformanceOutcomeMessage);
        Assert.IsTrue(vm.HasEmptyPerformance);
        Assert.AreEqual("", vm.PerformanceText);
        Assert.AreEqual("A bounded circumstance.", vm.PerformanceConsequence);
        Assert.IsFalse(vm.CanRequestPerformance);
        Assert.AreEqual(1, f.Catalog.Commits);
        Assert.AreEqual(f.Actor, vm.ClosePerformance());
        Assert.IsTrue(vm.IsSceneDetail);
        Assert.IsFalse(vm.HasRecordedPerformance);
        Assert.AreEqual(f.Scene, vm.InspectedScene!.Id);
    }

    [TestMethod]
    public async Task CreatorSurfaceShowsDistinctTypedAndInvocationUncertaintyWithoutGuessedContent()
    {
        foreach (var mode in new[] { "invalid", "incompatible", "io", "access" })
        {
            using var f = new Fixture();
            if (mode == "invalid") f.Catalog.AfterCommitInvalid = true;
            if (mode == "incompatible") f.Catalog.Failure = ProductAccessFailureKind.Incompatible;
            if (mode == "io") f.Catalog.CommitError = new IOException();
            if (mode == "access") f.Catalog.CommitError = new UnauthorizedAccessException();
            var owner = f.Owner(work => work()); var vm = OpenRequest(f, owner);
            await vm.SubmitPerformanceAsync();
            Assert.IsFalse(vm.HasRecordedPerformance);
            Assert.AreEqual("", vm.PerformanceText);
            Assert.AreEqual("", vm.PerformanceConsequence);
            Assert.IsTrue(vm.ShowPerformanceReopenHelp); // Existing owner conservatively fences all typed failures.
            Assert.IsTrue(vm.PerformanceOutcomeMessage.StartsWith(mode switch
            {
                "invalid" => "Performance not confirmed.",
                "incompatible" => "Performance unavailable in this version.",
                _ => "Confirmation unavailable."
            }, StringComparison.Ordinal));
            Assert.AreEqual(f.Actor, vm.ClosePerformance());
            Assert.IsTrue(vm.OpenPerformance(f.Actor));
            Assert.IsFalse(vm.CanRequestPerformance);
        }
    }

    [TestMethod]
    public async Task UncertaintyForOneActorNeverAppearsAsAnotherActorsRequestedOutcome()
    {
        using var f = new Fixture(); f.Catalog.AfterCommitInvalid = true;
        var owner = f.Owner(work => work()); var vm = OpenRequest(f, owner);
        var original = vm.PerformanceCharacter;
        await vm.SubmitPerformanceAsync(); vm.ClosePerformance();
        Assert.IsTrue(vm.OpenPerformance(f.OtherActor));
        Assert.AreNotEqual(original, vm.PerformanceCharacter);
        Assert.IsTrue(vm.ShowEarlierPerformanceWarning);
        Assert.IsTrue(vm.EarlierPerformanceWitness.Contains(original, StringComparison.Ordinal));
        Assert.AreEqual("Performance requests are unavailable until this Production is opened again.", vm.PerformanceOutcomeMessage);
        Assert.IsFalse(vm.HasPerformanceResult);
        Assert.IsFalse(vm.CanRequestPerformance);
        Assert.AreEqual(1, f.Catalog.Commits);
    }

    [TestMethod]
    public async Task ReopenedCreatorSurfaceKeepsEarlierIdentityAndWarnsAboutAnotherRecord()
    {
        using var f = new Fixture(); f.Catalog.AfterCommitInvalid = true;
        var owner = f.Owner(work => work()); var vm = OpenRequest(f, owner);
        await vm.SubmitPerformanceAsync(); vm.ClosePerformance();
        owner.OpenProduction(f.A);
        Assert.IsTrue(vm.OpenPerformance(f.OtherActor));
        Assert.IsTrue(vm.ShowEarlierPerformanceWarning);
        Assert.IsTrue(vm.EarlierPerformanceWitness.Contains("Character code", StringComparison.Ordinal));
        Assert.IsTrue(vm.CanRequestPerformance);
        Assert.AreEqual(f.OtherActor, vm.PerformanceIdentity!.CharacterId);
    }

    [TestMethod]
    public async Task CreatorSurfaceRenderingFaultCannotBecomeFailedRequestOrRetry()
    {
        using var f = new Fixture(); var owner = f.Owner(work => work()); var vm = OpenRequest(f, owner);
        var once = false;
        vm.PropertyChanged += (_, change) =>
        {
            if (!once && change.PropertyName == nameof(vm.PerformanceText) && vm.HasRecordedPerformance)
            { once = true; throw new InvalidOperationException("render fault"); }
        };
        await vm.SubmitPerformanceAsync();
        Assert.IsTrue(once);
        Assert.IsTrue(vm.HasRecordedPerformance);
        Assert.IsTrue(vm.LastPerformancePublication!.RenderingFailed);
        Assert.AreEqual("Performance recorded. A display update failed.", vm.PerformanceOutcomeMessage);
        Assert.AreEqual("A line.", vm.PerformanceText);
        Assert.IsFalse(vm.CanRequestPerformance);
        Assert.AreEqual(1, f.Catalog.Commits);
    }

    [TestMethod]
    public void OrdinaryCreatorRequestSurfaceIsTruthfullyUnavailable()
    {
        using var f = new Fixture(); var owner = ProductOperationCoordinator.Own(f.App);
        var vm = OpenRequest(f, owner);
        Assert.AreEqual("Performance is unavailable.", vm.PerformanceOutcomeMessage);
        Assert.IsFalse(vm.CanRequestPerformance);
        Assert.IsNull(typeof(ProductOperationCoordinator).GetMethod("OwnDirectorPreview"));
        Assert.AreEqual(f.Actor, vm.ClosePerformance());
        Assert.AreEqual(0, f.Runtime.Calls);
    }

    [TestMethod]
    public async Task InFlightInvalidationRetainsKnownCommitWithoutPaintingReplacement()
    {
        using var f = new Fixture(); var owner = f.Owner(work => work());
        f.Runtime.DuringPerform = owner.InvalidatePresentation;
        var activation = owner.Begin(f.Target(owner)); var result = await activation.Completion!;
        Assert.AreEqual(PerformanceOutcome.KnownSuccess, result.Outcome);
        Assert.IsFalse(owner.Publish(activation.Request!, _ => Assert.Fail("Stale completion rendered.")));
        Assert.AreEqual(1, f.Catalog.Commits);
        Assert.IsTrue(owner.RequiresReopen(f.A));
        Assert.AreSame(result, owner.Terminals.Single());
    }

    [TestMethod]
    public async Task DispatcherRejectionPreservesSuccessAndFencesLateAcceptedCallback()
    {
        foreach (var throws in new[] { false, true })
        {
            using var f = new Fixture(); var owner = f.Owner(work => work());
            Action? late = null;
            var vm = new MainPageViewModel(PresentationStartupResult.Owned(owner), work =>
            {
                late = work;
                if (throws) throw new InvalidOperationException("dispatcher unavailable");
                return false;
            });
            await vm.RequestPerformanceAsync(vm.CapturePerformanceTarget(f.Scene, f.Actor)!);
            Assert.AreEqual(PerformanceOutcome.KnownSuccess, owner.Terminals.Single().Outcome);
            Assert.IsTrue(owner.LastPublication!.RenderingFailed);
            Assert.IsFalse(owner.IsBusy);
            Assert.IsTrue(owner.RequiresReopen(f.A));
            Assert.IsTrue(vm.PerformanceAvailabilityMessage.StartsWith("Performance recorded.", StringComparison.Ordinal));
            owner.OpenProduction(f.A);
            var next = owner.Begin(f.Target(owner)); await next.Completion!;
            late!();
            Assert.IsTrue(owner.IsBusy);
            Assert.IsNull(vm.LastPerformancePublication);
            owner.Publish(next.Request!, _ => { });
        }
    }

    [TestMethod]
    public async Task DefaultWorkerDoesNotOccupyDispatcherWhileRuntimeIsDelayed()
    {
        using var f = new Fixture();
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        var invokingThread = Environment.CurrentManagedThreadId;
        var workerThread = invokingThread;
        f.Runtime.DuringPerform = () =>
        {
            workerThread = Environment.CurrentManagedThreadId;
            entered.Set();
            Assert.IsTrue(release.Wait(TimeSpan.FromSeconds(10)), "Test deadlock watchdog, not a timing assertion.");
        };
        var owner = ProductOperationCoordinator.ForTests(f.App, new ProductPerformer(f.Runtime), new ProductConsequenceInterpreter(f.Runtime));
        var activation = owner.Begin(f.Target(owner));
        try
        {
            Assert.IsTrue(entered.Wait(TimeSpan.FromSeconds(10)));
            Assert.AreNotEqual(invokingThread, workerThread);
            Assert.IsFalse(activation.Completion!.IsCompleted);
            // This is a dispatcher-side input/paint callback while the runtime is at a barrier.
            var dispatcherMarker = 0;
            Action processQueuedInput = () => dispatcherMarker++;
            processQueuedInput();
            Assert.AreEqual(1, dispatcherMarker);
            Assert.ThrowsExactly<InvalidOperationException>(() => owner.Query());
        }
        finally { release.Set(); }
        await activation.Completion!;
        owner.Publish(activation.Request!, _ => { });
    }

    [TestMethod]
    public async Task ReplacementViewReadsOnlySnapshotAndCannotReceiveEarlierTargetResult()
    {
        using var f = new Fixture(); var scheduler = new ManualScheduler(); var owner = f.Owner(scheduler.Enqueue);
        var oldView = new MainPageViewModel(PresentationStartupResult.Owned(owner));
        var invocation = oldView.RequestPerformanceAsync(oldView.CapturePerformanceTarget(f.Scene, f.Actor)!);
        oldView.DetachPerformancePresentation();
        var newView = new MainPageViewModel(PresentationStartupResult.Owned(owner));
        Assert.IsTrue(newView.IsApplicationBusy);
        scheduler.Take()(); await invocation;
        Assert.IsFalse(newView.IsApplicationBusy);
        Assert.IsTrue(owner.LastPublication!.Stale);
        Assert.IsTrue(newView.IsPerformanceReopenRequired);
        Assert.IsNull(newView.LastPerformancePublication);
        Assert.AreEqual(PerformanceOutcome.KnownNoncommit, owner.Terminals.Single().Outcome);
        Assert.AreEqual(0, f.Runtime.Calls);
    }

    [TestMethod]
    public void OrdinaryPresentationHasNoRuntimeOrTestAssemblyDependency()
    {
        var references = typeof(MainPageViewModel).Assembly.GetReferencedAssemblies().Select(reference => reference.Name!).ToArray();
        Assert.IsFalse(references.Any(name => name.Contains("Tests", StringComparison.Ordinal) || name.Contains("Infrastructure.Execution", StringComparison.Ordinal)));
        Assert.IsFalse(typeof(IProductExecutionRuntime).Assembly.GetTypes().Any(type =>
            type.IsClass && typeof(IProductExecutionRuntime).IsAssignableFrom(type)));
        Assert.IsNull(typeof(ProductOperationCoordinator).GetMethod("ForTests"));
        Assert.IsNull(typeof(ProductOperationCoordinator).GetMethod("Begin"));
    }

    [TestMethod]
    public void ShippingCompositionAndEitherMissingPortRemainUnavailable()
    {
        using var fixture = new Fixture();
        var owner = ProductOperationCoordinator.Own(fixture.App);
        Assert.AreSame(owner, ProductOperationCoordinator.Own(fixture.App));
        Assert.IsFalse(owner.HasExecutors);
        Assert.AreEqual(PerformanceAdmission.Unavailable, owner.Begin(fixture.Target(owner)).Admission);
        var vm = new MainPageViewModel(PresentationStartupResult.Product(ProductAccessResult<ProductApplication>.Success(fixture.App)));
        Assert.IsFalse(vm.HasPerformanceExecutors);
        Assert.AreEqual("Performance is unavailable.", vm.PerformanceAvailabilityMessage);
        Assert.AreEqual(0, fixture.Catalog.Commits);
        foreach (var missingPerformer in new[] { true, false })
        {
            using var other = new Fixture();
            var partial = ProductOperationCoordinator.ForTests(other.App,
                missingPerformer ? null : new ProductPerformer(other.Runtime),
                missingPerformer ? new ProductConsequenceInterpreter(other.Runtime) : null);
            Assert.AreEqual(PerformanceAdmission.Unavailable, partial.Begin(other.Target(partial)).Admission);
            Assert.AreEqual(0, other.Runtime.Calls);
        }
    }

    [TestMethod]
    public async Task ManualDispatchCapturesExactIdsAndRejectsDuplicateWorkAndEveryProductAccess()
    {
        using var f = new Fixture();
        var scheduler = new ManualScheduler();
        var owner = f.Owner(scheduler.Enqueue);
        var first = f.Target(owner);
        var second = owner.CaptureTarget(f.Scene, f.OtherActor)!;
        Assert.AreEqual(first.CharacterName, second.CharacterName);
        Assert.AreNotEqual(first.CharacterId, second.CharacterId);
        Assert.AreNotEqual(first.CharacterCode, second.CharacterCode);
        Assert.IsNull(owner.CaptureTarget(new SceneId("missing"), f.Actor));
        var activation = owner.Begin(second);
        Assert.IsTrue(owner.IsBusy);
        Assert.IsFalse(activation.Completion!.IsCompleted);
        Assert.AreEqual(PerformanceAdmission.Busy, owner.Begin(first).Admission);
        Action[] blocked = [() => owner.Query(), () => owner.Navigate(ApplicationScope.Home),
            () => owner.NavigateProduction(ProductSpace.Stage), () => owner.OpenProduction(f.A),
            () => owner.RecoverProduction(f.A), () => owner.CreateProduction("Other"),
            () => owner.CreateCharacter("Other"), () => owner.EstablishScene([]),
            () => owner.ReplaceWorldCurrentState(WorldCurrentState.Empty)];
        foreach (var action in blocked) Assert.ThrowsExactly<InvalidOperationException>(action);
        var dispatcherResponsive = false;
        Action dispatcherMarker = () => dispatcherResponsive = true;
        dispatcherMarker();
        Assert.IsTrue(dispatcherResponsive);
        var work = scheduler.Take(); work(); work();
        var result = await activation.Completion;
        Assert.AreEqual(PerformanceOutcome.KnownSuccess, result.Outcome);
        Assert.AreEqual(f.OtherActor, result.Execution!.AcceptedPerformance.CharacterId);
        Assert.AreEqual(2, f.Runtime.Calls); // One performer and one separate interpreter.
        Assert.AreEqual(1, f.Catalog.Commits);
        Assert.IsTrue(owner.IsBusy); // Worker completion is not permission to Query/navigate.
        Assert.IsTrue(owner.Publish(activation.Request!, _ => { }));
        Assert.IsFalse(owner.IsBusy);
    }

    [TestMethod]
    public async Task ViewModelGuardsNotificationReentryNavigationEditsAndQueuedActivation()
    {
        using var f = new Fixture();
        var scheduler = new ManualScheduler();
        var owner = f.Owner(scheduler.Enqueue);
        var vm = new MainPageViewModel(PresentationStartupResult.Owned(owner));
        vm.SelectProduction(vm.ProductionRows.Single(row => row.Id == f.A));
        Assert.IsTrue(vm.OpenSelectedProduction());
        vm.OpenScenes();
        var target = vm.CapturePerformanceTarget(f.Scene, f.Actor)!;
        Task<PerformanceAdmission>? duplicate = null;
        vm.PropertyChanged += (_, _) =>
        {
            if (!vm.IsApplicationBusy) return;
            vm.NavigateShell(ShellRoute.Settings); vm.SelectProduction(null); vm.OpenCharacters();
            vm.CloseScenes(); vm.OpenWorldTruths(); vm.BeginSceneDraft(); vm.BeginCharacterCreation();
            vm.ProductionNameDraft = "changed"; vm.CharacterNameDraft = "changed";
            Assert.IsFalse(vm.OpenSelectedProduction()); Assert.IsFalse(vm.RecoverSelectedProduction());
            Assert.IsFalse(vm.BeginProductionCreationSubmission());
            Assert.IsFalse(vm.BeginCharacterCreationSubmission());
            Assert.IsFalse(vm.BeginWorldTruthReplacementSubmission());
            Assert.IsNull(vm.EstablishScene());
            duplicate ??= vm.RequestPerformanceAsync(target);
        };
        var invocation = vm.RequestPerformanceAsync(target);
        Assert.IsFalse(invocation.IsCompleted);
        Assert.IsFalse(vm.IsSceneNavigationEnabled);
        Assert.IsTrue(vm.IsScenes);
        Assert.AreEqual(string.Empty, vm.CharacterNameDraft);
        Assert.AreEqual(PerformanceAdmission.Busy, await duplicate!);
        scheduler.Take()();
        Assert.AreEqual(PerformanceAdmission.Accepted, await invocation);
        Assert.IsTrue(vm.IsSceneNavigationEnabled);
        Assert.AreEqual(PerformanceOutcome.KnownSuccess, vm.LastPerformancePublication!.Terminal.Outcome);
    }

    [TestMethod]
    public async Task StaleGenerationCannotPaintAndOldCallbackCannotReleaseNewLease()
    {
        using var f = new Fixture();
        var scheduler = new ManualScheduler();
        var owner = f.Owner(scheduler.Enqueue);
        var first = owner.Begin(f.Target(owner));
        owner.InvalidatePresentation();
        scheduler.Take()();
        await first.Completion!;
        var painted = false;
        Assert.IsFalse(owner.Publish(first.Request!, _ => painted = true));
        Assert.IsFalse(painted);
        Assert.AreEqual(PerformanceOutcome.KnownNoncommit, owner.Terminals.Single().Outcome);
        Assert.AreEqual(PerformanceFailureCause.StaleBeforeInvocation, owner.Terminals.Single().Cause);
        Assert.AreEqual(0, f.Runtime.Calls);
        Assert.IsTrue(owner.RequiresReopen(f.A));
        Assert.IsTrue(owner.OpenProduction(f.A).IsSuccess);
        var second = owner.Begin(f.Target(owner));
        Assert.AreNotEqual(first.Request!.Id, second.Request!.Id);
        Assert.IsFalse(owner.Publish(first.Request, _ => Assert.Fail()));
        Assert.IsTrue(owner.IsBusy);
        scheduler.Take()(); await second.Completion!;
        Assert.IsTrue(owner.Publish(second.Request, _ => { }));
        Assert.AreEqual(1, f.Catalog.Commits);
    }

    [TestMethod]
    public async Task KnownSuccessSurvivesRenderFaultAndReentrantPublication()
    {
        using var f = new Fixture();
        var owner = f.Owner(work => work());
        var request = owner.Begin(f.Target(owner));
        var terminal = await request.Completion!;
        Assert.AreSame(terminal, owner.Terminals.Single());
        owner.Publish(request.Request!, _ =>
        {
            Assert.IsFalse(owner.Publish(request.Request!, _ => Assert.Fail()));
            Assert.AreEqual(PerformanceAdmission.Busy, owner.Begin(request.Request!.Target).Admission);
            throw new IOException("rendering failure after known success");
        });
        Assert.AreEqual(PerformanceOutcome.KnownSuccess, owner.LastPublication!.Terminal.Outcome);
        Assert.IsTrue(owner.LastPublication.RenderingFailed);
        Assert.IsFalse(owner.RequiresReopen(f.A));
        Assert.AreEqual(1, f.Catalog.Commits);
        Assert.AreEqual(1, owner.Query().CurrentProductionReplay!.AcceptedPerformanceHistory.Performances.Length);
    }

    [TestMethod]
    public async Task ViewModelNotificationFaultDoesNotLoseOrReclassifySuccess()
    {
        using var f = new Fixture();
        var scheduler = new ManualScheduler(); var owner = f.Owner(scheduler.Enqueue);
        var vm = new MainPageViewModel(PresentationStartupResult.Owned(owner));
        var target = vm.CapturePerformanceTarget(f.Scene, f.Actor)!;
        vm.PropertyChanged += (_, _) => throw new IOException("binding failure");
        var task = vm.RequestPerformanceAsync(target);
        scheduler.Take()(); await task;
        Assert.AreEqual(PerformanceOutcome.KnownSuccess, vm.LastPerformancePublication!.Terminal.Outcome);
        Assert.IsTrue(vm.LastPerformancePublication.RenderingFailed);
        Assert.IsFalse(owner.IsBusy);
    }

    [TestMethod]
    public async Task UncertaintySurvivesSwitchRecoverFailedOpenAndClearsOnlySameOrdinaryOpen()
    {
        using var f = new Fixture(); f.Catalog.AfterCommitInvalid = true;
        var owner = f.Owner(work => work());
        var first = owner.Begin(f.Target(owner)); await first.Completion!;
        owner.Publish(first.Request!, _ => { });
        Assert.AreEqual(PerformanceOutcome.OutcomeUnknown, owner.Terminals.Single().Outcome);
        Assert.AreEqual(1, f.Catalog.Commits); // A real journal append followed by typed Invalid.
        Assert.IsTrue(owner.RequiresReopen(f.A));
        Assert.AreEqual(PerformanceAdmission.ReopenRequired, owner.Begin(f.Target(owner)).Admission);
        owner.Navigate(ApplicationScope.Home); owner.OpenProduction(f.B);
        Assert.IsTrue(owner.RequiresReopen(f.A)); Assert.IsFalse(owner.RequiresReopen(f.B));
        f.Catalog.FailOpen = true;
        Assert.IsFalse(owner.OpenProduction(f.A).IsSuccess); Assert.IsTrue(owner.RequiresReopen(f.A));
        f.Catalog.FailOpen = false;
        owner.RecoverProduction(f.A); Assert.IsTrue(owner.RequiresReopen(f.A));
        Assert.ThrowsExactly<InvalidOperationException>(() => owner.CreateCharacter("blocked"));
        owner.OpenProduction(f.A); Assert.IsFalse(owner.RequiresReopen(f.A));
        Assert.AreEqual(PerformanceOutcome.OutcomeUnknown, owner.Terminals.Single().Outcome);
        Assert.AreEqual(1, owner.Query().CurrentProductionReplay!.AcceptedPerformanceHistory.Performances.Length);
    }

    [TestMethod]
    public async Task ClassificationsSeparateInvalidIncompatibleTechnicalIoAndKnownNoncommit()
    {
        foreach (var kind in new[] { "invalid", "incompatible", "technical", "io", "commit-io", "dispatch" })
        {
            using var f = new Fixture();
            if (kind == "invalid") f.Catalog.Failure = ProductAccessFailureKind.Invalid;
            if (kind == "incompatible") f.Catalog.Failure = ProductAccessFailureKind.Incompatible;
            if (kind == "technical") f.Runtime.Error = new InvalidOperationException("transport failure");
            if (kind == "io") f.Runtime.Error = new IOException("transport io");
            if (kind == "commit-io") f.Catalog.CommitError = new IOException("journal io");
            var owner = f.Owner(kind == "dispatch" ? _ => throw new InvalidOperationException("dispatch rejected") : work => work());
            var activation = owner.Begin(f.Target(owner)); var terminal = await activation.Completion!;
            owner.Publish(activation.Request!, _ => { });
            var expected = kind switch { "incompatible" => PerformanceOutcome.ProductIncompatible,
                "technical" or "io" => PerformanceOutcome.ExecutorFailure,
                "dispatch" => PerformanceOutcome.KnownNoncommit, _ => PerformanceOutcome.OutcomeUnknown };
            Assert.AreEqual(expected, terminal.Outcome, kind);
            Assert.AreEqual(kind is not ("technical" or "dispatch"), terminal.RequiresReopen, kind);
            Assert.AreEqual(0, f.Catalog.Commits, kind);
            var cause = kind switch { "invalid" => PerformanceFailureCause.ProductInvalid,
                "incompatible" => PerformanceFailureCause.ProductIncompatible,
                "technical" => PerformanceFailureCause.PerformerFault,
                "dispatch" => PerformanceFailureCause.DispatchRejected, _ => PerformanceFailureCause.InvocationIo };
            Assert.AreEqual(cause, terminal.Cause, kind);
            Assert.AreEqual(kind is "technical" or "io" or "dispatch" ? ProductCommitKnowledge.Noncommit : ProductCommitKnowledge.Unknown,
                terminal.CommitKnowledge, kind);
            if (kind is "technical" or "io" or "commit-io") Assert.IsNotNull(terminal.DiagnosticType);
        }
    }

    [TestMethod]
    public async Task DispatchThrowAfterAcceptanceDoesNotReleaseOrExecuteAgain()
    {
        using var f = new Fixture(); Action? saved = null;
        var owner = f.Owner(work => { saved = work; throw new IOException("enqueue failure"); });
        var activation = owner.Begin(f.Target(owner));
        var terminal = await activation.Completion!;
        Assert.AreEqual(PerformanceOutcome.KnownNoncommit, terminal.Outcome);
        saved!(); // Late accepted work is fenced by the once-only invocation guard.
        Assert.AreEqual(0, f.Runtime.Calls);
        owner.Publish(activation.Request!, _ => { });
    }

    [TestMethod]
    public async Task ReopenAndViewReplacementInvalidatePreviouslyCapturedTargets()
    {
        using var f = new Fixture(); var owner = f.Owner(work => work());
        var target = f.Target(owner); owner.OpenProduction(f.A);
        Assert.AreEqual(PerformanceAdmission.StaleTarget, owner.Begin(target).Admission);
        target = f.Target(owner); owner.OpenProduction(f.B);
        Assert.AreEqual(PerformanceAdmission.StaleTarget, owner.Begin(target).Admission);
        owner.OpenProduction(f.A); target = f.Target(owner); owner.InvalidatePresentation();
        Assert.AreEqual(PerformanceAdmission.StaleTarget, owner.Begin(target).Admission);
        var accepted = owner.Begin(f.Target(owner)); await accepted.Completion!;
        owner.Publish(accepted.Request!, _ => { });
        Assert.AreEqual(1, f.Catalog.Commits);
    }

    [TestMethod]
    public async Task AdaptersPreserveEmptyAndExactTextActorContextAndSeparateInterpreterBinding()
    {
        using var f = new Fixture(); f.Runtime.Text = "";
        var owner = f.Owner(work => work());
        var activation = owner.Begin(f.Target(owner)); var terminal = await activation.Completion!;
        owner.Publish(activation.Request!, _ => { });
        Assert.AreEqual("", terminal.Execution!.AcceptedPerformance.VisibleText);
        Assert.AreSame(f.Runtime.PerformanceRequest!.Context, f.Runtime.ConsequenceRequest!.Context);
        Assert.AreEqual("", f.Runtime.ConsequenceRequest.Performance.VisibleText);
        Assert.AreNotEqual(f.Runtime.PerformanceRequest.InvocationId, f.Runtime.ConsequenceRequest.InvocationId);
        Assert.AreEqual(f.Actor, f.Runtime.PerformanceRequest.Context.CharacterId);
        Assert.IsTrue(f.Runtime.PerformanceRequest.Context.Circumstances.IsEmpty);
        f.Runtime.Text = "  A line.\n";
        activation = owner.Begin(f.Target(owner)); terminal = await activation.Completion!;
        owner.Publish(activation.Request!, _ => { });
        Assert.AreEqual("  A line.\n", terminal.Execution!.AcceptedPerformance.VisibleText);
        Assert.AreEqual("A bounded circumstance.", f.Runtime.PerformanceRequest!.Context.Circumstances.Single().Text);
        owner.OpenProduction(f.A);
        Assert.AreEqual(2, owner.Query().CurrentProductionReplay!.AcceptedPerformanceHistory.Performances.Length);
    }

    [TestMethod]
    public async Task AdapterRejectsMissingInvalidUnattributedAndIncompleteOutputWithoutRepair()
    {
        foreach (var bad in new string?[] { null, " ", "\u200b", "e\u0301", "\ud800", "line\r\n" })
        {
            using var f = new Fixture(); f.Runtime.Text = bad;
            var owner = f.Owner(work => work()); var activation = owner.Begin(f.Target(owner));
            var terminal = await activation.Completion!; owner.Publish(activation.Request!, _ => { });
            Assert.AreEqual(PerformanceOutcome.ExecutorFailure, terminal.Outcome);
            Assert.AreEqual(0, f.Catalog.Commits);
        }
        foreach (var bad in new[] { "mismatch", "truncated", "refused", "zero", "multiple", "family", "blank" })
        {
            using var f = new Fixture(); f.Runtime.Mode = bad;
            var owner = f.Owner(work => work()); var activation = owner.Begin(f.Target(owner));
            var terminal = await activation.Completion!; owner.Publish(activation.Request!, _ => { });
            Assert.AreEqual(PerformanceOutcome.ExecutorFailure, terminal.Outcome, bad);
            Assert.AreEqual(0, f.Catalog.Commits, bad);
        }
    }

    private sealed class ManualScheduler
    {
        private readonly Queue<Action> _work = new();
        public void Enqueue(Action work) => _work.Enqueue(work);
        public Action Take() => _work.Dequeue();
    }

    // The only runtime implementation in this package lives in this test assembly.
    private sealed class SyntheticRuntime : IProductExecutionRuntime
    {
        public int Calls;
        public string? Text = "A line.";
        public string Mode = "";
        public Exception? Error;
        public Action? DuringPerform;
        public PerformerRequest? PerformanceRequest;
        public ConsequenceRequest? ConsequenceRequest;
        public PerformerResponse Perform(PerformerRequest request)
        {
            Calls++; PerformanceRequest = request;
            DuringPerform?.Invoke();
            if (Error is not null) throw Error;
            return new(Mode == "mismatch" ? Guid.NewGuid() : request.InvocationId,
                Mode == "truncated" ? RuntimeCompletion.Truncated : Mode == "refused" ? RuntimeCompletion.Refused : RuntimeCompletion.Completed, Text);
        }
        public ConsequenceResponse Interpret(ConsequenceRequest request)
        {
            Calls++; ConsequenceRequest = request;
            var consequence = new RuntimeConsequence(Mode == "family" ? "WorldTruth" : "CharacterCircumstance",
                Mode == "blank" ? " " : "A bounded circumstance.");
            return new(request.InvocationId, RuntimeCompletion.Completed,
                Mode == "zero" ? [] : Mode == "multiple" ? [consequence, consequence] : [consequence]);
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly string _root = Path.Combine(Path.GetTempPath(), "Kymaean-offline-" + Guid.NewGuid().ToString("N"));
        public ProductApplication App { get; }
        public FaultCatalog Catalog { get; }
        public SyntheticRuntime Runtime { get; } = new();
        public ProductionId A { get; }
        public ProductionId B { get; }
        public SceneId Scene { get; }
        public CharacterId Actor { get; }
        public CharacterId OtherActor { get; }
        public Fixture()
        {
            var real = new FileProductionCatalog(_root);
            var setup = ProductApplication.Start(real).Value;
            A = setup.CreateProduction("Same Production").Value.Id;
            B = setup.CreateProduction("Same Production").Value.Id;
            setup.OpenProduction(A);
            Actor = setup.CreateCharacter("Same Actor").Value.Character.Id;
            OtherActor = setup.CreateCharacter("Same Actor").Value.Character.Id;
            Scene = setup.EstablishScene([Actor, OtherActor]).Value.Scene.Id;
            Catalog = new FaultCatalog(real);
            App = ProductApplication.Start(Catalog).Value; App.OpenProduction(A);
        }
        public ProductOperationCoordinator Owner(Action<Action> scheduler) => ProductOperationCoordinator.ForTests(
            App, new ProductPerformer(Runtime), new ProductConsequenceInterpreter(Runtime), scheduler);
        public PerformanceTarget Target(ProductOperationCoordinator owner) => owner.CaptureTarget(Scene, Actor)!;
        public void Dispose() { if (Directory.Exists(_root)) Directory.Delete(_root, true); }
    }

    private sealed class FaultCatalog(FileProductionCatalog inner) : IProductionCatalog, IProductionPerformanceCommitter
    {
        public int Commits;
        public bool AfterCommitInvalid;
        public bool FailOpen;
        public ProductAccessFailureKind? Failure;
        public Exception? CommitError;
        public ProductAccessResult<IReadOnlyList<ProductionSummary>> ListProductions() => inner.ListProductions();
        public ProductAccessResult<ProductionReplayProjection> OpenProduction(ProductionId id) => FailOpen
            ? ProductAccessResult<ProductionReplayProjection>.Failure(ProductAccessFailureKind.Invalid) : inner.OpenProduction(id);
        public ProductAccessResult<ProductionReplayProjection> RecoverProduction(ProductionId id) => inner.RecoverProduction(id);
        public ProductAccessResult<ProductionReplayProjection> CommitAcceptedPerformance(ProductionId id,
            ProductionReplayProjection expected, AcceptedPerformance accepted)
        {
            if (CommitError is not null) throw CommitError;
            if (Failure is { } failure) return ProductAccessResult<ProductionReplayProjection>.Failure(failure);
            var result = inner.CommitAcceptedPerformance(id, expected, accepted);
            if (result.IsSuccess) Commits++;
            return AfterCommitInvalid ? ProductAccessResult<ProductionReplayProjection>.Failure(ProductAccessFailureKind.Invalid) : result;
        }
    }
}
