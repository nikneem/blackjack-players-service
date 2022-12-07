param defaultResourceName string
param environmentName string
param location string = resourceGroup().location

param integrationResourceGroupName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: uniqueString(defaultResourceName)
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

module storageAccountSecret 'keyvault-secret.bicep' = {
  name: 'storageAccountSecretModule'
  scope: resourceGroup(integrationResourceGroupName)
  params: {

    defaultResourceName: defaultResourceName
    integrationResourceGroupName: integrationResourceGroupName
    secretValue: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
  }
}
var x = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'

module functionApp 'functionapp-linux-consumption.bicep' = {
  name: 'functionAppModule'
  params: {
    corsOrigins: [
      'http://localhost:4200'
    ]
    defaultResourceName: defaultResourceName
    storageAccountConnectionString: x
    location: location
  }
}

module roleAssignments 'config-and-secrets-role-assignments.bicep' = {
  name: 'roleAssignmentsModule'
  params: {
    containerAppPrincipalId: functionApp.outputs.appIdentity
    integrationResourceGroupName: integrationResourceGroupName
  }
}

output functionAppName string = functionApp.outputs.appResourceName
