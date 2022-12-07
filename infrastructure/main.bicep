targetScope = 'subscription'

param systemName string
@allowed([
  'prod'
  'acc'
  'test'
  'dev'
])
param environmentName string
param location string = deployment().location
param locationAbbreviation string

param integrationResourceGroup string

var resourceGroupName = '${systemName}-${environmentName}-${locationAbbreviation}'
var defaultResourceName = resourceGroupName

resource targetResourceGroup 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: resourceGroupName
  location: location
}

module resourcesModule 'resources.bicep' = {
  name: 'resourcesModule'
  scope: targetResourceGroup
  params: {
    defaultResourceName: defaultResourceName
    environmentName: environmentName
    integrationResourceGroupName: integrationResourceGroup
    location: location
  }
}

output resourceGroupName string = targetResourceGroup.name
output functionAppName string = resourcesModule.outputs.functionAppName
