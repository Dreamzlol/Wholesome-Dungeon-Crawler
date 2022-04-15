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
    internal class Cache : ICycleable, ICache
    {

        private object cacheLock = new object();

        public bool IsInInstance { get; private set; }

        public Cache()
        {
        }

        public void Initialize()
        {
            //First Initalization of Variables
            IsInInstance = WTLocation.IsInInstance();
            //Beginning of Event Subscriptions
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
                IsInInstance = WTLocation.IsInInstance();
            }

        }
    }
}
