﻿using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
