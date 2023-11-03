namespace API.Services
{
    public class PowerPlantService
    {
        public List<PowerPlantOutput> CalculateProductionPlan(Payload payload) 
        {
            List<PowerPlantOutput> productionPlan = new List<PowerPlantOutput>();

            // Calculate wind power
            decimal windPower = payload.PowerPlants
                .Where(pp => pp.Type == PowerPlantType.windturbine.ToString())
                .Sum(pp => pp.Pmax * (payload.Fuels.Wind / 100));

            // Allocate wind power to wind turbines
            foreach (var windPlant in payload.PowerPlants.Where(pp => pp.Type == PowerPlantType.windturbine.ToString()))
            {
                decimal windAllocation = (windPlant.Pmax * (payload.Fuels.Wind / 100));
                productionPlan.Add(new PowerPlantOutput
                {
                    Name = windPlant.Name,
                    P = Math.Round(windAllocation, 2)
                });
            }

            // Calculate the required load to be met by non-wind power plants
            decimal requiredLoad = payload.Load - windPower;

            // Filter and sort gas-fired and turbojet plants by efficiency
            var gasAndTurbojetPlants = payload.PowerPlants
                .Where(pp => pp.Type == PowerPlantType.gasfired.ToString() || pp.Type == PowerPlantType.turbojet.ToString())
                .OrderByDescending(pp => pp.Efficiency);

            // Distribute the remaining load to gas-fired and turbojet plants
            foreach (var plant in gasAndTurbojetPlants)
            {
                decimal efficiency = plant.Efficiency;
                decimal costPerMWh = 0.0m;

                if (plant.Type == PowerPlantType.gasfired.ToString())
                {
                    costPerMWh = payload.Fuels.Gas / efficiency;
                }
                else if (plant.Type == PowerPlantType.turbojet.ToString())
                {
                    costPerMWh = payload.Fuels.Kerosine / efficiency;
                }

                decimal power = 0.0m;

                if (requiredLoad > 0)
                {
                    power = Math.Max(plant.Pmin, Math.Min(requiredLoad, plant.Pmax));
                    requiredLoad -= power;
                }

                productionPlan.Add(new PowerPlantOutput
                {
                    Name = plant.Name,
                    P = Math.Round(power, 1),
                });
            }

            // Set remaining gas and turbojet plants to 0 power
            foreach (var plant in gasAndTurbojetPlants)
            {
                if (productionPlan.All(pp => pp.Name != plant.Name))
                {
                    decimal power = 0.0m;
                    productionPlan.Add(new PowerPlantOutput
                    {
                        Name = plant.Name,
                        P = Math.Round(power, 1),
                    });
                }
            }

            return productionPlan;
        }
    }
}