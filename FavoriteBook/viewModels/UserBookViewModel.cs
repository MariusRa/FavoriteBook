﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FavoriteBook.viewModels
{
    public class UserBookViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}