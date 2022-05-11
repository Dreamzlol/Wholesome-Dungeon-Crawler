﻿using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholesomeDungeonCrawler.Data.Model;
using WholesomeDungeonCrawler.Helpers;
using WholesomeToolbox;
using wManager.Wow.Bot.Tasks;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeonCrawler.Profiles.Steps
{
    public class FollowUnitStep : Step
    {
        private FollowUnitModel _followUnitModel;

        public FollowUnitStep(FollowUnitModel followUnitModel)
        {
            _followUnitModel = followUnitModel;
        }

        public override void Run()
        {
            {
                WoWUnit foundUnit = _followUnitModel.FindClosest
                    ? FindClosestUnit(unit => unit.Entry == _followUnitModel.UnitId)
                    : ObjectManager.GetObjectWoWUnit().FirstOrDefault(unit => unit.Entry == _followUnitModel.UnitId);

                Vector3 myPosition = ObjectManager.Me.PositionWithoutType;

                if (foundUnit == null)
                {
                    if (myPosition.DistanceTo(_followUnitModel.ExpectedEndPosition) >= 15)
                    {
                        // Goto expected position
                        GoToTask.ToPosition(_followUnitModel.ExpectedStartPosition, 3.5f, false, context=> IsCompleted);
                    }
                    else if (_followUnitModel.SkipIfNotFound || myPosition.DistanceTo(_followUnitModel.ExpectedEndPosition) < 15)
                    {
                        Logger.LogDebug($"[Step {_followUnitModel.Name}]: Skipping unit {_followUnitModel.UnitId} because he's not here.");
                        IsCompleted = true;
                        return;
                    }
                }
                else
                {
                    if (myPosition.DistanceTo(_followUnitModel.ExpectedEndPosition) < 15)
                    {
                        Logger.LogDebug($"[Step {_followUnitModel.Name}]: Skipping Step with {_followUnitModel.UnitId} because we reached our Enddestination.");
                        IsCompleted = true;
                        return;
                    }
                    Vector3 targetPosition = foundUnit.PositionWithoutType;
                    float followDistance = 15;

                    if (!MovementManager.InMovement ||
                        MovementManager.CurrentMoveTo.DistanceTo(targetPosition) > followDistance)
                    {
                        GoToTask.ToPosition(targetPosition, 3.5f, false, context => ObjectManager.Me.PositionWithoutType.DistanceTo(targetPosition) < followDistance);
                    }
                }

                IsCompleted = false;


            }
        }
        private WoWUnit FindClosestUnit(Func<WoWUnit, bool> predicate, Vector3 referencePosition = null)
        { //this function calculates the flosest Unit
            //first clear ol foundUnit
            WoWUnit foundUnit = null;
            var distanceToUnit = float.MaxValue;
            //checks for a given reference position, if not there then use our position
            Vector3 position = referencePosition != null ? referencePosition : ObjectManager.Me.Position;
            //build a List of each Unit and their Distance
            foreach (WoWUnit unit in ObjectManager.GetObjectWoWUnit())
            {
                if (!predicate(unit)) continue;

                if (foundUnit == null)
                {
                    distanceToUnit = position.DistanceTo(unit.Position);
                    foundUnit = unit;
                }
                else
                {
                    //float currentDistanceToUnit = myPosition.DistanceTo(unit.PositionWithoutType);
                    //checks the Distance of the Unit to the given Position
                    float currentDistanceToUnit = WTPathFinder.CalculatePathTotalDistance(position, unit.PositionWithoutType);
                    if (currentDistanceToUnit < distanceToUnit)
                    {
                        foundUnit = unit;
                        distanceToUnit = currentDistanceToUnit;
                    }
                }
            }
            return foundUnit;
        }
    }
}
