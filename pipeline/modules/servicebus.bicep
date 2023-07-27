//[Microsoft.ServiceBus/Namespace](https://learn.microsoft.com/en-us/azure/templates/microsoft.servicebus/namespaces?pivots=deployment-language-bicep#sbsku)

param sbname string
param location string = resourceGroup().location

resource sbnamespace 'Microsoft.ServiceBus/namespaces@2022-01-01-preview' = {
  name: sbname
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
  properties: {
  }
}

//[Microsoft.ServiceBus/Topics](https://learn.microsoft.com/en-us/azure/templates/microsoft.servicebus/namespaces/topics?pivots=deployment-language-bicep)
resource symbolicname 'Microsoft.ServiceBus/namespaces/topics@2022-01-01-preview' = {
  name: 'greeting'
  parent: sbnamespace
  properties: {
  }
}
