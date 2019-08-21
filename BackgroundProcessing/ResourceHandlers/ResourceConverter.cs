using System;
using System.Collections.Generic;

namespace BackgroundProcessing.ResourceHandlers
{
    class ResourceConverter : ResourceModuleHandler
    {
        public string resourceName { get; private set; }
        public float resourceRate { get; private set; }

        public ResourceConverter(string rn = "", float rr = 0)
        {
            resourceName = rn;
            resourceRate = rr;
        }

        public static bool HasResourceGenerationData(
            PartModule m,
            ProtoPartModuleSnapshot s,
            Dictionary<String, List<ResourceModuleHandler>> resourceData,
            HashSet<String> interestingResources
        )
        {
            bool active = false;
            Boolean.TryParse(s.moduleValues.GetValue("IsActivated"), out active);
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

        public static List<ResourceModuleHandler> GetResourceGenerationData(
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