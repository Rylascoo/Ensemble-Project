namespace Ensemble.E0.Harness.Run;

internal static class ConfiguredRoleAttemptBoundary
{
    internal static RoleAttemptReceipt Accept(
        PreparedRoleAttempt expected,
        RoleAttemptReceipt receipt)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(receipt);

        if (!string.Equals(expected.AttemptId, receipt.AttemptId, StringComparison.Ordinal) ||
            !string.Equals(expected.IdentityHash, receipt.PreparedIdentityHash, StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-A provider receipt does not match the prepared attempt.");
        }

        if (!Enum.IsDefined(receipt.Outcome))
        {
            throw new E0AHarnessException("E0-A provider receipt outcome is invalid.");
        }

        if (receipt.Outcome == E0ARoleAttemptOutcome.Success)
        {
            var output = receipt.StructuredOutput;
            if (output is null ||
                receipt.Usage is null ||
                string.IsNullOrWhiteSpace(receipt.ResponseId) ||
                string.IsNullOrWhiteSpace(receipt.ReturnedModel) ||
                string.IsNullOrWhiteSpace(receipt.StructuredOutputHash) ||
                !string.Equals(
                    receipt.StructuredOutputHash,
                    PreparedRoleAttempt.LowerSha256(output),
                    StringComparison.Ordinal) ||
                receipt.DiagnosticCode is not null)
            {
                throw new E0AHarnessException("E0-A successful provider receipt is incomplete.");
            }

            receipt.Usage.Validate();
        }
        else
        {
            if (receipt.StructuredOutput is not null ||
                receipt.StructuredOutputHash is not null ||
                string.IsNullOrWhiteSpace(receipt.DiagnosticCode) ||
                (receipt.ResponseId is not null && string.IsNullOrWhiteSpace(receipt.ResponseId)) ||
                (receipt.ReturnedModel is not null && string.IsNullOrWhiteSpace(receipt.ReturnedModel)))
            {
                throw new E0AHarnessException("E0-A technical provider receipt metadata is invalid.");
            }

            receipt.Usage?.Validate();
        }

        return receipt;
    }
}
