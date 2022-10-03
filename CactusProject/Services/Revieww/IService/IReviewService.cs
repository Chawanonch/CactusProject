using CactusProject.ViewModels;

namespace CactusProject.Services.Revieww.IService
{
    public interface IReviewService
    {
        Task<string> CreateReview(ReviewVM data);
    }
}
