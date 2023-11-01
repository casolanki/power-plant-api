namespace API.Controllers
{
    public class ProductionPlanController : BaseApiController
    {
        private readonly PowerPlantService _powerPlantService;

        public ProductionPlanController(PowerPlantService powerPlantService)
        {
            _powerPlantService = powerPlantService;
        }

        [HttpPost("cal-prod-plan")]
        public IActionResult CalculateProductionPlan([FromBody] Payload payload)
        {
            if (payload == null)
            {
                return BadRequest("Invalid payload.");
            }

            List<PowerPlantOutput> productionPlan = _powerPlantService.CalculateProductionPlan(payload);
            return Ok(productionPlan);
        }
    }
}