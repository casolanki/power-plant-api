namespace API.Services
{
    public class PowerPlantService
    {
        public List<PowerPlantOutput> CalculateProductionPlan(Payload payload)
        {
            List<PowerPlantOutput> productionPlan = new List<PowerPlantOutput>(); 

            double windPowerAvailable = (payload.Fuels.Wind / 100.0) * payload.PowerPlants
            .Where(pp => pp.Type == "windturbine")
            .Sum(pp => pp.Pmax);

            double requiredLoad = payload.Load - windPowerAvailable;


            var gasPlants = payload.PowerPlants
                .Where(pp => pp.Type == "gasfired")
                .ToList();


            gasPlants.Sort((a, b) =>
                (payload.Fuels.Gas / a.Efficiency).CompareTo(payload.Fuels.Gas / b.Efficiency));

            foreach (var powerPlant in payload.PowerPlants)
            {
                double powerToProduce = 0.0;

                if (powerPlant.Type == "windturbine")
                {
                    powerToProduce = (powerPlant.Pmax * payload.Fuels.Wind / 100.0);
                }
                else if (powerPlant.Type == "gasfired")
                {
                    powerToProduce = Math.Max(powerPlant.Pmin, Math.Min(powerPlant.Pmax, requiredLoad));
                    requiredLoad -= powerToProduce;
                }
                else if (powerPlant.Type == "turbojet")
                {
                    powerToProduce = 0.0;
                }

                productionPlan.Add(new PowerPlantOutput
                {
                    Name = powerPlant.Name,
                    P = powerToProduce
                });
            }

            return productionPlan;
        }
    }
}