using Canducci.Pagination;
using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadGenerations
    {
        public ReadGenerations(PaginatedRest<Generation> generation)
        {
            PageCount = generation.PageCount;
            PageNumber = generation.PageNumber;
            PageSize = generation.PageSize;
            TotalItemCount = generation.TotalItemCount;
            HasPreviousPage = generation.HasPreviousPage;
            HasNextPage = generation.HasNextPage;
            IsFirstPage = generation.IsFirstPage;
            IsLastPage = generation.IsLastPage;
            Generations = generation.Items.Select(g => new ReadGeneration(g)).ToList();
        }
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public List<ReadGeneration> Generations { get; set; }
    }
}