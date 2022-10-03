using CactusProject.Helper;
using CactusProject.Services.UploadFiless.IService;

namespace CactusProject.Services.UploadFiless
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UploadFileService(IWebHostEnvironment webHostEnvironment) => _webHostEnvironment = webHostEnvironment;

        public Task DeleteImage(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var Path = $"{_webHostEnvironment.WebRootPath}/images/reviews/";
                string fullName = Path + filename;
                if (File.Exists(fullName)) File.Delete(fullName);
            }
            return Task.CompletedTask;
        }

        public bool IsUpload(IFormFileCollection formFiles) => formFiles == null ? false : formFiles.Count > 0;

        public async Task<List<string>> UploadImages(IFormFileCollection formFiles)
        {
            var listFileName = new List<string>();
            var uploadPath = $"{_webHostEnvironment.WebRootPath}/images/reviews/";
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            foreach (var formFile in formFiles)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                string fullName = uploadPath + fileName;

                using (var stream = File.Create(fullName)) await formFile.CopyToAsync(stream);
                listFileName.Add(fileName);
            }
            return listFileName;
        }

        public string Validation(IFormFileCollection formFiles)
        {
            foreach (var file in formFiles)
            {
                if (!ValidationExtension(file.FileName)) return "Invalid file  " + String.Join(", ", Constants.TypeImageForUploads); ;

                if (!ValidationSize(file.Length)) return "The file is too large " + (double)Constants.FileSizeLimit / 1024 / 1024 + "M";
            }
            return null!;
        }

        public bool ValidationExtension(string filename)
        {
            string[] permittedExtensions = Constants.TypeImageForUploads;
            string extension = Path.GetExtension(filename).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension)) return false;
            return true;
        }

        public bool ValidationSize(long fileSize) => Constants.FileSizeLimit > fileSize;

    }
}
