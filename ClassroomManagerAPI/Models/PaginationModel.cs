namespace ClassroomManagerAPI.Models
{
    public class PaginationModel
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Next => Page + 1;
        public int Prev => Page - 1;
        public bool HasNext => Next < Total;
        public bool HasPrev => Prev > 1;    
    }
}
