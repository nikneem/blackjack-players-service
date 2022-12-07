param containerAppPrincipalId string
param integrationResourceGroupName string

resource acrPullRole 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: resourceGroup()
  name: '7f951dda-4ed3-4680-a7ca-43fe172d538d'
}
module acrPullRoleAssignment 'roleAssignment.bicep' = {
  name: 'acrPullRoleAssignmentModule'
  scope: resourceGroup('Containers')
  params: {
    principalId: containerAppPrincipalId
    roleDefinitionId: acrPullRole.id
  }
}

resource storageAccountDataContributorRole 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: resourceGroup()
  name: '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3'
}
module storageAccountDataReaderRoleAssignment 'roleAssignment.bicep' = {
  name: 'storageAccountDataReaderRoleAssignmentModule'
  scope: resourceGroup()
  params: {
    principalId: containerAppPrincipalId
    roleDefinitionId: storageAccountDataContributorRole.id
  }
}

resource configurationDataReaderRole 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: resourceGroup()
  name: '516239f1-63e1-4d78-a4de-a74fb236a071'
}
module configurationReaderRoleAssignment 'roleAssignment.bicep' = {
  name: 'configurationReaderRoleAssignmentModule'
  scope: resourceGroup(integrationResourceGroupName)
  params: {
    principalId: containerAppPrincipalId
    roleDefinitionId: configurationDataReaderRole.id
  }
}

resource accessSecretsRole 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: resourceGroup()
  name: '4633458b-17de-408a-b874-0445c86b69e6'
}
module keyVaultSecretsAccessRoleAssignment 'roleAssignment.bicep' = {
  name: 'keyVaultSecretsAccessRoleAssignmentModule'
  scope: resourceGroup(integrationResourceGroupName)
  params: {
    principalId: containerAppPrincipalId
    roleDefinitionId: accessSecretsRole.id
  }
}
