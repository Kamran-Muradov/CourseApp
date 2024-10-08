﻿using Domain.Common;

namespace Domain.Models
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }
        public int StudentCount { get; set; }
        public List<Student> Students { get; set; }
    }
}
