using UIKit;
using Foundation;
using CoreGraphics;
using SDWebImage;

namespace XOCV.iOS
{
    [Register(nameof(CachedImageControl))]

	public class CachedImageControl : UIImageView
    {
		public string ImageUrl 
		{
			get
			{
				return imageUrl;
			}
			set
			{
				imageUrl = value;

				if (imageUrl != null)
				{
					if (imageUrl.StartsWith("http", System.StringComparison.InvariantCulture))
					{
						this.SetImage(new NSUrl(imageUrl));
					}
					else
					{
						this.SetImage(new NSUrl(imageUrl, true));
					}
				}

			}
		}
		
        public CachedImageControl()
        {
            Initialize();
        }

        public CachedImageControl(CGRect bounds) : base(bounds)
        {
            Initialize();
        }

        private void Initialize()
        {
            ContentMode = UIViewContentMode.ScaleAspectFit;
        }

		private string imageUrl;
    }
}