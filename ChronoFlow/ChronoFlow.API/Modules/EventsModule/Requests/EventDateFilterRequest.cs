﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.Modules.EventsModule.Response
{
    public class EventDateFilterRequest
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
