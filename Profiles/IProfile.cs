﻿using robotManager.Helpful;
using System.Collections.Generic;
using WholesomeDungeonCrawler.Profiles.Steps;

namespace WholesomeDungeonCrawler.Profiles
{
    public interface IProfile
    {
        IStep CurrentStep { get; }
        bool ProfileIsCompleted { get; }
        int MapId { get; }
        List<Vector3> DeathRunPathList { get; }

        void SetCurrentStep();
        void SetFirstLaunchStep();
    }
}
