﻿using System.ComponentModel.DataAnnotations;

namespace squittal.ScrimPlanetmans.ScrimMatch.Models
{
    public class ScrimActionRulePoints
    {
        [Required]
        public ScrimActionRule ActionRule { get; set; }

        public int Points { get; set; }
    }
}
