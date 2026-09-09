using System.Net;
using System.Text;
using Ensemble.E0.Harness.Gemini;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ensemble.E0.Harness.Tests;

[TestClass]
public sealed class GeminiCountTokensFailureDiagnosticTests
{
    private static readonly byte[] RequestBody = Encoding.UTF8.GetBytes(
        "{\"generateContentRequest\":{\"model\":\"models/gemini-3.5-flash-lite\"," +
        "\"contents\":[{\"role\":\"user\",\"parts\":[{\"text\":\"hello\"}]}]," +
        "\"generationConfig\":{\"candidateCount\":1,\"maxOutputTokens\":4096," +
        "\"responseFormat\":{\"text\":{\"mimeType\":\"application/json\"," +
        "\"schema\":{\"type\":\"object\"}}}}}}"
    );

    [TestMethod]
    public async Task MalformedErrorJson_FallsBackToHttpOnly()
    {
        var diagnostic = await DiagnosticAsync("{not-json");

        Assert.AreEqual("gemini-counttokens-http-400", diagnostic);
    }

    [TestMethod]
    public async Task OversizedErrorBody_FallsBackToHttpOnly()
    {
        var diagnostic = await DiagnosticAsync(new string('x', (16 * 1024) + 1));

        Assert.AreEqual("gemini-counttokens-http-400", diagnostic);
    }

    [TestMethod]
    public async Task UnknownStatusAndUnresolvedFields_AreOmittedWithProviderProse()
    {
        const string secret = "secret-provider-prose-must-not-persist";
        var body = "{\"error\":{\"code\":400,\"message\":\"" + secret +
            "\",\"status\":\"SECRET_STATUS\",\"details\":[" +
            "{\"@type\":\"type.googleapis.com/google.rpc.BadRequest\",\"fieldViolations\":[" +
            "{\"field\":\"generateContentRequest.generationConfig.notActuallyPresent\",\"description\":\"" + secret + "\"}," +
            "{\"field\":\"generateContentRequest.contents[9].parts[0].text\",\"description\":\"" + secret + "\"}" +
            "]}," +
            "{\"@type\":\"type.googleapis.com/google.rpc.ErrorInfo\",\"reason\":\"" + secret + "\"}" +
            "]}}";

        var diagnostic = await DiagnosticAsync(body);

        Assert.AreEqual("gemini-counttokens-http-400", diagnostic);
        Assert.IsFalse(diagnostic.Contains(secret, StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task ValidFields_AreDeduplicatedOrdinalSortedAndCappedAtFour()
    {
        var body = "{\"error\":{\"status\":\"INVALID_ARGUMENT\",\"details\":[" +
            "{\"@type\":\"type.googleapis.com/google.rpc.BadRequest\",\"fieldViolations\":[" +
            "{\"field\":\"generateContentRequest.model\"}," +
            "{\"field\":\"generateContentRequest.generationConfig.responseFormat.text.schema\"}," +
            "{\"field\":\"generateContentRequest.generationConfig.maxOutputTokens\"}," +
            "{\"field\":\"generateContentRequest.contents[0].parts[0].text\"}," +
            "{\"field\":\"generateContentRequest.generationConfig.responseFormat.text.mimeType\"}," +
            "{\"field\":\"generateContentRequest.generationConfig.candidateCount\"}," +
            "{\"field\":\"generateContentRequest.generationConfig.candidateCount\"}" +
            "]}]}}";

        var diagnostic = await DiagnosticAsync(body);

        Assert.AreEqual(
            "gemini-counttokens-http-400;status=INVALID_ARGUMENT" +
            ";field=generateContentRequest.contents[0].parts[0].text" +
            ";field=generateContentRequest.generationConfig.candidateCount" +
            ";field=generateContentRequest.generationConfig.maxOutputTokens" +
            ";field=generateContentRequest.generationConfig.responseFormat.text.mimeType",
            diagnostic);
    }

    [TestMethod]
    public async Task SnakeCaseFieldPath_ResolvesAgainstExactCamelCaseRequest()
    {
        var body = "{\"error\":{\"status\":\"INVALID_ARGUMENT\",\"details\":[" +
            "{\"@type\":\"type.googleapis.com/google.rpc.BadRequest\",\"fieldViolations\":[" +
            "{\"field\":\"generate_content_request.generation_config.max_output_tokens\"}" +
            "]}]}}";

        var diagnostic = await DiagnosticAsync(body);

        Assert.AreEqual(
            "gemini-counttokens-http-400;status=INVALID_ARGUMENT" +
            ";field=generate_content_request.generation_config.max_output_tokens",
            diagnostic);
    }

    private static async Task<string> DiagnosticAsync(string body)
    {
        using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        var failure = await E0AGeminiCountTokensFailureException.FromHttpResponseAsync(
            response,
            RequestBody,
            CancellationToken.None);
        return failure.Diagnostic;
    }
}
