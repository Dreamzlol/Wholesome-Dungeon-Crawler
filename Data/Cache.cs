﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholesomeToolbox;
using wManager.Events;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeonCrawler.Data
{
    internal class Cache
    {

        private bool isInInstance;

        private object cacheLock = new object();

        public Cache()
        {
        }

        public void Initialize()
        {
            isInInstance = WTLocation.IsInInstance();
            ObjectManagerEvents.OnObjectManagerPulsed += OnObjectManagerPulse;
            EventsLua.AttachEventLua("WORLD_MAP_UPDATE", m => CacheIsInInstance());
        }

        public void Dispose()
        {
            ObjectManagerEvents.OnObjectManagerPulsed -= OnObjectManagerPulse;
        }


        private void OnObjectManagerPulse()
        {
            lock (cacheLock)
            {
                
            }
        }

        private void CacheIsInInstance()
        {
            lock (cacheLock)
            {
                isInInstance = WTLocation.IsInInstance();
            }

        }
    }
}
