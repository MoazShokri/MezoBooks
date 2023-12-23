﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MezoBooks.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required]
		[DisplayName("Category Name")]
		public string Name { get; set; }
		[DisplayName("Display Order")]
		[Range(1, 100, ErrorMessage = "Display Order Must be Between 1-100 ")]
		public int DisplayOrder { get; set; }
	}
}
