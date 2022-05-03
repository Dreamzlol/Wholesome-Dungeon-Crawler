﻿using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using System.Collections.Generic;
using System.Linq;
using WholesomeDungeonCrawler.Data;
using WholesomeDungeonCrawler.Dungeonlogic;
using WholesomeToolbox;
using wManager.Wow.Helpers;

namespace WholesomeDungeonCrawler.States.ProfileStates
{
    class SExecute : State
    {
        private readonly ICache _cache;
        private readonly IEntityCache _entityCache;
        private readonly IProfile _profile;
        public SExecute(ICache iCache, IEntityCache iEntityCache, int priority)
        {
            _cache = iCache;
            _entityCache = iEntityCache;
            Priority = priority;
        }
        public override bool NeedToRun
        {
            get
            {
                if (!Conditions.InGameAndConnected
                    || !_entityCache.Me.Valid
                    || Fight.InFight)
                {
                    return false;
                }

                return _profile.CurrentStepType.Contains("Execute");
            }
        }

        public override void Run()
        {

        }
    }
}
