using System;
using FFImageLoading.Work;

namespace XOCV.Interfaces
{
	public interface IVectorImageSource
	{
		IVectorDataResolver GetVectorDataResolver();

		Xamarin.Forms.ImageSource ImageSource { get; }

		int VectorWidth { get; set; }

		int VectorHeight { get; set; }

		bool UseDipUnits { get; set; }
	}

	public interface IVectorDataResolver : IDataResolver
	{
		int VectorWidth { get; }

		int VectorHeight { get; }

		bool UseDipUnits { get; }
	}
}
