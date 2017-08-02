using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Merlin.Profiles.Gatherer
{
    public enum Trigger
    {
        Restart,
        DiscoveredResource,
        DepletedResource,
        Overweight,
        EncounteredAttacker,
        EliminatedAttacker,
    }
}
