namespace XOCV.Helpers
{
    #region Base Options
    public class MediaStorageOptions
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public int? MaxPixelDimension { get; set; }
        public int? PercentQuality { get; set; }

        protected MediaStorageOptions() {}
    }
    #endregion

    #region Camera Options
    public enum CameraDevice
    {
        Rear,
        Front
    }
    
    public class CameraMediaStorageOptions : MediaStorageOptions
    {
        public CameraDevice DefaultCamera { get; set; }
        public bool SaveMediaOnCapture { get; set; }

        public CameraMediaStorageOptions()
        {
            SaveMediaOnCapture = true;
        }
    }
    #endregion
}