using System;
namespace XOCV.Interfaces
{
	public interface IOrientationHandler
	{
		void ForceLandscape();

		void ForcePortrait();

		void ForceNormal();
	}
}
