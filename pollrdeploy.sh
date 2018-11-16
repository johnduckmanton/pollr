#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

declare subscriptionId=""
declare resourceGroupName=""
declare deploymentName=""
declare resourceGroupLocation=""

#login to azure using your credentials
az account show 1> /dev/null

if [ $? != 0 ];
then
	az login
fi

#set the default subscription id
az account set --subscription $subscriptionId

set +e


# Create a resource group
az group create --name tmp-pollr-test-rg --location "West Europe"

# Create a SQL Server
az sql server create --name tmppollrserver --resource-group tmp-pollr-test-rg --location "West Europe" --admin-user pollradmin --admin-password pollrPassw0rd

# Create a firewall rule to allow access to the server
az sql server firewall-rule create --resource-group tmp-pollr-test-rg --server tmppollrserver  --name tmpAllowYourIp --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# Create a database
az sql db create --resource-group tmp-pollr-test-rg --server tmppollrserver  --name pollr --service-objective S0

# connection string will be
#Server=tcp:tmppollrserver.database.windows.net,1433;Database=pollr;User ID=pollradmin;Password=pollrPassw0rd;Encrypt=true;Connection Timeout=30;

# Create a deployment user
az webapp deployment user set --user-name pollrops --password pollropsPassw0rd

# Create an app service plan
az appservice plan create --name tmppollrappserviceplan --resource-group tmp-pollr-test-rg --sku FREE

################################################
# Add signalr hub
################################################
az signalr create -n tmp-pollr-hub -g tmp-pollr-test-rg --sku Standard_S1 --unit-count 1

################################################
# Create a web app for the API
################################################
az webapp create --resource-group tmp-pollr-test-rg --plan tmppollrappserviceplan --name tmp-pollr-api --deployment-local-git

# Create environment variables
# Connection String
az webapp config connection-string set --resource-group tmp-pollr-test-rg --name tmp-pollr-api --settings PollrDatabase='Server=tcp:tmppollrserver.database.windows.net,1433;Database=pollr;User ID=pollradmin;Password=pollrPassw0rd;Encrypt=true;Connection Timeout=30;' --connection-string-type SQLServer

# Set the environment to Production
az webapp config appsettings set --name tmp-pollr-api --resource-group tmp-pollr-test-rg --settings ASPNETCORE_ENVIRONMENT="Production"

# SignalR__Azure__SignalR__ConnectionString
###### Update as required
#az webapp config appsettings set --name tmp-pollr-api --resource-group tmp-pollr-test-rg --settings SignalR__Azure__SignalR__ConnectionString="Endpoint=https://tmp-pollr-hub.service.signalr.net;AccessKey=MgdlZniHHD/TGroykFmanT/Dy/MwPQRslt3COZTwCrs=;Version=1.0;"

# SignalR__UseAzureSignalRManagedHub
az webapp config appsettings set --name tmp-pollr-api --resource-group tmp-pollr-test-rg --settings SignalR__UseAzureSignalRManagedHub="false"

################################################
# Create a web app for the admin ui
################################################
az webapp create --resource-group tmp-pollr-test-rg --plan tmppollrappserviceplan --name tmp-pollr-admin --deployment-local-git

# Create environment variables

# Set the environment to Production
az webapp config appsettings set --name tmp-pollr-admin --resource-group tmp-pollr-test-rg --settings ASPNETCORE_ENVIRONMENT="Production"

# Set web Api URL
az webapp config appsettings set --name tmp-pollr-admin --resource-group tmp-pollr-test-rg --settings ClientConfiguration__ApiUrl="https://tmp-pollr-api.azurewebsites.net/api"

# Set Signalr hub url
az webapp config appsettings set --name tmp-pollr-admin --resource-group tmp-pollr-test-rg --settings ClientConfiguration__HubUrl="https://tmp-pollr-api.azurewebsites.net/votehub"

# Set Vote URL
az webapp config appsettings set --name tmp-pollr-admin --resource-group tmp-pollr-test-rg --settings ClientConfiguration__VoteUrl="http://aka.ms/pollr"

################################################
# Create a web app for the vote ui
################################################
az webapp create --resource-group tmp-pollr-test-rg --plan tmppollrappserviceplan --name tmp-pollr --deployment-local-git

# Create environment variables

# Set the environment to Production
az webapp config appsettings set --name tmp-pollr --resource-group tmp-pollr-test-rg --settings ASPNETCORE_ENVIRONMENT="Production"

# Set web Api URL
az webapp config appsettings set --name tmp-pollr --resource-group tmp-pollr-test-rg --settings ClientConfiguration__ApiUrl="https://tmp-pollr-api.azurewebsites.net/api"

# Set Signalr hub url
az webapp config appsettings set --name tmp-pollr --resource-group tmp-pollr-test-rg --settings ClientConfiguration__HubUrl="https://tmp-pollr-api.azurewebsites.net/votehub"

# Set Angulr production mode
az webapp config appsettings set --name tmp-pollr --resource-group tmp-pollr-test-rg --settings ClientConfiguration__production="false"

###########
# Go to local PC and clone the repo
# For now, delete the .git directory then
# go to each subdirectory in turn and run
# git init
# git add .
# git commit -m "initial commit"
# git remote add azure https://pollrops@tmp-pollr-api.scm.azurewebsites.net/tmp-pollr-api.git
# git push azure master

#################################################
# Other manual sctivities
#################################################
#
# Turn on websockets on all web apps
#
# Create function app
# 
# Deploy function app code