using System.Threading.Tasks;
using api.Controllers.Model;
using api.Models.Requests;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.Models.Common;
using TutorBits.Thumbnail;
using Utils.Common;

namespace tutorbits_api.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ThumbnailController : BaseModelController<Thumbnail, CreateThumbnailModel, UpdateThumbnailModel, ThumbnailViewModel>
    {
        private readonly ThumbnailService thumbnailService_;

        public ThumbnailController(IConfiguration configuration,
                            DBDataAccessService dbDataAccessService,
                            AccountAccessService accountAccessService,
                            ThumbnailService thumbnailService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
            thumbnailService_ = thumbnailService;
        }

        [Authorize]
        [HttpPost]
        public override async Task<IActionResult> Create([FromForm] CreateThumbnailModel createModel)
        {
            return await base.Create(createModel);
        }

        protected override async Task EnrichViewModel(ThumbnailViewModel viewModel, Thumbnail entity)
        {
            await base.EnrichViewModel(viewModel, entity);
            viewModel.Url = ProjectUrlGenerator.GenerateProjectThumbnailUrl(entity.Id, configuration_);
        }

        protected override async Task OnCreated(CreateThumbnailModel createModel, Thumbnail model)
        {
            using (var memoryStream = createModel.Thumbnail.OpenReadStream())
            {
                await thumbnailService_.UploadThumbnail(model.Id, memoryStream);
            }
        }

        protected override async Task EnrichModel(Thumbnail model, api.Controllers.Model.Action action)
        {
            await base.EnrichModel(model, action);

            switch (action)
            {
                case api.Controllers.Model.Action.Create:
                    model.Status = BaseState.Active;
                    break;
            }
        }
    }
}
