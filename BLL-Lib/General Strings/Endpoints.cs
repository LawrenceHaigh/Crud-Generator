using Pluralize.NET;
using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.General_Strings
{
    public class Endpoints
    {
        private readonly Pluralizer _pluralizer;

        private readonly TextInfo culture = new CultureInfo("en-ZA", false).TextInfo;
        private string TableNameSingularized { get; set; }
        private string TableNameTitleCaseSingularized { get; set; }
        private string TableNameLowerSingularized { get; set; }
        private string? TableName { get; set; }

        public Endpoints(string tableName)
        {
            this.TableName = tableName;
            _pluralizer = new Pluralizer();

            TableNameSingularized = _pluralizer.Singularize(TableName);
            TableNameTitleCaseSingularized = _pluralizer.Singularize(culture.ToTitleCase(TableName ?? string.Empty));
            TableNameLowerSingularized = _pluralizer.Singularize(TableName?.ToLower());
        }

        public string Create(string apiAssembly, string contractsAssembly, string repositoriesAssembly)
        {   
            return $@"using FastEndpoints;
            using Microsoft.AspNetCore.Authentication.JwtBearer;
            using {contractsAssembly}.Contracts.{TableName};
            using {repositoriesAssembly}.Repositories.Interfaces;
            using System.Net;

            namespace {apiAssembly}.Endpoints.{TableName}
            {{
                public class CreateEndpoint : Endpoint<{TableNameSingularized}Request, HttpStatusCode>
                {{
                    private readonly I{TableNameSingularized}Repository _{TableNameTitleCaseSingularized}Repository;

                    public CreateEndpoint(I{TableNameSingularized}Repository {TableNameTitleCaseSingularized}Repository)
                    {{
                        _{TableNameTitleCaseSingularized}Repository = {TableNameTitleCaseSingularized}Repository;
                    }}

                    public override void Configure()
                    {{
                        Post(""/{TableNameLowerSingularized}/create"");
                        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
                        Policies(""Put your policy here"");
                    }}

                    public override async Task HandleAsync({TableNameSingularized}Request req, CancellationToken ct)
                    {{
                        try
                        {{
                            await _{TableNameLowerSingularized}Repository.Create(req);

                            await SendAsync(HttpStatusCode.OK, cancellation: ct);
                        }}
                        catch (Exception)
                        {{
                            await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                        }}
                    }}
                }}
            }}";
        }

        public string Update(string apiAssembly, string contractsAssembly, string repositoriesAssembly, string idType)
        {
            return $@"using FastEndpoints;
            using Microsoft.AspNetCore.Authentication.JwtBearer;
            using {contractsAssembly}.Contracts.{TableName};
            using {repositoriesAssembly}.Repositories.Interfaces;
            using System.Net;

            namespace {apiAssembly}.Endpoints.{TableName}
            {{
                public class UpdateEndpoint : Endpoint<{TableNameSingularized}Request, {TableNameSingularized}Response>
                {{
                    private readonly I{TableNameSingularized}Repository _{TableNameTitleCaseSingularized}Repository;

                    public UpdateEndpoint(I{TableNameSingularized}Repository {TableNameTitleCaseSingularized}Repository)
                    {{
                        _{TableNameTitleCaseSingularized}Repository = {TableNameTitleCaseSingularized}Repository;
                    }}

                    public override void Configure()
                    {{
                        Put(""/{TableNameLowerSingularized}/update/{{Id}}"");
                        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
                        Policies(""Put your policy here"");
                    }}

                    public override async Task HandleAsync({TableNameSingularized}Request req, CancellationToken ct)
                    {{
                        try
                        {{
                            string? id = Route<string>(""Id"");

                            if (id == null)
                            {{
                                await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                            }}

                            Response = await _{TableNameTitleCaseSingularized}Repository.Update(req, {idType}.Parse(id ?? throw new NullReferenceException()));

                            await SendAsync(Response, cancellation: ct);
                        }}
                        catch (Exception)
                        {{
                            await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                        }}
                    }}
                }}
            }}";
        }

        public string Delete(string apiAssembly, string repositoriesAssembly, string idType)
        {
            return $@"using FastEndpoints;
            using Microsoft.AspNetCore.Authentication.JwtBearer;
            using Microsoft.Extensions.Options;
            using {repositoriesAssembly}.Repositories.Interfaces;
            using System.Net;

            namespace {apiAssembly}.Endpoints.{TableName}
            {{
                public class DeleteByIdEndpoint : EndpointWithoutRequest<HttpStatusCode>
                {{
                    private readonly I{TableNameSingularized}Repository _{TableNameTitleCaseSingularized}Repository;

                    public DeleteByIdEndpoint(I{TableNameSingularized}Repository {TableNameTitleCaseSingularized}Repository)
                    {{
                        _{TableNameTitleCaseSingularized}Repository = {TableNameTitleCaseSingularized}Repository;
                    }}

                    public override void Configure()
                    {{
                        Delete(""/{TableNameLowerSingularized}/delete/{{Id}}"");
                        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
                        Policies(""Put your policy here"");
                    }}

                    public override async Task HandleAsync(CancellationToken ct)
                    {{
                        try
                        {{
                            string? id = Route<string>(""Id"");

                            if (id == null)
                            {{
                                await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                            }}

                            Response = await _{TableNameTitleCaseSingularized}Repository.Delete({idType}.Parse(id ?? throw new NullReferenceException()));

                            await SendAsync(Response, cancellation: ct);
                        }}
                        catch (Exception)
                        {{
                            await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                        }}
                    }}
                }}
            }}";
        }

        public string GetById(string apiAssembly, string contractsAssembly, string repositoriesAssembly, string idType)
        {
            return $@"using FastEndpoints;
            using {repositoriesAssembly}.Repositories.Interfaces;
            using {contractsAssembly}.Contracts.{TableName};
            using Microsoft.AspNetCore.Authentication.JwtBearer;
            using System.Net;

            namespace {apiAssembly}.Endpoints.{TableName}
            {{
                public class GetByIdEndpoint : EndpointWithoutRequest<{TableNameSingularized}Response>
                {{
                    private readonly I{TableNameSingularized}Repository _{TableNameTitleCaseSingularized}Repository;

                    public GetByIdEndpoint(I{TableNameSingularized}Repository {TableNameTitleCaseSingularized}Repository)
                    {{
                        _{TableNameTitleCaseSingularized}Repository = {TableNameTitleCaseSingularized}Repository;
                    }}

                    public override void Configure()
                    {{
                        Get(""/{TableNameLowerSingularized}/getbyid/{{Id}}"");
                        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
                        Policies(""Put your policy here"");
                    }}

                    public override async Task HandleAsync(CancellationToken ct)
                    {{
                        try
                        {{
                            string? id = Route<string>(""Id"");

                            if (id == null)
                            {{
                                await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                            }}

                            Response = await _{TableNameTitleCaseSingularized}Repository.GetByIdAsync({idType}.Parse(id ?? throw new NullReferenceException())) ?? new();

                            await SendAsync(Response, cancellation: ct);
                        }}
                        catch (Exception)
                        {{
                            await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                        }}
                    }}
                }}
            }}";
        }

        public string GetAll(string apiAssembly, string contractsAssembly, string repositoriesAssembly)
        {
            return $@"using FastEndpoints;
            using Microsoft.AspNetCore.Authentication.JwtBearer;
            using {contractsAssembly}.Contracts.{TableName};
            using {repositoriesAssembly}.Repositories.Interfaces;
            using System.Net;

            namespace {apiAssembly}.Endpoints.{TableName}
            {{
                public class GetAllEndpoint : EndpointWithoutRequest<IEnumerable<{TableNameSingularized}Response>>
                {{
                    private readonly I{TableNameSingularized}Repository _{TableNameTitleCaseSingularized}Repository;

                    public GetAllEndpoint(I{TableNameSingularized}Repository {TableNameTitleCaseSingularized}Repository)
                    {{
                        _{TableNameTitleCaseSingularized}Repository = {TableNameTitleCaseSingularized}Repository;
                    }}

                    public override void Configure()
                    {{
                        Get(""/{TableNameLowerSingularized}/getall"");
                        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
                        Policies(""Put your policy here"");
                    }}

                    public override async Task HandleAsync(CancellationToken ct)
                    {{
                        try
                        {{
                            Response = await _{TableNameTitleCaseSingularized}Repository.GetAll();

                            await SendAsync(Response, cancellation: ct);
                        }}
                        catch (Exception)
                        {{
                            await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                        }}
                    }}
                }}
            }}";
        }

        public string GetPaged(string apiAssembly, string contractsAssembly, string repositoriesAssembly)
        {
            return $@"using FastEndpoints;
            using {contractsAssembly}.Contracts.Common;
            using {contractsAssembly}.Contracts.{TableName};
            using {repositoriesAssembly}.Repositories.Interfaces;
            using System.Net;

            namespace {apiAssembly}.Endpoints.{TableName}
            {{
                public class GetPagedEndpoint : Endpoint<ContinuousPagingRequest, PagedDataResponse<{TableNameSingularized}Response>>
                {{
                    private readonly I{TableNameSingularized}Repository _{TableNameTitleCaseSingularized}Repository;

                    public GetAllCourselistEndpoint(I{TableNameSingularized}Repository {TableNameTitleCaseSingularized}Repository)
                    {{
                        _{TableNameTitleCaseSingularized}Repository = {TableNameTitleCaseSingularized}Repository;
                    }}

                    public override void Configure()
                    {{
                        Post(""/{TableNameLowerSingularized}/getpaged"");
                        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
                        Policies(""Put your policy here"");
                    }}

                    public override async Task HandleAsync (ContinuousPagingRequest req, CancellationToken ct)
                    {{
                        try
                        {{
                            Response = await _{TableNameTitleCaseSingularized}Repository.GetPaged(req);

                            await SendAsync(Response, cancellation: ct);
                        }}
                        catch (Exception)
                        {{
                            await SendErrorsAsync((int)HttpStatusCode.BadRequest, cancellation: ct);
                        }}
                    }}
                }}
            }}";
        }
    }
}
