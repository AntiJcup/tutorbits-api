﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Models.Requests;
using api.Models.Updates;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.Models.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseRatingController<TModel, TCreateModel, TUpdateModel, TViewModel> : BaseModelController<TModel, TCreateModel, TUpdateModel, TViewModel>
        where TModel : Rating, new()
        where TCreateModel : CreateRatingModel<TModel>
        where TUpdateModel : UpdateRatingModel<TModel>
        where TViewModel : BaseViewModel<TModel>, new()
    {
        public BaseRatingController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    AccountAccessService accountAccessService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetCountForTarget([FromQuery] BaseState state, [FromQuery] Guid targetId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityCount = await dbDataAccessService_.CountAllBaseModel(
                state == BaseState.Undefined ?
                    (Expression<Func<TModel, Boolean>>)(m => m.TargetId == targetId) :
                    (Expression<Func<TModel, Boolean>>)(m => m.Status == state && m.TargetId == targetId));

            return new JsonResult(entityCount);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetScoreForTarget([FromQuery] BaseState state, [FromQuery] Guid targetId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dbDataAccessService_.GetAllBaseModel(
                state == BaseState.Undefined ?
                    (Expression<Func<TModel, Boolean>>)(m => m.TargetId == targetId) :
                    (Expression<Func<TModel, Boolean>>)(m => m.Status == state && m.TargetId == targetId));

            var score = 1;
            foreach (var rating in entities)
            {
                score += rating.Score;
            }

            return new JsonResult(score);
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<IActionResult> GetYourScoreForTarget([FromQuery] Guid targetId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dbDataAccessService_.GetAllBaseModel(
                    (Expression<Func<TModel, Boolean>>)(m => m.Owner == UserName && m.TargetId == targetId));
            return new JsonResult(entities.FirstOrDefault());
        }

        protected override async Task EnrichModel(TModel model, Action action)
        {
            await base.EnrichModel(model, action);

            switch (action)
            {
                case Action.Create:
                    model.Status = BaseState.Active;
                    break;
            }
        }

        //Overrides create to prevent duplicates
        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingModel = (await dbDataAccessService_.GetAllBaseModel(
                (Expression<Func<TModel, Boolean>>)(m => m.Owner == UserName && m.TargetId == createModel.TargetId)))
                .FirstOrDefault();
            if (existingModel == null)
            {
                return await base.Create(createModel);
            }

            return BadRequest("Rating already exists");
        }
    }
}
