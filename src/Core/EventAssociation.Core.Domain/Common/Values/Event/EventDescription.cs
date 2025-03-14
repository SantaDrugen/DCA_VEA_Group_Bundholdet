using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAssociation.Core.Domain.Common.Values.Event
{
    internal class EventDescription
    {
        private string value { get; }

        public EventDescription(string value)
        {
            this.value = value;
        }
    }
}
