﻿using System;
using System.Collections.Generic;

namespace BackgroundProcessing.ResourceHandlers
{
    class HasModuleBackgroundProcessing : ResourceModuleHandler
    {
        public string resourceName { get; private set; }
        public float resourceRate { get; private set; }

        public HasModuleBackgroundProcessing(string rn = "", float rr = 0)
        {
            resourceName = rn;
            resourceRate = rr;
        }

        public static bool HasBackgroundProcessingData(
            PartModule m,
            ProtoPartModuleSnapshot s,
            Dictionary<String, List<ResourceModuleHandler>> resourceData,
            HashSet<String> interestingResources
        )
        {
            bool active = false;

            Boolean.TryParse(s.moduleValues.GetValue("enabled"), out active);

            Log.Info("HasBackgroundProcessingData, enabled: " + active);
            if (active)
            {
                ModuleResourceConverter g = (ModuleResourceConverter)m;
                if (g.resHandler.inputResources.Count <= 0)
                {
                    foreach (ModuleResource gr in g.resHandler.outputResources)
                    {
                        if (interestingResources.Contains(gr.name)) { return true; }
                    }
                }
            }

            return false;
        }

        public static List<ResourceModuleHandler> GetBackgroundProcessingData(
            PartModule m,
            ProtoPartSnapshot part,
            Dictionary<String, List<ResourceModuleHandler>> resourceData,
            HashSet<String> interestingResources
        )
        {
            List<ResourceModuleHandler> ret = new List<ResourceModuleHandler>();
            ModuleResourceConverter g = (ModuleResourceConverter)m;

            if (g.resHandler.inputResources.Count <= 0)
            {
                foreach (ModuleResource gr in g.resHandler.outputResources)
                {
                    if (interestingResources.Contains(gr.name))
                    {
                        ret.Add(new ResourceConverter(gr.name, (float)gr.rate));
                    }
                }
            }

            return ret;
        }

        public override HashSet<ProtoPartResourceSnapshot> HandleResource(Vessel v, VesselData data, HashSet<ProtoPartResourceSnapshot> modified)
        {
            return AddResource(data, resourceRate * TimeWarp.fixedDeltaTime, resourceName, modified);
        }
    }
}