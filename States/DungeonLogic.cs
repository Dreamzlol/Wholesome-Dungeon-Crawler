﻿using robotManager.FiniteStateMachine;
using WholesomeDungeonCrawler.Helpers;
using WholesomeDungeonCrawler.Managers;
using WholesomeDungeonCrawler.ProductCache;
using WholesomeDungeonCrawler.ProductCache.Entity;
using wManager.Wow.Helpers;

namespace WholesomeDungeonCrawler.States
{
    class DungeonLogic : State, IState
    {
        public override string DisplayName => "DungeonLogic";

        private readonly IEntityCache _entityCache;
        private readonly IProfileManager _profileManager;
        private readonly IPartyChatManager _partyChatManager;
        private readonly ICache _cache;

        public DungeonLogic(IEntityCache iEntityCache,
            IProfileManager profilemanager,
            IPartyChatManager partyChatManager,
            ICache cache)
        {
            _entityCache = iEntityCache;
            _profileManager = profilemanager;
            _partyChatManager = partyChatManager;
            _cache = cache;
        }
        public override bool NeedToRun
        {
            get
            {

                if (!Conditions.InGameAndConnected
                    || !_entityCache.Me.Valid
                    || Fight.InFight
                    || !_cache.IsInInstance
                    || _profileManager.CurrentDungeonProfile == null
                    || _profileManager.CurrentDungeonProfile.CurrentStep == null
                    || _profileManager.CurrentDungeonProfile.CurrentStep.Order > _partyChatManager.TankStatus?.StepOrder)
                {
                    return false;
                }

                return true;
            }
        }

        public override void Run()
        {
            if (_profileManager.CurrentDungeonProfile.CurrentStep.IsCompleted)
            {
                _profileManager.CurrentDungeonProfile.SetCurrentStep();
            }
            else
            {
                //Logger.Log($"DungeonLogic: {_profileManager.CurrentDungeonProfile.CurrentStep.Name}");
                _profileManager.CurrentDungeonProfile.CurrentStep.Run();
            }
        }
    }
}
