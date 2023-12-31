# MassTransitPoc: Hangman variant

* Creates a State Machine Saga that is an instance of a Hangman game.
  * On Start: Generate a word randomly from a known list of answers (see answers.txt)
  * Guesses reveal a masked answer until all letters are guessed.
  * Max Guesses is 24. This allows for most puzzles to solve but a few to fail.
* Uses a single Player, which is a Consumer. Consumer has automated logic to guess and respond to saga state changes.
* Includes a Background Worker to start games: Press **"S"** to start a new game.


## Table of Contents

* [Prerequisites](#prerequisites)
* [Technical Reference](#technical-reference)
* [Provisioning Azure Resources](#provisioning-azure-resources)


# Prerequisites

## Tools

* [PowerShell 7](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)
* [Azure CLI](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/install#azure-cli)
* [Install Bicep Tools](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/install)
* [Enable Windows Long Paths](https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=registry)
  ```PowerShell
  New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem" `
  -Name "LongPathsEnabled" -Value 1 -PropertyType DWORD -Force
  ```
* If using Azure Service Bus, set the connection string in environment variable `mtpoccs` and restart (maybe just log out and back in?)


# Attributions

* Word List
  * Word list taken from [variances](https://www.kilgarriff.co.uk/BNClists/variances) of the [British National Corpus (BNC) database and word frequency lists](https://www.kilgarriff.co.uk/bnc-readme.html) with reductions and modifications.


# Technical Reference

## .NET

* [Fix for "Please use language version 1.0 or greater"](https://chanmingman.wordpress.com/2021/10/20/feature-global-using-directive-is-not-available-in-c-9-0-please-use-language-version-10-0-or-greater/)


## MassTransit

* [MassTransit: Getting Started](https://masstransit.io/quick-starts/in-memory)
  * `dotnet new --install MassTransit.Templates`
* [Quick Start: Azure Service Bus](https://masstransit.io/quick-starts/azure-service-bus)
  * `dotnet add package MassTransit.Azure.ServiceBus.Core`
* [Sagas](https://masstransit.io/documentation/patterns/saga) (Orchestration & Coordination)
* [Routing Slip](https://masstransit.io/documentation/patterns/routing-slip) (Choreography)
  * [EIP: Routing Slip Pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/RoutingTable.html)


# Provisioning Azure Resources

## One-Time Environment Setup (Manual Steps)

Scripts below assume that developer is deploying to DEV environment. If deploying to other environments, all references to the resource group will change.

[azcli: Get Started with Azure CLI](https://docs.microsoft.com/en-us/cli/azure/get-started-with-azure-cli#how-to-sign-into-the-azure-cli)

### Step 1: login to az cli

```
az login
```

### Step 2: Set active subscription

[azcli: Managing multiple subscriptions with AZ CLI](https://docs.microsoft.com/en-us/cli/azure/manage-azure-subscriptions-azure-cli)

Lists all subscriptions and sets a specific one. Useful if user has access to multiple subscriptions (i.e., Corporate, personal, MDN, etc.)
```
az account show --output table
az account subscription list --output table
az account set --subscription "<subscriptionid>"
az account show --output table
```

[azcli: Create Resource Group](https://docs.microsoft.com/en-us/cli/azure/group?view=azure-cli-latest#az-group-create)

```
az group create -l canadacentral -n "rgmasstransitpoc"
```

### Step 3: Provision Azure Resources with Bicep

[Deploy Bicep using AZ CLI](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/deploy-cli)

:exclamation: Remove `--what-if` from the script below when ready to deploy the Bicep script.

```
az deployment group create --what-if --resource-group "rgmasstransitpoc" --template-file pipeline\provision_azure.bicep --parameters "pipeline\parameters\azure-param-default.json"
```
