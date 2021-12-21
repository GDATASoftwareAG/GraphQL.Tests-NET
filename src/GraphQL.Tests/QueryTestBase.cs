using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Execution;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using GraphQLParser.Exceptions;
using Shouldly;

namespace GraphQL.Tests;

public class QueryTestBase<TSchema, TDocumentBuilder>
    where TSchema : ISchema
    where TDocumentBuilder : IDocumentBuilder, new()
{
    public QueryTestBase()
    {
        Services = new SimpleContainer();
        Executer = new DocumentExecuter(new TDocumentBuilder(), new DocumentValidator(), new ComplexityAnalyzer());
        Writer = new DocumentWriter(true);
    }

    public ISimpleContainer Services { get; }

    public TSchema Schema => Services.Get<TSchema>();

    public IDocumentExecuter Executer { get; }

    public IDocumentWriter Writer { get; }

    public Task AssertQuerySuccessAsync(
        string query,
        string expected,
        Inputs inputs = null,
        object root = null,
        IDictionary<string, object> userContext = null,
        CancellationToken cancellationToken = default,
        IEnumerable<IValidationRule> rules = null)
    {
        var queryResult = CreateQueryResult(expected);
        return AssertQueryAsync(query, queryResult, inputs, root, userContext, cancellationToken, rules);
    }

    public Task AssertQueryWithErrorsAsync(
        string query,
        string expected,
        Inputs inputs = null,
        object root = null,
        IDictionary<string, object> userContext = null,
        CancellationToken cancellationToken = default,
        int expectedErrorCount = 0,
        bool renderErrors = false)
    {
        var queryResult = CreateQueryResult(expected);
        return AssertQueryIgnoreErrorsAsync(
            query,
            queryResult,
            inputs,
            root,
            userContext,
            cancellationToken,
            expectedErrorCount,
            renderErrors);
    }

    public Task AssertQueryIgnoreErrorsAsync(
        string query,
        ExecutionResult expectedExecutionResult,
        Inputs inputs = null,
        object root = null,
        IDictionary<string, object> userContext = null,
        CancellationToken cancellationToken = default,
        int expectedErrorCount = 0,
        bool renderErrors = false)
    {
        return AssertQueryAsync(query, expectedExecutionResult, inputs, root, userContext, cancellationToken,
            null, expectedErrorCount, renderErrors);
    }

    public async Task AssertQueryAsync(
        string query,
        ExecutionResult expectedExecutionResult,
        Inputs inputs,
        object root,
        IDictionary<string, object> userContext = null,
        CancellationToken cancellationToken = default,
        IEnumerable<IValidationRule> rules = null,
        int expectedErrorCount = 0,
        bool renderErrors = true)
    {
        var runResult = await ExecuteAsync(query, inputs, root, userContext, cancellationToken, rules);

        var renderResult = renderErrors ? runResult : new ExecutionResult { Data = runResult.Data };

        var writtenResult = await Writer.WriteToStringAsync(runResult);
        var expectedResult = await Writer.WriteToStringAsync(expectedExecutionResult);


        string additionalInfo = null;
        if (renderErrors)
        {
            if (renderResult.Errors?.Any() == true)
            {
                additionalInfo = string.Join(Environment.NewLine, renderResult.Errors
                    .Where(x => x.InnerException is GraphQLSyntaxErrorException)
                    .Select(x => x.InnerException.Message));
            }
            writtenResult.ShouldBe(expectedResult.Replace("\\u002B", "+"), additionalInfo);
        }
        else
        {
            var errors = renderResult.Errors ?? new ExecutionErrors();
            errors.Count().ShouldBe(expectedErrorCount);
        }
    }

    private Task<ExecutionResult> ExecuteAsync(string query, Inputs inputs, object root, IDictionary<string, object> userContext,
        CancellationToken cancellationToken, IEnumerable<IValidationRule> rules)
    {
        return Executer.ExecuteAsync(_ =>
        {
            _.Schema = Schema;
            _.Query = query;
            _.Root = root;
            _.Inputs = inputs;
            _.UserContext = userContext;
            _.CancellationToken = cancellationToken;
            _.ValidationRules = rules;
        });
    }

    public static ExecutionResult CreateQueryResult(string result, ExecutionErrors errors = null)
        => result.ToExecutionResult(errors);
}
