﻿using System.Collections.Generic;
using System.Linq;
using WholesomeDungeonCrawler.Data;
using WholesomeDungeonCrawler.Data.Model;
using WholesomeDungeonCrawler.Helpers;
using WholesomeDungeonCrawler.Profiles.Steps;

namespace WholesomeDungeonCrawler.Profiles
{
    public class Profile : IProfile
    {
        private readonly IEntityCache _entityCache;
        private List<IStep> _profileSteps = new List<IStep>();

        public IStep CurrentStep { get; private set; }

        public Profile(ProfileModel profileModel, IEntityCache entityCache)
        {
            _entityCache = entityCache;
            foreach (StepModel model in profileModel.StepModels)
            {
                if (model is MoveAlongPathModel)
                {
                    _profileSteps.Add(new MoveAlongPathStep((MoveAlongPathModel)model, _entityCache));
                }
                else if (model is GoToModel)
                {
                    _profileSteps.Add(new GoToStep((GoToModel)model, _entityCache));
                }
                else if(model is ExecuteModel)
                {
                    _profileSteps.Add(new ExecuteStep((ExecuteModel)model, _entityCache));
                }
                else if(model is InteractWithModel)
                {
                    _profileSteps.Add(new InteractWithStep((InteractWithModel)model, _entityCache));
                }
                else if(model is MoveToUnitModel)
                {
                    _profileSteps.Add(new MoveToUnitStep((MoveToUnitModel)model, _entityCache));
                }
                else if(model is PickupObjectModel)
                {
                    _profileSteps.Add(new PickupObjectStep((PickupObjectModel)model, _entityCache));
                }
                else if(model is FollowUnitModel)
                {
                    _profileSteps.Add(new FollowUnitStep((FollowUnitModel)model, _entityCache));
                }
                else if(model is DefendSpotModel)
                {
                    _profileSteps.Add(new DefendSpotStep((DefendSpotModel)model, _entityCache));
                }
                //elseif...
            }
        }

        public void Dispose()
        {
        }

        public void ExecuteSteps()
        {
            var totalSteps = _profileSteps.Count();
            if (totalSteps <= 0)
            {
                Logger.Log("Profile is missing Profile Steps.");
                return;
            }
            var incompleteSteps = _profileSteps.Count(s => !s.IsCompleted);
            var completedSteps = _profileSteps.Count(s => s.IsCompleted);
            if (CurrentStep == null)
            {
                CurrentStep = _profileSteps[0];
            }
            //if (!CurrentStep.IsCompleted)
            //{
            //    CurrentStepType = CurrentStep.StepType;
            //    return;
            //}
            if (CurrentStep.IsCompleted)
            {
                if (completedSteps == totalSteps)
                {
                    Logger.Log("Profile is Done");
                    // Exit the dungeon / Unload profile etc...
                    return;
                }

                CurrentStep = _profileSteps.Find(step => !step.IsCompleted);
            }
        }
    }
}
