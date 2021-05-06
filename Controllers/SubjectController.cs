using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxLearn.Main.Dto.Subject;
using FoxLearn.Main.IData.Interfaces;
using FoxLearn.SharedKernel.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoxLearn.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        #region -       ctor & props      -
        private readonly ISubjectRepository subjectRepository;
        public SubjectController(ISubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAllSubjects()
        {
            var result = await subjectRepository.GetAllSubjects();

            switch (result.OperationResultType)
            {
                case OperationResultTypes.Exception:
                    return new JsonResult("Exeption") { StatusCode = 400 };

                case OperationResultTypes.Success:
                    return new JsonResult(result.IEnumerableResult) { StatusCode = 200 };

                case OperationResultTypes.NotExist:
                    return new JsonResult("Unkown Error") { StatusCode = 204 };

                case OperationResultTypes.Forbidden:
                    return new JsonResult("Forbidden") { StatusCode = 403 };

            }
            return new JsonResult(result.OperationResultMessage) { StatusCode = 500 };
        }


        [HttpPost]
        public async Task<IActionResult> SetSubject(SubjectDto subjectDto)
        {
            var result = await subjectRepository.SetSubject(subjectDto);

            switch (result.OperationResultType)
            {
                case OperationResultTypes.Exception:
                    return new JsonResult("Exeption") { StatusCode = 400 };

                case OperationResultTypes.Success:
                    return new JsonResult(result.Result) { StatusCode = 200 };

                case OperationResultTypes.NotExist:
                    return new JsonResult("Unkown Error") { StatusCode = 204 };

                case OperationResultTypes.Forbidden:
                    return new JsonResult("Forbidden") { StatusCode = 403 };

            }
            return new JsonResult(result.OperationResultMessage) { StatusCode = 500 };
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await subjectRepository.DeleteSubject(id);

            switch (result.OperationResultType)
            {
                case OperationResultTypes.Exception:
                    return new JsonResult("Exeption") { StatusCode = 400 };

                case OperationResultTypes.Success:
                    return new JsonResult(result.Result) { StatusCode = 200 };

                case OperationResultTypes.NotExist:
                    return new JsonResult("Unkown Error") { StatusCode = 204 };

                case OperationResultTypes.Forbidden:
                    return new JsonResult("Forbidden") { StatusCode = 403 };

            }
            return new JsonResult(result.OperationResultMessage) { StatusCode = 500 };
        }
    }
}
