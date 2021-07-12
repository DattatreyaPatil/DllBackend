using DynamicLoadingOfDllsApi.Helpers;
using DynamicLoadingOfDllsApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicLoadingOfDllsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicDllsController : ControllerBase
    {
        #region Variables
        private string pathOfFolderContainingDynmaicDlls = @"C:\Users\datts\Downloads\TestFolder";
        private List<string> listOfAllAvailableDllsInTheFolder = new List<string>();
        #endregion
        #region Http Actions
        [HttpGet]
        [Route("/details")]
        [EnableCors("AllowOrigin")]
        public ActionResult<DynamicDllDetailsModel[]> GetAllDllsPresentAndThierDetails()
        {
            try
            {
                if (Directory.Exists(pathOfFolderContainingDynmaicDlls))
                {
                    listOfAllAvailableDllsInTheFolder = Directory.GetFiles(pathOfFolderContainingDynmaicDlls, "*.dll", SearchOption.AllDirectories).ToList();
                    if (listOfAllAvailableDllsInTheFolder.Count > 0)
                    {
                        List<DynamicDllDetailsModel> listOfDllsAndThierDetails = new List<DynamicDllDetailsModel>();
                        foreach (string dllPath in listOfAllAvailableDllsInTheFolder)
                        {
                            listOfDllsAndThierDetails.Add(DllLoader.ParseDllAndExtractInformation(dllPath));
                        }
                        return Ok(listOfDllsAndThierDetails.ToArray());

                    }
                    return NotFound("No Dlls found in the folder");
                }
                return NotFound("Directory doesn't exists!!");
            }
            catch (Exception e)
            {
                return Problem(
                detail: e.StackTrace,
                title: e.Message);
            }

        }

        [HttpPost]
        [Route("/execute/{dllName}")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<string>> ExecuteDll()
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                string content = await reader.ReadToEndAsync();
                DynamicDllDetailsModel dllDetailsModel = JsonConvert.DeserializeObject<DynamicDllDetailsModel>(content);

                string returnText = (DllLoader.ParseAndExecuteDlls(dllDetailsModel.DllFullPath));
              
                return Ok(returnText);
            }
            catch (Exception e)
            {
                return Problem(
                detail: e.StackTrace,
                title: e.Message);
            }

        }

        #endregion
    }
}
