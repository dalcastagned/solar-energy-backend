using Canducci.Pagination;
using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Data.Dtos
{
    public class ReadPlants
    {
        public ReadPlants(Task<PaginatedRest<Plant>> plants)
        {
            PageCount = plants.Result.PageCount;
            PageNumber = plants.Result.PageNumber;
            PageSize = plants.Result.PageSize;
            TotalItemCount = plants.Result.TotalItemCount;
            HasPreviousPage = plants.Result.HasPreviousPage;
            HasNextPage = plants.Result.HasNextPage;
            IsFirstPage = plants.Result.IsFirstPage;
            IsLastPage = plants.Result.IsLastPage;
            Plants = plants.Result.Items.ToList();
        }
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public List<Plant> Plants { get; set; }
    }
}