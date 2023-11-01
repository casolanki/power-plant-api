# Power Plant Production Plan API

This API calculates the power production plan for a set of power plants based on load and fuel cost considerations. It considers different power plant types, their efficiency, Pmin, and Pmax constraints, as well as the cost of fuel.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [Visual Studio](https://visualstudio.microsoft.com/) 
- [Docker](https://www.docker.com/)

## Build and Run

1]Clone the repository to your local machine: git clone https://github.com/your/repo.git
   
2]Change to the project directory: **cd PowerPlantAPI**

3]Build the project using the .NET Core CLI: **dotnet build**

4]Run the application : **dotnet run**

Alternatively, you can use Visual Studio to open the solution file and run the project from the IDE.

The API will be accessible at http://localhost:8888, Futhermore swaggger json at https://localhost:7245/swagger/
You can make POST requests to the /productionplan/cal-prod-plan/ endpoint with the payload to calculate power production plans.


# Dockerize:

1]Build the Docker image: **docker build -t power-plant-api .**

2]Run the Docker container: **docker run --rm -it -p 8080:80 power-plant-api:latest**

This will expose the API on port 8080 within the Docker container.
The API will be accessible at http://localhost:8080. You can make POST requests to the /productionplan endpoint with the payload to calculate power production plans.

3]API Usage
Calculate Power Production Plan
Endpoint: /productionplan

Method: POST

Payload: Provide a JSON payload in the request body with the load and power plant information.

# Example Payload:
{
  "load": 910,
  "fuels": {
    "gas(euro/MWh)": 13.4,
    "kerosine(euro/MWh)": 50.8,
    "co2(euro/ton)": 20,
    "wind(%)": 60
  },
  "powerplants": [
    {
      "name": "gasfiredbig1",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "gasfiredbig2",
      "type": "gasfired",
      "efficiency": 0.53,
      "pmin": 100,
      "pmax": 460
    },
    {
      "name": "gasfiredsomewhatsmaller",
      "type": "gasfired",
      "efficiency": 0.37,
      "pmin": 40,
      "pmax": 210
    },
    // Add more power plant entries here
  ]
}
Response: The API will return a JSON response with the calculated power production plan.

# Example Response:
[
    {
        "name": "windpark1",
        "p": 90.0
    },
    {
        "name": "windpark2",
        "p": 21.6
    },
    {
        "name": "gasfiredbig1",
        "p": 460.0
    },
    {
        "name": "gasfiredbig2",
        "p": 338.4
    },
    {
        "name": "gasfiredsomewhatsmaller",
        "p": 0.0
    },
    {
        "name": "tj1",
        "p": 0.0
    }
]

