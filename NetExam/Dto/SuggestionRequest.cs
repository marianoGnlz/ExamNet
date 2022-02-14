namespace NetExam.Dto
{
    using System;
    using System.Collections.Generic;

    public class SuggestionRequest
    {
        public int CapacityNeeded { get; }
        public string PreferedNeigborHood { get; }
        public IEnumerable<string> ResourcesNeeded { get; }

        public SuggestionRequest(int capacityNeeded, IEnumerable<string> resourcesNeeded)
            : this(capacityNeeded, null, resourcesNeeded)
        {
        }

        public SuggestionRequest(int capacityNeeded, string preferedNeigborHood, IEnumerable<string> resourcesNeeded)
        {
            if (capacityNeeded <= 0) throw new ArgumentOutOfRangeException(nameof(capacityNeeded));

            CapacityNeeded = capacityNeeded;
            PreferedNeigborHood = preferedNeigborHood;
            ResourcesNeeded = resourcesNeeded ?? new List<string>();
        }

        public void Deconstruct(out int capacityNeeded, out string preferedNeigborHood, out IEnumerable<string> resourcesNeeded)
        {
            capacityNeeded = CapacityNeeded;
            preferedNeigborHood = PreferedNeigborHood;
            resourcesNeeded = ResourcesNeeded;

        }
    }
}