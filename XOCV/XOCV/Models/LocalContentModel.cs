using System;
using SQLite.Net.Attributes;

namespace XOCV.Models
{
	public class LocalContentModel
	{
		[AutoIncrement, PrimaryKey]
		public int ID { get; set; }
		public string LocalContent { get; set; }
	}
}