using CloudinaryDotNet.Actions;//This is to get ImageUploadResult

namespace RunGroopWebApp.Interfaces
{
    public interface IPhotoService
    {
        //IForm file ensures that when we upload file it has all types of property that we need for image.
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);//Here ImageUploadResult makes use of Cloudinary service.
        Task<DeletionResult> DeletePhotoAsync(string publicId);

    }
}
