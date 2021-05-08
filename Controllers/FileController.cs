using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoxLearn.Main.Dto.File;
using FoxLearn.Main.IData.Interfaces;
using FoxLearn.SharedKernel.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoxLearn.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        #region -       ctor & props      -
        private readonly IFileRepository fileRepository;
        public FileController(IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetFiles(int? subjectId, int? id)
        {
            var result = await fileRepository.GetFiles(subjectId, id);

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
        public async Task<IActionResult> AddFile([FromForm] CreateFileDto fileDto)
        {
            var result = await fileRepository.AddFile(fileDto);

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

        [HttpPost]
        public async Task<IActionResult> RenameFile(RenameFileDto fileDto)
        {
            var result = await fileRepository.RenameFile(fileDto);

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

        [HttpPost]
        public async Task<IActionResult> RemoveFiles(List<int> DeletedFilesId)
        {
            var result = await fileRepository.RemoveFiles(DeletedFilesId);

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
