namespace ClassroomManagerAPI.Models.Excels
{
    public class ImportAndValidateModel<T>
    {
        public IList<T> Datas { get; set; } = new List<T>();
        public Stream? Stream { get; set; }
    }
}
