namespace API.DTOs
{
    public class Payload
    {
        public int Load { get; set; }
        public Fuels Fuels { get; set; }
        public List<PowerPlant> PowerPlants { get; set; }
        
    }
}