using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class PaginatedResponse
    {
        public int TotalCount { get; set; }
        public int StartAt { get; set; }
        public int Count { get; set; }
        public IEnumerable<string> SortFields { get; set; }
        public string Error { get; set; }
    }

    public class PaginatedResponse<T> : PaginatedResponse
    {
        public PaginatedResponse()
        {
        }

        public PaginatedResponse(IEnumerable<T> data, int startAt = 0, int totalCount = 0, IEnumerable<string> sortFields = null, string errorMessage = null)
        {
            Data = data ?? Enumerable.Empty<T>();
            StartAt = startAt;
            Count = Data.Count();
            TotalCount = totalCount > 0 ? totalCount : Count;
            SortFields = sortFields;
            Error = errorMessage;
        }

        public PaginatedResponse(IEnumerable<T> data, PaginatedResponse baseResponse)
        {
            Data = data ?? Enumerable.Empty<T>();

            StartAt = baseResponse.StartAt;
            Count = Data.Count();
            TotalCount = baseResponse.TotalCount > 0 ? baseResponse.TotalCount : Count;
            SortFields = baseResponse.SortFields;
            Error = baseResponse.Error;
        }

        public IEnumerable<T> Data { get; set; }
    }
}
