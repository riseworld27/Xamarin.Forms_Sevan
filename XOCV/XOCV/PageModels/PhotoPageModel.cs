using System;
using XOCV.Models.ResponseModels;
using XOCV.ViewModels.Base;

namespace XOCV.PageModels
{
	public class PhotoPageModel : BasePageModel
	{
		public string Media { get; set; }

		public override void Init(object initData)
		{
			base.Init(initData);
			Media = initData as string;
		}
		public PhotoPageModel()
		{
		}
	}
}
