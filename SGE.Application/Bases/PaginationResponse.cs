using System;
namespace SGE.Application.Pagination
{
	public class PaginationResponse<T>
	{
		public T? Data { get; set; }
		public int Total { get; set; }
		public int Cantidad { get; set; }
	}
}

	