targetScope = 'subscription'

param systemName string
param environmentName string
param locationAbbreviation string
param location string = deployment().location
param integrationResourceGroup string
param containerVersion string

param storageTables array

var targetResourceGroupName = '${systemName}-${environmentName}-${locationAbbreviation}'
var defaultResourceName = targetResourceGroupName

resource targetResourceGroup 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: targetResourceGroupName
  location: location
}

module serviceResources 'resources.bicep' = {
  name: 'serviceResourcesModule'
  scope: targetResourceGroup
  params: {
    defaultResourceName: defaultResourceName
    environmentName: environmentName
    integrationResourceGroup: integrationResourceGroup
    location: location
    storageTables: storageTables
    containerVersion: containerVersion
  }
}
