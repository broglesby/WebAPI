﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace MyServices.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool Deleted { get; set; }
        public string By { get; set; }
        public int Time { get; set; }
        public string Text { get; set; }
        public bool Dead { get; set; }
        public string Parent { get; set; }
        public string Poll { get; set; }
        public List<int> Kids { get; set; }
        public string URL { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public List<int> Parts { get; set; }
        public int Descendants { get; set; }
    }
}
