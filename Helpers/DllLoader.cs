using DynamicLoadingOfDllsApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace DynamicLoadingOfDllsApi.Helpers
{
    public static class DllLoader
    {
        public static string ParseAndExecuteDlls(string pathOfDllToBeLoaded) 
        {
            Assembly myAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(@pathOfDllToBeLoaded);

            TypeInfo declaredFirstClass = myAssembly.DefinedTypes.FirstOrDefault();          
            MethodInfo declaredFirstProperty = declaredFirstClass.DeclaredMethods.Where(x => x.ReturnType.Equals(typeof(String))).FirstOrDefault();
           
            ConstructorInfo constructor = declaredFirstClass.GetConstructor(Type.EmptyTypes);
            object classObject = constructor.Invoke(new object[] { });

            MethodInfo methodInfo = declaredFirstClass.GetMethod(declaredFirstProperty.Name);
            string returnValue = (string)methodInfo.Invoke(classObject, new object[] {});
            return (returnValue);
        }

        public static DynamicDllDetailsModel ParseDllAndExtractInformation(string fullPathOfDll)
        {
            Assembly myAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(@fullPathOfDll);

            DynamicDllDetailsModel dynamicDllDetailsModel = new DynamicDllDetailsModel();
           
            
            FileInfo fileInfo = new FileInfo(@fullPathOfDll);
            string dllName = fileInfo.Name;
            dynamicDllDetailsModel.DllName = dllName;
            dynamicDllDetailsModel.DllFullPath = fullPathOfDll;
            dynamicDllDetailsModel.DllDeatils = new List<DllClassesAndTheirMethods>();

            IEnumerable<TypeInfo> declaredClasses = myAssembly.DefinedTypes;
            
            foreach (TypeInfo typeOfClass in declaredClasses)
            {
                DllClassesAndTheirMethods dllClassesAndTheirMethodsDetails = new DllClassesAndTheirMethods();
                dllClassesAndTheirMethodsDetails.DeclaredClass = typeOfClass.ToString();
                foreach (MethodInfo methodsOfClass in myAssembly.DefinedTypes.FirstOrDefault().DeclaredMethods)
                {
                    dllClassesAndTheirMethodsDetails.DeclaredMethodsOfClass.Add(methodsOfClass.Name.ToString());
                }
                
                dynamicDllDetailsModel.DllDeatils.Add(dllClassesAndTheirMethodsDetails);
                
            }
            return dynamicDllDetailsModel;

        }
    }
}
