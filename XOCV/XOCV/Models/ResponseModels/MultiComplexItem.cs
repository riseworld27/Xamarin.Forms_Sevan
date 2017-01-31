using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XOCV.Models.ResponseModels
{
	public class MultiComplexItem
	{
		[JsonProperty(PropertyName = "complexItems")]
		public List<ComplexItem> ComplexItems { get; set; }

		public MultiComplexItem()
		{
			ComplexItems = new List<ComplexItem> ();
		}
	}
}
