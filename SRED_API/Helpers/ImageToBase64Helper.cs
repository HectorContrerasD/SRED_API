namespace SRED_API.Helpers
{
    public class ImageToBase64Helper
    {
        public static string ConvertBase64(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                byte[] imageArray = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageArray);
            }
            return "";
        }
    }
}
