﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using xAPI.Client.Exceptions;
using xAPI.Client.Http;
using xAPI.Client.Http.Options;
using xAPI.Client.Requests;
using xAPI.Client.Resources;

namespace xAPI.Client.Endpoints.Impl
{
    internal class StatementsApi : IStatementsApi
    {
        private const string ENDPOINT = "statements";
        private const string XAPI_CONSISTENT_THROUGH_HEADER = "X-Experience-API-Consistent-Through";
        private readonly IHttpClientWrapper _client;

        public StatementsApi(IHttpClientWrapper client)
        {
            this._client = client;
        }

        #region IStatementsApi members

        async Task<Statement> IStatementsApi.Get(GetStatementRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            request.Validate();

            var options = new RequestOptions(ENDPOINT);
            this.CompleteOptions(options, request);

            HttpResult<Statement> result = await this._client.GetJson<Statement>(options);
            return result.Content;
        }

        async Task<bool> IStatementsApi.Put(PutStatementRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            request.Validate();

            var options = new RequestOptions(ENDPOINT) { NullValueHandling = NullValueHandling.Ignore };
            this.CompleteOptions(options, request);

            try
            {
                await this._client.PutJson(options, request.Statement);
                return true;
            }
            catch (ConflictException)
            {
                return false;
            }
        }

        async Task<bool> IStatementsApi.Post(PostStatementRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            request.Validate();

            var options = new RequestOptions(ENDPOINT) { NullValueHandling = NullValueHandling.Ignore };
            this.CompleteOptions(options, request);

            try
            {
                await this._client.PostJson(options, request.Statement);
                return true;
            }
            catch (ConflictException)
            {
                return false;
            }
        }

        async Task<StatementResult> IStatementsApi.GetMany(GetStatementsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            request.Validate();

            var options = new RequestOptions(ENDPOINT);
            this.CompleteOptions(options, request);

            HttpResult<StatementResult> result = await this._client.GetJson<StatementResult>(options);
            result.Content.ConsistentThrough = this.GetConsistentThroughHeader(result.Headers);
            return result.Content;
        }

        async Task<StatementResult> IStatementsApi.GetMore(Uri more)
        {
            if (more == null)
            {
                throw new ArgumentNullException(nameof(more));
            }
            if (more.IsAbsoluteUri)
            {
                throw new ArgumentException("The URI must be relative", nameof(more));
            }

            string endpoint = $"/{more.ToString().TrimStart('/')}";
            var options = new RequestOptions(endpoint);

            HttpResult<StatementResult> result = await this._client.GetJson<StatementResult>(options);
            result.Content.ConsistentThrough = this.GetConsistentThroughHeader(result.Headers);
            return result.Content;
        }

        async Task<bool> IStatementsApi.PostMany(PostStatementsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            request.Validate();

            throw new NotImplementedException();
        }

        #endregion

        #region Utils

        private void CompleteOptions(RequestOptions options, GetStatementRequest request)
        {
            if (request.StatementId.HasValue)
            {
                options.QueryStringParameters.Add("statementId", request.StatementId.Value.ToString());
            }
            if (request.VoidedStatementId.HasValue)
            {
                options.QueryStringParameters.Add("voidedStatementId", request.VoidedStatementId.Value.ToString());
            }

            foreach (string language in request.GetAcceptedLanguages())
            {
                options.CustomHeaders.Add("Accept-Language", language);
            }
        }

        private void CompleteOptions(RequestOptions options, PutStatementRequest request)
        {
            options.QueryStringParameters.Add("statementId", request.Statement.Id.ToString());
        }

        private void CompleteOptions(RequestOptions options, PostStatementRequest request)
        {
        }

        private void CompleteOptions(RequestOptions options, GetStatementsRequest request)
        {
            //TODO

            foreach (string language in request.GetAcceptedLanguages())
            {
                options.CustomHeaders.Add("Accept-Language", language);
            }
        }

        private DateTimeOffset GetConsistentThroughHeader(HttpResponseHeaders headers)
        {
            IEnumerable<string> values;
            if (!headers.TryGetValues(XAPI_CONSISTENT_THROUGH_HEADER, out values))
            {
                throw new LRSException($"Header {XAPI_CONSISTENT_THROUGH_HEADER} is missing from LRS response");
            }

            string header = values.First();
            DateTimeOffset date;
            if (!DateTimeOffset.TryParse(header, out date))
            {
                throw new LRSException($"Header {XAPI_CONSISTENT_THROUGH_HEADER} is not in a valid DateTime format");
            }

            return date;
        }

        #endregion
    }
}
