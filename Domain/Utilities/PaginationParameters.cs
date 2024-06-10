﻿namespace Domain.Utilities
{
    public class PaginationParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string query { get; set; } = "";
    }
}