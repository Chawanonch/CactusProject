using CactusProject.Data;
using CactusProject.Models;
using CactusProject.Services.Revieww.IService;
using CactusProject.Services.UploadFiless.IService;
using CactusProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Services.Revieww
{
    public class ReviewService : IReviewService
    {
        private readonly IUploadFileService uploadFileService;
        private readonly CactusContext productContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ReviewService(IUploadFileService uploadFileService, CactusContext productContext, IWebHostEnvironment webHostEnvironment)
        {
            this.uploadFileService = uploadFileService;
            this.productContext = productContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> CreateReview(ReviewVM data)
        {
            #region Check Image and UpLoadImage
            (string errorMessage, List<string> imageListName) = await UpLoadImage(data.Images!);
            if (!string.IsNullOrEmpty(errorMessage)) return errorMessage;
            #endregion

            #region New Review
            var newReview = new Review();
            newReview.Id = DateTime.Now.Ticks.ToString();
            newReview.UserId = data.UserId;
            newReview.CactusId = data.ProductId;
            newReview.Comment = data.Comment;
            newReview.Star = data.Star;
            newReview.CreateDate = DateTime.Now;

            await productContext.Reviews.AddAsync(newReview);
            #endregion

            #region Add New ReviewImage
            if (imageListName.Count > 0)
                foreach (var img in imageListName)
                {
                    var newReviewImage = new ReviewImage();
                    newReviewImage.Id = DateTime.Now.Ticks.ToString();
                    newReviewImage.Image = "/images/reviews/" + img;
                    newReviewImage.ReviewId = newReview.Id;
                    await productContext.ReviewImages.AddAsync(newReviewImage);
                }
            #endregion

            await productContext.SaveChangesAsync();
            return ("Success");
        }

        private async Task<(string errorMessage, List<string> imageListName)> UpLoadImage(IFormFileCollection formFiles)
        {
            string errorMessage = string.Empty;
            List<string> imageListName = new List<string>();
            if (uploadFileService.IsUpload(formFiles))
            {
                errorMessage = uploadFileService.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMessage))
                    imageListName = await uploadFileService.UploadImages(formFiles);
            }
            return (errorMessage, imageListName);
        }

    }
}
