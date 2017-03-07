﻿using System;
using xAPI.Client.Resources;

namespace xAPI.Client.Requests
{
    public class GetActivityRequest : ARequest
    {
        public GetActivityRequest()
        {
        }

        public GetActivityRequest(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }

            this.ActivityId = activity.Id;
        }

        public Uri ActivityId { get; set; }

        internal override void Validate()
        {
            if (this.ActivityId == null)
            {
                throw new ArgumentNullException(nameof(this.ActivityId));
            }
            if (!this.ActivityId.IsAbsoluteUri)
            {
                throw new ArgumentException("IRI should be absolute", nameof(this.ActivityId));
            }
        }
    }
}
