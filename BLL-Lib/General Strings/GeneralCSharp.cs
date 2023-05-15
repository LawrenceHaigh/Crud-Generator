using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.General_Strings
{
    public static class GeneralCSharp
    {
        public static string ClassNameSpace(string assemblyName, string folder, string classStr)
        {
            return @$"namespace {assemblyName}.{folder} 
            {{ 
                {classStr} 
            }}";
        }

        public static string InterfaceNameSpace(string assemblyName, string folder, string interfaceStr)
        {
            return @$"namespace {assemblyName}.{folder} 
            {{ 
                {interfaceStr} 
            }}";
        }

        public static string ClassInterface(string fileName) =>
            @$": I{fileName}";

        public static string Class(string protectionLevel, string fileName, string contentBody)
        {
            return @$"{protectionLevel} class {fileName}{ClassInterface(fileName)}
            {{
                {contentBody}
            }}";
        }

        public static string Interface(string protectionLevel, string fileName, string interfaceBody)
        {
            return @$"{protectionLevel} interface I{fileName}
            {{
                {interfaceBody}
            }}";
        }

        public static string ModelClass(string @namespace, string className, string properties)
        {
            return @$"using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
                      
            namespace {@namespace}
            {{
                public class {className}
                {{
                    {properties}
                }}
            }}";
        }

        public static string ModelPropery(string propertyName, string propertyType) =>
            @$"public {propertyType} {propertyName} {{ get; set; }}";

        public static string RepositoryClass(string @namespace, string modelAssembly, string servicesAssembly, string className, string tableNameSingularized, string functions)
        {
            return @$"using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using ToDataTable;
            using Dapper;
            using {modelAssembly}.Contracts.{className};
            using {servicesAssembly}.Data;
            using {servicesAssembly}.Repositories.Interfaces;
                      
            namespace {@namespace}
            {{
                public class {tableNameSingularized}Repository: I{tableNameSingularized}Repository
                {{
                    {functions}
                }}
            }}";
        }

        public static string RepositoryInterface(string @namespace, string modelAssembly, string className, string tableNameSingularized, string idType)
        {
            string functions = string.Empty;

            if (!string.IsNullOrEmpty(idType))
            {
                functions = $@"Task<HttpStatusCode> DeleteAsync({idType} id);
                    Task<{tableNameSingularized}Response> GetByIdAsync({idType} id);";
            }

            return @$"using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using {modelAssembly}.Contracts.{className};
                      
            namespace {@namespace}
            {{
                public interface I{tableNameSingularized}Repository
                {{
                    Task CreateAsync(List<{tableNameSingularized}Request> request);
                    Task UpdateAsync(List<{tableNameSingularized}Request> request);                    
                    Task<IEnumerable<{tableNameSingularized}Response>> GetAllAsync();
                    {functions}
                }}
            }}";
        }
    }
}
