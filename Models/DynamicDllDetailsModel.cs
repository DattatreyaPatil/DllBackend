using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicLoadingOfDllsApi.Models
{
    public class DynamicDllDetailsModel
    {
        public string DllName { get; set; }

        public string DllFullPath { get; set; }

        public List<DllClassesAndTheirMethods> DllDeatils { get; set; }
    }

    public class DllClassesAndTheirMethods 
    {
        public string DeclaredClass { get; set; }

        public List<string> DeclaredMethodsOfClass { get; set; } = new List<string>();
    }
}
