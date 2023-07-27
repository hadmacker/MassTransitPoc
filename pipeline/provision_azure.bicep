param sbname string
param location string = resourceGroup().location

module sbmodule 'modules/servicebus.bicep' = {
  name: 'deploy_${sbname}'
  params: {
    sbname:sbname
    location:location
  }
}
