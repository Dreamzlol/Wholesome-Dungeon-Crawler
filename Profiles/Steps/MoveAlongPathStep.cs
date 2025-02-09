﻿using robotManager.Helpful;
using System.Collections.Generic;
using System.Linq;
using WholesomeDungeonCrawler.Helpers;
using WholesomeDungeonCrawler.Models;
using WholesomeDungeonCrawler.ProductCache.Entity;
using WholesomeToolbox;
using wManager.Wow.Helpers;

namespace WholesomeDungeonCrawler.Profiles.Steps
{
    public class MoveAlongPathStep : Step
    {
        private MoveAlongPathModel _moveAlongPathModel;
        private readonly IEntityCache _entityCache;

        public override string Name { get; }
        public override int Order { get; }

        public MoveAlongPathStep(MoveAlongPathModel stepModel, IEntityCache entityCache)
        {
            _moveAlongPathModel = stepModel;
            _entityCache = entityCache;
            Name = stepModel.Name;
            Order = stepModel.Order;
        }

        public override void Run()
        {
            if (GetMoveAlongPath.Count <= 0)
            {
                Logger.LogError($"Step {Name} path is empty, skipping.");
                IsCompleted = true;
                return;
            }

            Vector3 lastPointOfPath = GetMoveAlongPath.Last();

            if (_entityCache.Me.PositionWithoutType.DistanceTo(lastPointOfPath) < 5f)
            {
                if (_moveAlongPathModel.CompleteCondition == null
                    || !_moveAlongPathModel.CompleteCondition.HasCompleteCondition
                    || EvaluateCompleteCondition(_moveAlongPathModel.CompleteCondition))
                {
                    IsCompleted = true;
                    return;
                }
            }

            if (!MovementManager.InMovement && !_entityCache.Me.Dead)
            {
                List<Vector3> pathToFollow = WTPathFinder.PathFromClosestPoint(GetMoveAlongPath);
                MovementManager.Go(pathToFollow);
            }
        }

        public List<Vector3> GetMoveAlongPath => _moveAlongPathModel.Path;
    }
}
