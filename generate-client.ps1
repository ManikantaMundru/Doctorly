# generate-client.ps1
# Reads from your existing Swashbuckle spec — no API code changes needed.
#
# Usage:
#   1. Start the API:  dotnet run --project src/DoctorScheduler.API
#   2. Run this:       pwsh generate-client.ps1

param(
    [string]$ApiUrl = "http://localhost:5265",
    [string]$OutputPath = "./client/DoctorScheduler.Client/Generated/DoctorSchedulerClient.cs"
)

Write-Host "Fetching spec from $ApiUrl/swagger/v1/swagger.json..." -ForegroundColor Cyan

nswag openapi2csclient `
  /input:"$ApiUrl/swagger/v1/swagger.json" `
  /namespace:DoctorScheduler.Client `
  /classname:DoctorSchedulerClient `
  /generateClientInterfaces:true `
  /injectHttpClient:true `
  /useSystemTextJson:true `
  /output:"$OutputPath"

Write-Host "Done. Client at $OutputPath" -ForegroundColor Green