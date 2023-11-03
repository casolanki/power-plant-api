namespace API.DTOs
{
    public class PowerPlant
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Efficiency { get; set; }
        public int Pmin { get; set; }
        public int Pmax { get; set; }
    }
}