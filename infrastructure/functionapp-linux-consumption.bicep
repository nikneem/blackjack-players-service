param defaultResourceName string
param location string = resourceGroup().location

param storageAccountConnectionString string

param linuxFxVersion string = 'DOTNET-ISOLATED|7.0'

param corsOrigins array
param corsSupportCredentials bool = false

param appSettings array = [
  {
    name: 'AzureWebJobsStorage'
    value: storageAccountConnectionString
  }
  {
    name: 'FUNCTIONS_EXTENSION_VERSION'
    value: '~4'
  }
  {
    name: 'FUNCTIONS_WORKER_RUNTIME'
    value: 'dotnet-isolated'
  }
]

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${defaultResourceName}-plan'
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    capacity: 1
  }
  kind: 'functionapp'
}

resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${defaultResourceName}-func'
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    enabled: true
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
    }
  }
}

resource appConfig 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'web'
  parent: functionApp
  properties: {
    linuxFxVersion: linuxFxVersion
    ftpsState: 'Disabled'
    minTlsVersion: '1.2'
    http20Enabled: true
    cors: {
      allowedOrigins: corsOrigins
      supportCredentials: corsSupportCredentials
    }
    appSettings: appSettings
  }
}

output planResourceName string = appServicePlan.name
output appResourceName string = functionApp.name
output appIdentity string = functionApp.identity.principalId