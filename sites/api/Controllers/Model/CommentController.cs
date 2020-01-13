using System;
using System.Threading.Tasks;
using api.Models.Requests;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.Models.Common;
using TutorBits.Preview;
using TutorBits.Project;
using Utils.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : BaseModelController<Comment, CreateCommentModel, UpdateCommentModel, CommentViewModel>
    {
        private readonly PreviewService previewService_;
        private readonly ProjectService projectService_;

        public CommentController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    AccountAccessService accountAccessService,
                                    ProjectService projectService,
                                    PreviewService previewService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
            projectService_ = projectService;
            previewService_ = previewService;
        }

        [HttpGet]
        public IActionResult GetCommentTypes()
        {
            return new JsonResult(Enum.GetNames(typeof(CommentType)));
        }
    }
}
